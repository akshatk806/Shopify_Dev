using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product_Management.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class addfav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "FavTable",
                columns: table => new
                {
                    FavId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductRefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavTable", x => x.FavId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavTable");

        }
    }
}
