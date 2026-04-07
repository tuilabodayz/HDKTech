using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDKTech.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update main categories
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Máy tính xách tay mạnh mẽ cho công việc và giải trí hàng ngày' WHERE MaDanhMuc = 1;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Gaming laptop với hiệu năng cao cho các tựa game AAA' WHERE MaDanhMuc = 2;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'PC chuyên dụng cho thiết kế đồ họa, video editing và content creation' WHERE MaDanhMuc = 3;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Bộ xử lý, mainboard, card đồ họa và các linh kiện chính' WHERE MaDanhMuc = 4;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Vỏ máy, nguồn điện và tản nhiệt chất lượng cao' WHERE MaDanhMuc = 5;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Ổ cứng, RAM, thẻ nhớ và các thiết bị lưu trữ' WHERE MaDanhMuc = 6;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Tai nghe, loa và các thiết bị âm thanh chuyên dụng' WHERE MaDanhMuc = 7;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Màn hình máy tính với độ phân giải và tốc độ cao' WHERE MaDanhMuc = 8;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Chuột, bàn phím, tay cầm và các phụ kiện máy tính' WHERE MaDanhMuc = 9;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Bàn, ghế gaming và đồ nội thất văn phòng' WHERE MaDanhMuc = 10;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Máy chơi game cầm tay và thiết bị giải trí di động' WHERE MaDanhMuc = 11;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Dịch vụ sửa chữa, bảo hành và tư vấn kỹ thuật' WHERE MaDanhMuc = 12;");

            // Update brand categories
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop ASUS - Hiệu năng tốt, thiết kế sang trọng' WHERE MaDanhMuc = 16;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop DELL - Mạnh mẽ và đáng tin cậy cho doanh vụ' WHERE MaDanhMuc = 17;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop HP - Cân bằng giữa hiệu năng và tính di động' WHERE MaDanhMuc = 18;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop LENOVO - Độ bền cao, pin lâu' WHERE MaDanhMuc = 19;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Apple MacBook - Thiết kế tinh tế, hiệu năng vượt trội' WHERE MaDanhMuc = 20;");

            // Update price range categories
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop dưới 15 triệu đồng - Giá rẻ, hiệu năng cơ bản' WHERE MaDanhMuc = 22;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop từ 15-20 triệu đồng - Cân bằng giá và hiệu năng' WHERE MaDanhMuc = 23;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Laptop trên 20 triệu đồng - Hiệu năng cao, tính năng đầy đủ' WHERE MaDanhMuc = 24;");

            // Update CPU categories
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Bộ xử lý Intel Core i3 - Phù hợp cho công việc cơ bản' WHERE MaDanhMuc = 26;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Bộ xử lý Intel Core i5 - Cân bằng giữa giá và hiệu năng' WHERE MaDanhMuc = 27;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Bộ xử lý Intel Core i7 - Hiệu năng cao cho công việc nặng' WHERE MaDanhMuc = 28;");
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = N'Intel Core Ultra - Công nghệ mới nhất, hiệu năng AI tích hợp' WHERE MaDanhMuc = 29;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert all descriptions to NULL
            migrationBuilder.Sql(@"UPDATE DanhMuc SET MoTaDanhMuc = NULL WHERE MaDanhMuc IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 16, 17, 18, 19, 20, 22, 23, 24, 26, 27, 28, 29);");
        }
    }
}
