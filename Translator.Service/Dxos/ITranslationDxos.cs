using Translator.Domain.Commands;
using Translator.Domain.Dtos;
using Translator.Domain.Models;

namespace Translator.Service.Dxos
{
    public interface ITranslationDxos
    {
        TranslationResponseDto MapTranslationDto(Translation translation);
        TranslationResponseDto MapTranslationDto(TranslateRequestCommand command);
    }
}