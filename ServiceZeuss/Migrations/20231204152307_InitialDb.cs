using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceZeuss.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StrName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BiActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryFK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StrName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BiActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProduct_tblCategory_CategoryFK",
                        column: x => x.CategoryFK,
                        principalTable: "tblCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCategory_StrName",
                table: "tblCategory",
                column: "StrName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_CategoryFK",
                table: "tblProduct",
                column: "CategoryFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProduct");

            migrationBuilder.DropTable(
                name: "tblCategory");
        }
    }
}
