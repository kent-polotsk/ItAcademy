using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDatabase.Migrations
{
    /// <inheritdoc />
    public partial class addProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PositivityRate",
                table: "Articles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PositivityRate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Articles");
        }
    }
}
