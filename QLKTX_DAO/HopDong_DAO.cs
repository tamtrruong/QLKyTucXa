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

        public async Task<bool> IsSinhVienCoPhong(string maSV)
        {
            // tinh_trang = 1 (Đang ở), kiểm tra ngày kết thúc còn hạn
            return await _context.hop_dongs
                .AnyAsync(hd => hd.ma_sv == maSV && hd.tinh_trang == 1 && hd.ngay_ket_thuc > DateTime.Now);
        }

        public async Task<List<hop_dong>> GetAllAsync()
        {
            return await _context.hop_dongs
                .Include(h => h.sinh_vien)
                .Include(h => h.phong)
                .ToListAsync();
        }


        public async Task CreateHopDong(hop_dong hd)
        {
            // 1. Kiểm tra sinh viên có đang ở phòng khác không
            if (await IsSinhVienCoPhong(hd.ma_sv))
            {
                throw new Exception($"Sinh viên {hd.ma_sv} đang có hợp đồng hiệu lực tại phòng khác!");
            }

            // 2. Kiểm tra phòng còn chỗ không
            var phong = await _context.phongs.FindAsync(hd.ma_phong);
            if (phong == null) throw new Exception("Phòng không tồn tại.");

            if (phong.so_nguoi_hien_tai >= phong.suc_chua)
            {
                throw new Exception($"Phòng {hd.ma_phong} đã đủ người.");
            }

            await _context.hop_dongs.AddAsync(hd);
            await _context.SaveChangesAsync();
            // Trigger Postgres sẽ tự cập nhật số người
        }

        public async Task ThanhLyHopDong(string maHD)
        {
            var hd = await _context.hop_dongs.FindAsync(maHD);
            if (hd != null)
            {
                hd.tinh_trang = 1; // Hoặc 0 tùy quy định 'Đã thanh lý' của bạn
                hd.ngay_ket_thuc = DateTime.Now;

                _context.hop_dongs.Update(hd);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<hop_dong>> GetHopDongSapHetHan()
        {
            var today = DateTime.Now;
            var nextMonth = today.AddDays(30);

            return await _context.hop_dongs
                .Where(h => h.tinh_trang == 1 && h.ngay_ket_thuc <= nextMonth && h.ngay_ket_thuc >= today)
                .Include(h => h.ma_svNavigation)    // Kiểm tra tên navigation property trong file hop_dong.cs
                .Include(h => h.ma_phongNavigation) // Kiểm tra tên navigation property trong file hop_dong.cs
                .ToListAsync();
        }

    }
}