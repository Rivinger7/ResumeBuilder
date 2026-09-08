using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeBuilder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBGHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColorHeader",
                table: "ResumeSetting",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColorHeader",
                table: "ResumeSetting");
        }
    }
}
