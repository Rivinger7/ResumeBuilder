using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Configurations;

public sealed class PersonalInformationEntryConfiguration : IEntityTypeConfiguration<PersonalInformationEntry>
{
    public void Configure(EntityTypeBuilder<PersonalInformationEntry> builder)
    {
        builder.ToTable(nameof(PersonalInformationEntry));

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ResumeSection)
            .WithMany(x => x.PersonalInformationEntries)
            .HasForeignKey(x => x.ResumeSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.PersonalInformationLayout)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.PersonalInformationTextAlignment)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.PersonalInformationMarkerStyle)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.PersonalInformationIconStyle)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}
