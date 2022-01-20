using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Translator.Data.EntityConfigurations
{
    public class TranslatorConfiguration : IEntityTypeConfiguration<Domain.Models.Translation>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Translation> builder)
        {
            builder.ToTable("translations");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("translation_id").HasDefaultValueSql("newId()");

            builder.Property(b => b.Source).HasColumnName("translation_source").HasMaxLength(2048)
                .IsRequired();

            builder.Property(b => b.Result).HasColumnName("translation_result").HasMaxLength(2048).IsRequired();

            builder.Property(b => b.SourceLang).HasColumnName("translation_sourcelang").HasMaxLength(255).IsRequired();

            builder.Property(b => b.TargetLang).HasColumnName("translation_destlang").HasMaxLength(255).IsRequired();
        }
    }
}