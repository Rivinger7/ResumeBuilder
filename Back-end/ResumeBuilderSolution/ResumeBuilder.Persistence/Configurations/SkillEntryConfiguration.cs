using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class SkillEntryConfiguration : IEntityTypeConfiguration<SkillEntry>
{
    public void Configure(EntityTypeBuilder<SkillEntry> builder)
    {
        builder.ToTable(nameof(SkillEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.SkillEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.SkillLayout)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}
