using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class ExperienceEntryConfiguration : IEntityTypeConfiguration<ExperienceEntry>
{
    public void Configure(EntityTypeBuilder<ExperienceEntry> builder)
    {
        builder.ToTable(nameof(ExperienceEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.ExperienceEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
