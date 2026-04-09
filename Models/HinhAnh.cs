using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDKTech.Models
{
    [Table("HinhAnh")]
    public class HinhAnh
    {
        [Key]
        public int MaHinhAnh { get; set; }

        public int MaSanPham { get; set; }

        [Required]
        [StringLength(300)]
        public string Url { get; set; }

        public bool IsDefault { get; set; } = false;

        [StringLength(200)]
        public string? AltText { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey("MaSanPham")]
        public virtual SanPham SanPham { get; set; }
    }
}
