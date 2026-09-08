using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class ProjectEntryConfiguration : IEntityTypeConfiguration<ProjectEntry>
{
    public void Configure(EntityTypeBuilder<ProjectEntry> builder)
    {
        builder.ToTable(nameof(ProjectEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.ProjectEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
