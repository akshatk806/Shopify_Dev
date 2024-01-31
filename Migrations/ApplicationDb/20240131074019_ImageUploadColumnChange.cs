using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product_Management.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class ImageUploadColumnChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Products",
                newName: "ImageURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Products",
                newName: "ImagePath");
        }
    }
}
