using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestBooks.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BookID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(41)", maxLength: 41, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    datatime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookID", x => x.BookID);
                });

            migrationBuilder.CreateTable(
                name: "ReBook",
                columns: table => new
                {
                    ReBookID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    datatime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookID = table.Column<string>(type: "varchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReBookID", x => x.ReBookID);
                    table.ForeignKey(
                        name: "FK_ReBook_Book_BookID",
                        column: x => x.BookID,
                        principalTable: "Book",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReBook_BookID",
                table: "ReBook",
                column: "BookID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReBook");

            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
