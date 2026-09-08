using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class LanguageEntryConfiguration : IEntityTypeConfiguration<LanguageEntry>
{
    public void Configure(EntityTypeBuilder<LanguageEntry> builder)
    {
        builder.ToTable(nameof(LanguageEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.LanguageEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.LanguageLayout)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}
