using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeBuilder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resume",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resume_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeSection_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Column = table.Column<int>(type: "integer", nullable: false),
                    Font = table.Column<string>(type: "text", nullable: false),
                    FontSize = table.Column<float>(type: "real", nullable: false),
                    LineHeight = table.Column<float>(type: "real", nullable: false),
                    SpaceBetweenElements = table.Column<int>(type: "integer", nullable: false),
                    LeftRightMargin = table.Column<int>(type: "integer", nullable: false),
                    IsPagenNumbersEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeSetting_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    CertificateUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CertificateLayout = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CertificateGridColumn = table.Column<int>(type: "integer", nullable: true),
                    CertificateRowSpacing = table.Column<string>(type: "text", nullable: true),
                    IsStartRowsWithBullet = table.Column<bool>(type: "boolean", nullable: false),
                    SubinfoStyle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolName = table.Column<string>(type: "text", nullable: true),
                    Degree = table.Column<string>(type: "text", nullable: true),
                    Major = table.Column<string>(type: "text", nullable: true),
                    GPA = table.Column<decimal>(type: "numeric", nullable: true),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsByOrder = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsByOrder = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageName = table.Column<string>(type: "text", nullable: true),
                    Proficiency = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    LanguageLayout = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LanguageGridColumn = table.Column<int>(type: "integer", nullable: true),
                    LanguageRowSpacing = table.Column<string>(type: "text", nullable: true),
                    IsStartRowsWithBullet = table.Column<bool>(type: "boolean", nullable: false),
                    SubinfoStyle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    SubTitle = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInformationEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ProfessionalTitle = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    LinkedIn = table.Column<string>(type: "text", nullable: true),
                    GitHub = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    PersonalInformationTextAlignment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PersonalInformationLayout = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PersonalInformationMarkerStyle = table.Column<string>(type: "text", nullable: true),
                    IsTitleProfessionalTitleOnSameLine = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInformationEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalInformationEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    SubTitle = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProjectUrl = table.Column<string>(type: "text", nullable: true),
                    RepositoryUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummaryEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummaryEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateEntry_ResumeSectionId",
                table: "CertificateEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationEntry_ResumeSectionId",
                table: "EducationEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceEntry_ResumeSectionId",
                table: "ExperienceEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageEntry_ResumeSectionId",
                table: "LanguageEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveEntry_ResumeSectionId",
                table: "ObjectiveEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformationEntry_ResumeSectionId",
                table: "PersonalInformationEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEntry_ResumeSectionId",
                table: "ProjectEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resume_UserId",
                table: "Resume",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSection_ResumeId",
                table: "ResumeSection",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSetting_ResumeId",
                table: "ResumeSetting",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummaryEntry_ResumeSectionId",
                table: "SummaryEntry",
                column: "ResumeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateEntry");

            migrationBuilder.DropTable(
                name: "EducationEntry");

            migrationBuilder.DropTable(
                name: "ExperienceEntry");

            migrationBuilder.DropTable(
                name: "LanguageEntry");

            migrationBuilder.DropTable(
                name: "ObjectiveEntry");

            migrationBuilder.DropTable(
                name: "PersonalInformationEntry");

            migrationBuilder.DropTable(
                name: "ProjectEntry");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ResumeSetting");

            migrationBuilder.DropTable(
                name: "SummaryEntry");

            migrationBuilder.DropTable(
                name: "ResumeSection");

            migrationBuilder.DropTable(
                name: "Resume");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
