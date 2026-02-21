using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class initDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PictureAlt = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    PictureTitle = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
