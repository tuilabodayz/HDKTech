using HDKTech.Data;
using HDKTech.Models;
using HDKTech.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HDKTech.Repositories
{
    public class ProductRepository : GenericRepository<SanPham>, IProductRepository
    {
        public ProductRepository(HDKTechContext context) : base(context) { }

        public async Task<List<SanPham>> GetAllWithImagesAsync()
        {
            return await _dbSet
                .Include(p => p.HinhAnhs)
                .Include(p => p.HangSX)
                .ToListAsync();
        }

        public async Task<SanPham?> GetProductWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.HinhAnhs)
                .Include(p => p.HangSX)
                .Include(p => p.DanhMuc)
                .Include(p => p.DanhGias)
                .ThenInclude(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
        }

        public async Task<List<SanPham>> GetRelatedProductsAsync(int categoryId, int currentProductId, int limit)
        {
            return await _dbSet
                .Where(p => p.MaDanhMuc == categoryId && p.MaSanPham != currentProductId)
                .Include(p => p.HinhAnhs)
                .Include(p => p.DanhMuc)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<SanPham>> FilterProductsAsync(ProductFilterModel filter)
        {
            var query = _dbSet
                .Include(p => p.HinhAnhs)
                .Include(p => p.HangSX)
                .AsQueryable();

            // Lọc theo danh mục
            if (filter.CategoryId.HasValue && filter.CategoryId > 0)
            {
                query = query.Where(p => p.MaDanhMuc == filter.CategoryId);
            }

            // Lọc theo hãng sản xuất
            if (filter.BrandId.HasValue && filter.BrandId > 0)
            {
                query = query.Where(p => p.MaHangSX == filter.BrandId);
            }

            // Lọc theo giá
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Gia >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Gia <= filter.MaxPrice);
            }

            // Lọc theo trạng thái (có hàng/hết hàng)
            if (filter.Status.HasValue)
            {
                query = query.Where(p => p.TrangThaiSanPham == filter.Status);
            }

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
            {
                query = query.Where(p => p.TenSanPham.Contains(filter.SearchKeyword));
            }

            // Lọc theo CPU (dùng ThongSoKyThuat hoặc thêm cột riêng)
            if (!string.IsNullOrWhiteSpace(filter.CpuLine))
            {
                query = query.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(filter.CpuLine));
            }

            // Lọc theo VGA
            if (!string.IsNullOrWhiteSpace(filter.VgaLine))
            {
                query = query.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(filter.VgaLine));
            }

            // Lọc theo loại RAM
            if (!string.IsNullOrWhiteSpace(filter.RamType))
            {
                query = query.Where(p => p.ThongSoKyThuat != null && p.ThongSoKyThuat.Contains(filter.RamType));
            }

            // Sắp xếp
            query = ApplySortBy(query, filter.SortBy);

            return await query.ToListAsync();
        }

        private IQueryable<SanPham> ApplySortBy(IQueryable<SanPham> query, string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(p => p.TenSanPham),
                "name_desc" => query.OrderByDescending(p => p.TenSanPham),
                "price_asc" => query.OrderBy(p => p.Gia),
                "price_desc" => query.OrderByDescending(p => p.Gia),
                "new" => query.OrderByDescending(p => p.ThoiGianTaoSP),
                _ => query.OrderByDescending(p => p.ThoiGianTaoSP) // Mặc định: mới nhất
            };
        }

        public async Task<List<string>> GetUniqueBrandsByCategory(int categoryId)
        {
            return await _dbSet
                .Where(p => p.MaDanhMuc == categoryId)
                .Include(p => p.HangSX)
                .Select(p => p.HangSX.TenHangSX)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetUniqueCpuLines()
        {
            return await _dbSet
                .Where(p => p.ThongSoKyThuat != null)
                .Select(p => p.ThongSoKyThuat)
                .Distinct()
                .ToListAsync()
                .ContinueWith(t => ExtractUniqueCpuLines(t.Result));
        }

        private List<string> ExtractUniqueCpuLines(List<string>? specs)
        {
            var cpuLines = new HashSet<string>();
            var cpuPatterns = new[] { "i3", "i5", "i7", "i9", "Ryzen 3", "Ryzen 5", "Ryzen 7", "Ryzen 9" };

            if (specs == null) return cpuLines.ToList();

            foreach (var spec in specs)
            {
                foreach (var pattern in cpuPatterns)
                {
                    if (spec?.Contains(pattern, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        cpuLines.Add(pattern);
                    }
                }
            }

            return cpuLines.ToList();
        }
    }
}