using Newtonsoft.Json;

namespace Translator.Domain.Dtos
{
    /// <summary>
    /// This is the result of a translation request.
    /// I keep the properties in a plain object for simplicity, but 
    /// this could be better if we split the class in three parts:
    ///  - Status { ErrorCode, Success }
    ///  - OriginalRequest { SourceLnag, TargetLang, Source}
    ///  - Result
    /// </summary>
    public class TranslationResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("sourcelang")]
        public string SourceLanguage { get; set; }
        [JsonProperty("targetlang")]
        public string TargetLanguage { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }


        public TranslationResponseDto WithSuccess()
        {
            Success = true;
            ErrorCode = "";
            return this;
        }

        public TranslationResponseDto WithError(string errorCode)
        {
            Success = false;
            ErrorCode = errorCode;
            return this;
        }
    }
}
