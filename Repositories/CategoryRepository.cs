using HDKTech.Data;
using HDKTech.Models;
using Microsoft.EntityFrameworkCore;

namespace HDKTech.Repositories
{
    public class CategoryRepository : GenericRepository<DanhMuc>
    {
        public CategoryRepository(HDKTechContext context) : base(context) { }

        public async Task<DanhMuc?> GetByIdWithProductsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.SanPhams)
                .ThenInclude(p => p.HinhAnhs)
                .FirstOrDefaultAsync(c => c.MaDanhMuc == id);
        }

        public async Task<List<DanhMuc>> GetAllWithProductsAsync()
        {
            return await _dbSet
                .Include(c => c.SanPhams)
                .ToListAsync();
        }

        public async Task<List<SanPham>> GetProductsByCategoryAsync(int categoryId)
        {
            // Lấy tất cả danh mục con (recursive) của categoryId
            var allChildIds = new List<int> { categoryId };
            var queue = new Queue<int>();
            queue.Enqueue(categoryId);

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                var children = await _context.DanhMucs
                    .Where(d => d.MaDanhMucCha == currentId)
                    .Select(d => d.MaDanhMuc)
                    .ToListAsync();

                foreach (var childId in children)
                {
                    if (!allChildIds.Contains(childId))
                    {
                        allChildIds.Add(childId);
                        queue.Enqueue(childId);
                    }
                }
            }

            // Lấy sản phẩm từ tất cả danh mục (cha + con + con của con...)
            return await _context.SanPhams
                .Where(p => allChildIds.Contains(p.MaDanhMuc))
                .Include(p => p.HinhAnhs)
                .Include(p => p.HangSX)
                .OrderByDescending(p => p.ThoiGianTaoSP)
                .ToListAsync();
        }
    }
}
