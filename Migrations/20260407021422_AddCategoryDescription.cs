using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDKTech.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoTaDanhMuc",
                table: "DanhMuc",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoTaDanhMuc",
                table: "DanhMuc");
        }
    }
}
