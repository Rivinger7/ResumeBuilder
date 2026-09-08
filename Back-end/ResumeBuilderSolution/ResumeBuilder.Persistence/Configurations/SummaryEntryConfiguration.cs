using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class SummaryEntryConfiguration : IEntityTypeConfiguration<SummaryEntry>
{
    public void Configure(EntityTypeBuilder<SummaryEntry> builder)
    {
        builder.ToTable(nameof(SummaryEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.SummaryEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
