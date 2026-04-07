using HDKTech.Repositories;
using HDKTech.Repositories.Interfaces;
using HDKTech.Models;
using Microsoft.AspNetCore.Mvc;

namespace HDKTech.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryRepository categoryRepo, IProductRepository productRepo, ILogger<CategoryController> logger)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int id, string sortBy = "featured", decimal? minPrice = null, decimal? maxPrice = null, int? brandId = null, string? cpuLine = null, string? vgaLine = null, string? ramType = null, int? status = null, int page = 1)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null) return RedirectToAction("Index", "Home");

            // Tìm danh mục root (cha cuối cùng)
            int rootCategoryId = id;
            var currentCategory = category;

            while (currentCategory.MaDanhMucCha.HasValue && currentCategory.MaDanhMucCha > 0)
            {
                var parent = await _categoryRepo.GetByIdAsync(currentCategory.MaDanhMucCha.Value);
                if (parent != null)
                {
                    rootCategoryId = parent.MaDanhMuc;
                    currentCategory = parent;
                }
                else
                {
                    break;
                }
            }

            // Lấy sản phẩm từ danh mục root
            var products = await _categoryRepo.GetProductsByCategoryAsync(rootCategoryId);

            // ===== APPLY FILTERS =====
            // Nếu user click vào danh mục con (filter category), áp dụng filter dựa vào loại danh mục
            if (id != rootCategoryId)
            {
                var filterCategory = category;
                var parentCategory = filterCategory.MaDanhMucCha.HasValue 
                    ? await _categoryRepo.GetByIdAsync(filterCategory.MaDanhMucCha.Value) 
                    : null;

                // Map từ parent category ID để xác định loại filter
                // ID 15 = Thương hiệu (Brand)
                if (parentCategory?.MaDanhMuc == 15)
                {
                    var brandName = filterCategory.TenDanhMuc.ToLower();
                    products = products.Where(p => 
                        p.HangSX?.TenHangSX?.ToLower().Contains(brandName) ?? false
                    ).ToList();
                }
                // ID 21 = Giá bán (Price)
                else if (parentCategory?.MaDanhMuc == 21)
                {
                    var priceRangeName = filterCategory.TenDanhMuc.ToLower();

                    if (priceRangeName.Contains("dưới") || priceRangeName.Contains("under"))
                    {
                        if (priceRangeName.Contains("15")) products = products.Where(p => p.Gia < 15000000).ToList();
                    }
                    else if (priceRangeName.Contains("15") && priceRangeName.Contains("20"))
                    {
                        products = products.Where(p => p.Gia >= 15000000 && p.Gia <= 20000000).ToList();
                    }
                    else if (priceRangeName.Contains("trên") || priceRangeName.Contains("above"))
                    {
                        if (priceRangeName.Contains("20")) products = products.Where(p => p.Gia > 20000000).ToList();
                    }
                }
                // ID 25 = CPU Intel
                else if (parentCategory?.MaDanhMuc == 25)
                {
                    var cpuName = filterCategory.TenDanhMuc;
                    products = products.Where(p => 
                        p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(cpuName)
                    ).ToList();
                }
                // ID 26 = VGA
                else if (parentCategory?.MaDanhMuc == 26)
                {
                    var vgaName = filterCategory.TenDanhMuc;
                    products = products.Where(p => 
                        p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(vgaName)
                    ).ToList();
                }
                // ID 27 = RAM
                else if (parentCategory?.MaDanhMuc == 27)
                {
                    var ramName = filterCategory.TenDanhMuc;
                    products = products.Where(p => 
                        p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(ramName)
                    ).ToList();
                }
            }

            // Filter từ dropdown/query params
            if (brandId.HasValue && brandId > 0)
                products = products.Where(p => p.MaHangSX == brandId.Value).ToList();

            if (minPrice.HasValue)
                products = products.Where(p => p.Gia >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Gia <= maxPrice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(cpuLine))
                products = products.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(cpuLine)).ToList();

            if (!string.IsNullOrWhiteSpace(vgaLine))
                products = products.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(vgaLine)).ToList();

            if (!string.IsNullOrWhiteSpace(ramType))
                products = products.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(ramType)).ToList();

            if (status.HasValue)
                products = products.Where(p => p.TrangThaiSanPham == status.Value).ToList();

            // Áp dụng sắp xếp
            products = ApplySorting(products, sortBy);

            // ===== PAGINATION =====
            const int pageSize = 8;
            int totalProducts = products.Count;
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var paginatedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var brands = await _productRepo.GetUniqueBrandsByCategory(id);
            var cpuLines = await _productRepo.GetUniqueCpuLines();

            ViewBag.CategoryName = category.TenDanhMuc;
            ViewBag.CategoryId = id;
            ViewBag.Brands = brands;
            ViewBag.CpuLines = cpuLines;
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalProducts = totalProducts;

            return View(paginatedProducts);
        }

        private List<SanPham> ApplySorting(List<SanPham> products, string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name_asc" => products.OrderBy(p => p.TenSanPham).ToList(),
                "name_desc" => products.OrderByDescending(p => p.TenSanPham).ToList(),
                "price_asc" => products.OrderBy(p => p.Gia).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Gia).ToList(),
                "new" => products.OrderByDescending(p => p.ThoiGianTaoSP).ToList(),
                _ => products.OrderByDescending(p => p.ThoiGianTaoSP).ToList()
            };
        }
    }
}