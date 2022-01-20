using System.Collections.Generic;
using Translator.Domain.Dtos;

namespace Translator.Service.Services
{
    public interface ITranslatorService
    {
        IEnumerable<string> GetAvailableLanguages();

        TranslationServiceResult Translate(TranslationServiceInput input);
    }
}
