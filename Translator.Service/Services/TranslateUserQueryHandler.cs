using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Translator.Data.IRepositories;
using Translator.Domain.Dtos;
using Translator.Domain.Models;
using Translator.Domain.Queries;
using Translator.Service.Dxos;

namespace Translator.Service.Services
{
    /// <summary>
    /// This class is only for demo purposes. I mean, usually this is only for queries with short input parameters, but I've coded it
    /// to demonstrate that you can use both query and requesthandler. Or you can use dapper and do real query stuff. 
    /// So, 
    /// </summary>
    public class TranslateUserQueryHandler : IRequestHandler<GetTranslationQuery, TranslationResponseDto>
    {
        private readonly ITranslationRepository _translationRepository;
        private readonly ITranslationDxos _translationDxos;
        private readonly IMediator _mediator;

        public TranslateUserQueryHandler(ITranslationRepository translationRepository, IMediator mediator, ITranslationDxos translationDxos)
        {
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _translationDxos = translationDxos ?? throw new ArgumentNullException(nameof(translationDxos));
        }

        public async Task<TranslationResponseDto> Handle(GetTranslationQuery request, CancellationToken cancellationToken)
        {
            if (await _translationRepository.TranslationAlreadyExists(request.Source, request.SourceLanguage, request.TargetLanguage))
            {
                var storedTranslation = await _translationRepository.GetExistingTranslation(request.Source, request.SourceLanguage, request.TargetLanguage);
                return _translationDxos.MapTranslationDto(storedTranslation);
            }

            var result = "result from Query";
            var translation = new Translation(request.Source, request.SourceLanguage, request.TargetLanguage, result);

            _translationRepository.Add(translation);

            if (await _translationRepository.SaveChangesAsync() == 0)
            {
                throw new ApplicationException();
            }

            await _mediator.Publish(new Domain.Events.TranslationCreatedEvent(translation.Id), cancellationToken);

            var translationDto = _translationDxos.MapTranslationDto(translation);
            return translationDto;
        }
    }
}
