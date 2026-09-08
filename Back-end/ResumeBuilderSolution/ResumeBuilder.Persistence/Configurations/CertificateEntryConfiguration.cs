using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class CertificateEntryConfiguration : IEntityTypeConfiguration<CertificateEntry>
{
    public void Configure(EntityTypeBuilder<CertificateEntry> builder)
    {
        builder.ToTable(nameof(CertificateEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.CertificateEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CertificateLayout)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}
