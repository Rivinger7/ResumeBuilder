using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class EducationEntryConfiguration : IEntityTypeConfiguration<EducationEntry>
{
    public void Configure(EntityTypeBuilder<EducationEntry> builder)
    {
        builder.ToTable(nameof(EducationEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.EducationEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
