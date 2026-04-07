using HDKTech.Areas.Identity.Data;
using HDKTech.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HDKTech.Controllers
{
    public class BrandController : Controller
    {
        private readonly HDKTechContext _DbContext;

        public BrandController(HDKTechContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<IActionResult> Index(string slug = "")
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index", "Home");

            // Tìm danh mục
            var category = await _DbContext.HangSXs
                .FirstOrDefaultAsync(c => c.TenHangSX.Trim().ToLower() == slug.Trim().ToLower());

            if (category == null) return RedirectToAction("Index", "Home");

            // Lấy sản phẩm
            var products = await _DbContext.SanPhams
                                            .Where(p => p.MaDanhMuc == category.MaHangSX)
                                            .Include(p => p.HinhAnhs)
                                            .OrderByDescending(p => p.MaSanPham)
                                            .ToListAsync();

            ViewBag.CategoryName = category.MaHangSX;

            return View(products);
        }
    }
}
