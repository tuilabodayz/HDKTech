using HDKTech.Data;
using HDKTech.Models;
using HDKTech.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HDKTech.Repositories
{
    public class OrderRepository : GenericRepository<DonHang>, IOrderRepository
    {
        public OrderRepository(HDKTechContext context) : base(context)
        {
        }

        public async Task<DonHang> CreateOrderAsync(string userId, string tenNguoiNhan, string soDienThoai, 
                                                    string diaChiGiaoHang, List<CartItem> items, decimal phiVanChuyen = 0)
        {
            // Tính tổng tiền
            var tongTien = items.Sum(x => x.Price * x.Quantity);

            // Tạo mã đơn hàng: #HDKxxxxxxxxx (HDK + timestamp)
            var maDonHangChuoi = $"HDK{DateTime.Now:yyyyMMddHHmmss}";

            var donHang = new DonHang
            {
                MaNguoiDung = userId,
                MaDonHangChuoi = maDonHangChuoi,
                TenNguoiNhan = tenNguoiNhan,
                SoDienThoaiNhan = soDienThoai,
                DiaChiGiaoHang = diaChiGiaoHang,
                TongTien = tongTien,
                PhiVanChuyen = phiVanChuyen,
                TrangThaiDonHang = 0, // Chờ xác nhận
                NgayDatHang = DateTime.Now,
                ChiTietDonHangs = new List<ChiTietDonHang>()
            };

            // Thêm chi tiết đơn hàng
            foreach (var item in items)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaSanPham = item.ProductId,
                    SoLuong = item.Quantity,
                    GiaBanLucMua = item.Price
                };
                donHang.ChiTietDonHangs.Add(chiTiet);
            }

            // Lưu vào database
            await _context.AddAsync(donHang);
            await _context.SaveChangesAsync();

            return donHang;
        }

        public async Task<DonHang> GetOrderByMaDonHangAsync(string maDonHangChuoi)
        {
            return await _context.Set<DonHang>()
                .Include(x => x.ChiTietDonHangs)
                .Include(x => x.NguoiDung)
                .FirstOrDefaultAsync(x => x.MaDonHangChuoi == maDonHangChuoi);
        }

        public async Task<IEnumerable<DonHang>> GetUserOrdersAsync(string userId)
        {
            return await _context.Set<DonHang>()
                .Include(x => x.ChiTietDonHangs)
                .Where(x => x.MaNguoiDung == userId)
                .OrderByDescending(x => x.NgayDatHang)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int maDonHang, int trangThaiMoi)
        {
            var donHang = await _context.Set<DonHang>().FindAsync(maDonHang);
            if (donHang == null)
                return false;

            donHang.TrangThaiDonHang = trangThaiMoi;
            _context.Update(donHang);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int maDonHang)
        {
            var donHang = await _context.Set<DonHang>()
                .Include(x => x.ChiTietDonHangs)
                .FirstOrDefaultAsync(x => x.MaDonHang == maDonHang);

            if (donHang == null)
                return false;

            _context.RemoveRange(donHang.ChiTietDonHangs);
            _context.Remove(donHang);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
