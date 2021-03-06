// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Translator.Data;

namespace Translator.Data.Migrations
{
    [DbContext(typeof(TranslatorDbContext))]
    partial class TranslatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Translator.Domain.Models.Translation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("translation_id")
                        .HasDefaultValueSql("newId()");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnName("translation_result")
                        .HasMaxLength(2048);

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnName("translation_source")
                        .HasMaxLength(2048);

                    b.Property<string>("SourceLang")
                        .IsRequired()
                        .HasColumnName("translation_sourcelang")
                        .HasMaxLength(255);

                    b.Property<string>("TargetLang")
                        .IsRequired()
                        .HasColumnName("translation_destlang")
                        .HasMaxLength(255);

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("translations");
                });
#pragma warning restore 612, 618
        }
    }
}
