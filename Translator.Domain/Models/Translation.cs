using System;

namespace Translator.Domain.Models
{
    public class Translation : ModelBase
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string Result { get; set; }


        public Translation(string source, string sourceLang, string targetLang, string result)
        {
            Id = Guid.NewGuid();
            Source = source;
            SourceLang = sourceLang;
            TargetLang = targetLang;
            Result = result;
        }
    }
}