using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Translator.Domain.Queries;

namespace Translator.Service.Services
{
    /// <summary>
    /// Just the handler to get Available Languages.
    /// I don't think this comment have to be necessary. Documentation has to be maintained to and tend to be misaligned from sourcecode.
    /// So, I only write comments when neccesary (complex). and public API's. If you need to comment every single line, your code is not good enough.
    /// </summary>
    public class GetAvailableLanguagesHandler : IRequestHandler<GetAvailableLanguagesQuery, IEnumerable<string>>
    {
        private readonly ITranslatorService _translatorService;

        public GetAvailableLanguagesHandler(ITranslatorService translatorService)
        {
            _translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
        }

        public Task<IEnumerable<string>> Handle(GetAvailableLanguagesQuery request, CancellationToken cancellationToken)
        {
            var result = _translatorService.GetAvailableLanguages();
            return Task.FromResult(result);
        }
    }
}