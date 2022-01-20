using Newtonsoft.Json;
using Translator.Domain.Dtos;

namespace Translator.Domain.Commands
{
    public class TranslateRequestCommand : CommandBase<TranslationResponseDto>
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Source { get; set; }

        [JsonConstructor]
        public TranslateRequestCommand(string source, string sourceLanguage, string targetLanguage)
        {
            Source = source;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
        }
    }
}
