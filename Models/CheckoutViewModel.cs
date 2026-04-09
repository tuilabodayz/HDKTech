using System.ComponentModel.DataAnnotations;

namespace HDKTech.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên")]
        public string TenNguoiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiaoHang { get; set; }

        [Display(Name = "Ghi chú đơn hàng")]
        public string GhiChu { get; set; }

        // Summary info (read-only)
        public decimal TongTien { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal TongCong => TongTien + PhiVanChuyen;
        public int SoSanPham { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }
}
