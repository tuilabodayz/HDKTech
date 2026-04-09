using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDKTech.Models // <-- PHẢI CÓ DÒNG NÀY
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string TenSanPham { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        public string? MoTaSanPham { get; set; }

        public string? ThongSoKyThuat { get; set; }

        public int MaDanhMuc { get; set; }
        public int MaHangSX { get; set; }
        public int TrangThaiSanPham { get; set; }
        // Thêm vào class SanPham
        public string? ThongTinBaoHanh { get; set; } = "24 Tháng"; // Mặc định 24 tháng
        public string? KhuyenMai { get; set; } // Lưu chuỗi khuyến mãi, cách nhau bởi dấu |
        public DateTime ThoiGianTaoSP { get; set; } = DateTime.Now;

        [ForeignKey("MaDanhMuc")]
        public virtual DanhMuc? DanhMuc { get; set; }

        [ForeignKey("MaHangSX")]
        public virtual HangSX? HangSX { get; set; }

        public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
        public virtual ICollection<ChiTietGioHang>? ChiTietGioHangs { get; set; }
        public virtual ICollection<HinhAnh>? HinhAnhs { get; set; }
        public virtual ICollection<KhoHang>? KhoHangs { get; set; }
        // Thêm dòng này vào class SanPham trong file SanPham.cs
        public virtual ICollection<DanhGia>? DanhGias { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? GiaNiemYet { get; set; }

        [NotMapped]
        public int PhanTramGiamGia
        {
            get
            {
                if (GiaNiemYet.HasValue && GiaNiemYet > Gia && GiaNiemYet > 0)
                {
                    return (int)Math.Round((double)((GiaNiemYet - Gia) / GiaNiemYet * 100));
                }
                return 0;
            }
        }
    }
} // <-- PHẢI ĐÓNG NGOẶC CHO NAMESPACE