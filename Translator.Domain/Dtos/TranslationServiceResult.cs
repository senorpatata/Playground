namespace Translator.Domain.Dtos
{
    public class TranslationServiceResult
    {
        public bool Success { get; set; } // I'd use init {get; init} but i'm not using c# 9.0
        public string ErrorCode { get; set; }
        public string Result { get; set; }
    }
}
