using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class ResumeSettingConfiguration : IEntityTypeConfiguration<ResumeSetting>
{
    public void Configure(EntityTypeBuilder<ResumeSetting> builder)
    {
        builder.ToTable(nameof(ResumeSetting));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Resume)
            .WithOne(x => x.Settings)
            .HasForeignKey<ResumeSetting>(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
