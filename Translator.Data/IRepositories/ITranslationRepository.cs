using System.Threading.Tasks;
using Translator.Domain.Models;

namespace Translator.Data.IRepositories
{
    public interface ITranslationRepository : IRepository<Translation>
    {
        Task<bool> TranslationAlreadyExists(string source, string sourceLang, string targetLang);
        Task<Translation> GetExistingTranslation(string source, string sourceLang, string targetLang);
    }
}