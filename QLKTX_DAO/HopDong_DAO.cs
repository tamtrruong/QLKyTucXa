using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace QLKTX_DAO
{
    public class HopDong_DAO
    {
        private readonly QLKTXContext _context;

        public HopDong_DAO(QLKTXContext context)
        {
            _context = context;
        }

        // Kiểm tra sinh viên có hợp đồng còn hiệu lực không
        public async Task<bool> IsSinhVienCoPhong(string maSV)
        {
            return await _context.hop_dongs
                .AnyAsync(hd =>
                    hd.ma_sv == maSV &&
                    hd.tinh_trang == 1 &&
                    hd.ngay_ket_thuc > DateTime.Now
                );
        }

        // Lấy tất cả hợp đồng + sinh viên + phòng
        public async Task<List<hop_dong>> GetAllAsync()
        {
            return await _context.hop_dongs
                .Include(h => h.ma_svNavigation)
                .Include(h => h.ma_phongNavigation)
                .ToListAsync();
        }

        // Tạo hợp đồng mới
        public async Task CreateHopDong(hop_dong hd)
        {
            // 1. Sinh viên đã có phòng chưa?
            if (await IsSinhVienCoPhong(hd.ma_sv!))
            {
                throw new Exception($"Sinh viên {hd.ma_sv} đang có hợp đồng hiệu lực!");
            }

            // 2. Phòng tồn tại không?
            var phong = await _context.phongs.FindAsync(hd.ma_phong);
            if (phong == null)
                throw new Exception("Phòng không tồn tại.");

            // 3. Phòng còn chỗ không?
            if (phong.so_nguoi_hien_tai >= phong.suc_chua)
            {
                throw new Exception($"Phòng {hd.ma_phong} đã đủ người.");
            }

            // 4. Set mặc định
            hd.tinh_trang = 1; // đang ở

            await _context.hop_dongs.AddAsync(hd);
            await _context.SaveChangesAsync();
            // Trigger DB sẽ tự cập nhật số người
        }

        // Thanh lý hợp đồng
        public async Task ThanhLyHopDong(string maHD)
        {
            var hd = await _context.hop_dongs.FindAsync(maHD);
            if (hd == null)
                throw new Exception("Hợp đồng không tồn tại.");

            hd.tinh_trang = 0; // ĐÃ THANH LÝ
            hd.ngay_ket_thuc = DateTime.Now;

            _context.hop_dongs.Update(hd);
            await _context.SaveChangesAsync();
        }

        // Hợp đồng sắp hết hạn (30 ngày)
        public async Task<List<hop_dong>> GetHopDongSapHetHan()
        {
            var today = DateTime.Now;
            var nextMonth = today.AddDays(30);

            return await _context.hop_dongs
                .Where(h =>
                    h.tinh_trang == 1 &&
                    h.ngay_ket_thuc >= today &&
                    h.ngay_ket_thuc <= nextMonth
                )
                .Include(h => h.ma_svNavigation)
                .Include(h => h.ma_phongNavigation)
                .ToListAsync();
        }

        public async Task<hop_dong?> GetHopDongHienTaiCuaSV(string MaSV)
        {
            return await _context.hop_dongs
                .Where(h => h.ma_sv == maSV && h.tinh_trang == 1)
                .OrderByDescending(h => h.ngay_ket_thuc)
                .FirstOrDefaultAsync();

        }

    }
}
