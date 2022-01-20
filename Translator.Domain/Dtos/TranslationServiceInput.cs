namespace Translator.Domain.Dtos
{
    public class TranslationServiceInput
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Source { get; set; }
    }
}
