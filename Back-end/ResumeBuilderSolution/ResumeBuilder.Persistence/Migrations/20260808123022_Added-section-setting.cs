using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeBuilder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addedsectionsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BackgroundColorHeader",
                table: "ResumeSetting",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionTitleColor",
                table: "ResumeSetting",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "SectionTitleFontSize",
                table: "ResumeSetting",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionTitleColor",
                table: "ResumeSetting");

            migrationBuilder.DropColumn(
                name: "SectionTitleFontSize",
                table: "ResumeSetting");

            migrationBuilder.AlterColumn<string>(
                name: "BackgroundColorHeader",
                table: "ResumeSetting",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
