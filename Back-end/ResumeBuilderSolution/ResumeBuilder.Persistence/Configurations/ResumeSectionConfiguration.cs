using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class ResumeSectionConfiguration : IEntityTypeConfiguration<ResumeSection>
{
    public void Configure(EntityTypeBuilder<ResumeSection> builder)
    {
        builder.ToTable(nameof(ResumeSection));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Resume)
            .WithMany(x => x.ResumeSections)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(100);
    }
}
