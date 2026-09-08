using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class ObjectiveEntryConfiguration : IEntityTypeConfiguration<ObjectiveEntry>
{
    public void Configure(EntityTypeBuilder<ObjectiveEntry> builder)
    {
        builder.ToTable(nameof(ObjectiveEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.ObjectiveEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
