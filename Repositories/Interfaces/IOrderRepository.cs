using HDKTech.Models;

namespace HDKTech.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<DonHang>
    {
        /// <summary>
        /// Tạo đơn hàng mới và lưu vào database
        /// </summary>
        Task<DonHang> CreateOrderAsync(string userId, string tenNguoiNhan, string soDienThoai, 
                                       string diaChiGiaoHang, List<CartItem> items, decimal phiVanChuyen = 0);

        /// <summary>
        /// Lấy đơn hàng theo mã đơn hàng
        /// </summary>
        Task<DonHang> GetOrderByMaDonHangAsync(string maDonHangChuoi);

        /// <summary>
        /// Lấy tất cả đơn hàng của một user
        /// </summary>
        Task<IEnumerable<DonHang>> GetUserOrdersAsync(string userId);

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int maDonHang, int trangThaiMoi);

        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        Task<bool> DeleteOrderAsync(int maDonHang);
    }
}
