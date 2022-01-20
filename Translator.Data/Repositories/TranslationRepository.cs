using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Translator.Data.IRepositories;
using Translator.Domain.Models;

namespace Translator.Data.Repositories
{
    public class TranslationRepository : Repository<Domain.Models.Translation>, ITranslationRepository
    {
        public TranslationRepository(TranslatorDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> TranslationAlreadyExists(string source, string sourceLang, string targetLang)
        {
            //TODO -> Duplicate Expression. Make
            // Use specification pattern
            //        public sealed class DirectSpecification<TEntity> : Specification<TEntity> where TEntity : class
            //{
            //    public DirectSpecification();
            //    public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria);

            //    public override Expression<Func<TEntity, bool>> IsSatisfiedBy();
            //}
           return await ModelDbSets.AsNoTracking().AnyAsync(e => 
            e.Source.Equals(source, StringComparison.CurrentCultureIgnoreCase) && 
            e.SourceLang == sourceLang &&
            e.TargetLang == targetLang);
        }

        public Task<Translation> GetExistingTranslation(string source, string sourceLang, string targetLang)
        {
            return ModelDbSets.FirstOrDefaultAsync(x =>
                x.Source.Equals(source, StringComparison.CurrentCultureIgnoreCase) && 
                x.SourceLang == sourceLang && 
                x.TargetLang == targetLang);
        }
    }
}