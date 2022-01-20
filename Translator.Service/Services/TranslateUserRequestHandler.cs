using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Translator.Data.IRepositories;
using Translator.Domain.Commands;
using Translator.Domain.Dtos;
using Translator.Domain.Models;
using Translator.Service.Dxos;

namespace Translator.Service.Services
{
    public class TranslateUserRequestHandler : IRequestHandler<TranslateRequestCommand, TranslationResponseDto>
    {
        private readonly ITranslationRepository _translationRepository;
        private readonly ITranslatorService _translatorService;
        private readonly ITranslationDxos _translationDxos;
        private readonly IMediator _mediator;

        public TranslateUserRequestHandler(
            ITranslationRepository translationRepository,
            ITranslatorService translatorService,
            ITranslationDxos translationDxos,
            IMediator mediator)
        {
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
            _translationDxos = translationDxos ?? throw new ArgumentNullException(nameof(translationDxos));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TranslationResponseDto> Handle(TranslateRequestCommand request, CancellationToken cancellationToken)
        {
            // Just for fun, I search in the database if I did the same request, and return it
            // Just like somehow a cached request? 
            if (await _translationRepository.TranslationAlreadyExists(request.Source, request.SourceLanguage, request.TargetLanguage))
            {
                var storedTranslation = await _translationRepository.GetExistingTranslation(request.Source, request.SourceLanguage, request.TargetLanguage);
                return _translationDxos.MapTranslationDto(storedTranslation).WithSuccess();
            }

            var languages = _translatorService.GetAvailableLanguages();
            if(!languages.Contains(request.SourceLanguage) || !languages.Contains(request.TargetLanguage))
            {
                return _translationDxos.MapTranslationDto(request).WithError("NOT_AVAILABLE_LANG"); //This is a MAGIC STRING, OF COURSE SHOULD BE IN CONSTANTS
            }

            var translationServiceResult = _translatorService.Translate(new TranslationServiceInput()
            {
                Source = request.Source,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage
            });

            if (!translationServiceResult.Success)
            {
                // I always try to use early return approach to simplify nesting
                return _translationDxos.MapTranslationDto(request).WithError(translationServiceResult.ErrorCode);
            }

            var translation = new Translation(request.Source, request.SourceLanguage, request.TargetLanguage, translationServiceResult.Result);
            _translationRepository.Add(translation);

            if (await _translationRepository.SaveChangesAsync() == 0)
            {
                throw new ApplicationException();
            }
            await _mediator.Publish(new Domain.Events.TranslationCreatedEvent(translation.Id), cancellationToken);

            var translationDto = _translationDxos.MapTranslationDto(translation).WithSuccess();
            return translationDto;
        }
    }
}