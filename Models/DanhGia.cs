using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDKTech.Models
{
    [Table("DanhGia")]
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        public int MaSanPham { get; set; }

        // Liên kết với bảng NguoiDung (Identity) để biết ai đánh giá
        public string? IdNguoiDung { get; set; }

        [Required(ErrorMessage = "Vui lòng để lại nhận xét")]
        [StringLength(500)]
        public string NoiDung { get; set; }

        [Range(1, 5)]
        public int SoSao { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Quan hệ liên kết
        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; }

        [ForeignKey("IdNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}