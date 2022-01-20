using Newtonsoft.Json;
using Translator.Domain.Dtos;

namespace Translator.Domain.Queries
{
    public class GetTranslationQuery : QueryBase<TranslationResponseDto>
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Source { get; set; }

        public GetTranslationQuery()
        {
        }

        [JsonConstructor]
        public GetTranslationQuery(string source, string sourceLanguage, string targetLanguage)
        {
            Source = source;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
        }
    }
}