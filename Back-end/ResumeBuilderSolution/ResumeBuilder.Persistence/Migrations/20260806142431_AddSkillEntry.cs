using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeBuilder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SkillLevel = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    SkillLayout = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SkillGridColumn = table.Column<int>(type: "integer", nullable: true),
                    SkillRowSpacing = table.Column<string>(type: "text", nullable: true),
                    IsStartRowsWithBullet = table.Column<bool>(type: "boolean", nullable: false),
                    SubinfoStyle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillEntry_ResumeSection_ResumeSectionId",
                        column: x => x.ResumeSectionId,
                        principalTable: "ResumeSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillEntry_ResumeSectionId",
                table: "SkillEntry",
                column: "ResumeSectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillEntry");
        }
    }
}
