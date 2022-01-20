using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Translator.Domain.Dtos;

namespace Translator.Service.Services
{
    public class TranslatorService : ITranslatorService
    {
        private IDictionary<string, string> GetLanguages()
        {
            var dictio = new Dictionary<string, string>();
            dictio.Add("English", "en");
            dictio.Add("French", "fr");
            dictio.Add("German", "de");

            return dictio;
        }

        public IEnumerable<string> GetAvailableLanguages()
        {
            return GetLanguages().Keys;
        }

        public TranslationServiceResult Translate(TranslationServiceInput input)
        {
            if (ThereIsAtLeastOneUnavailableLanguage(input))
            {
                return new TranslationServiceResult()
                {
                    Success = false,
                    ErrorCode = "NO_AVAILABLE_LANG",
                    Result = ""
                };
            }

            string sourceLang = GetLanguages()[input.SourceLanguage];
            string targetLang = GetLanguages()[input.TargetLanguage];

            var success = true;
            var result = string.Empty;
            var errorCode = string.Empty;
            try
            {
                result = TranslateFromJsonPublicAPI(input.Source, sourceLang, targetLang);
            }
            catch (Exception ex)
            {
                // This should be logged in a base service providing base common methods
                errorCode = ex.Message;
                success = false;
            }

            return new TranslationServiceResult()
            {
                Success = success,
                ErrorCode = errorCode,
                Result = result
            };
        }

        private bool ThereIsAtLeastOneUnavailableLanguage(TranslationServiceInput input)
        {
            return !GetAvailableLanguages().Contains(input.SourceLanguage) || !GetAvailableLanguages().Contains(input.TargetLanguage);
        }

        private string TranslateFromJsonPublicAPI
          (string sourceText,
           string sourceLanguage,
           string targetLanguage)
        {
            string translation = string.Empty;

            // Download translation
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                        sourceLanguage,
                                        targetLanguage,
                                        sourceText);
            string outputFile = Path.GetTempFileName();
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                wc.DownloadFile(url, outputFile);
            }

            // Get translated text
            if (File.Exists(outputFile))
            {

                // Get phrase collection
                string text = File.ReadAllText(outputFile);
                int index = text.IndexOf(string.Format(",,\"{0}\"", sourceLanguage));
                if (index == -1)
                {
                    // Translation of single word
                    int startQuote = text.IndexOf('\"');
                    if (startQuote != -1)
                    {
                        int endQuote = text.IndexOf('\"', startQuote + 1);
                        if (endQuote != -1)
                        {
                            translation = text.Substring(startQuote + 1, endQuote - startQuote - 1);
                        }
                    }
                }
                else
                {
                    // Translation of phrase
                    text = text.Substring(0, index);
                    text = text.Replace("],[", ",");
                    text = text.Replace("]", string.Empty);
                    text = text.Replace("[", string.Empty);
                    text = text.Replace("\",\"", "\"");

                    // Get translated phrases
                    string[] phrases = text.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; (i < phrases.Count()); i += 2)
                    {
                        string translatedPhrase = phrases[i];
                        if (translatedPhrase.StartsWith(",,"))
                        {
                            i--;
                            continue;
                        }
                        translation += translatedPhrase + "  ";
                    }
                }

                // Fix up translation
                translation = translation.Trim();
                translation = translation.Replace(" ?", "?");
                translation = translation.Replace(" !", "!");
                translation = translation.Replace(" ,", ",");
                translation = translation.Replace(" .", ".");
                translation = translation.Replace(" ;", ";");
            }

            return translation;
        }
    }
}
