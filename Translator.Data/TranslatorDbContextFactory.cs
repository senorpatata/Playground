using Microsoft.EntityFrameworkCore;

namespace Translator.Data
{
    public class TranslatorDbContextFactory : DesignTimeDbContextFactory<TranslatorDbContext>
    {
        protected override TranslatorDbContext CreateNewInstance(DbContextOptions<TranslatorDbContext> options)
        {
            return new TranslatorDbContext(options);
        }
    }
}