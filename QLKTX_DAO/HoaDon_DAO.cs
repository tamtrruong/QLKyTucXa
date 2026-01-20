using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace QLKTX_DAO
{
    public class HoaDon_DAO
    {
        private readonly QLKTXContext context;

        public HoaDon_DAO(QLKTXContext context)
        {
            this.context = context;
        }

        public async Task<List<hoa_don>> GetByPhongAsync(string maPhong)
        {
            return await context.hoa_dons
                .Where(hd => hd.ma_phong == maPhong)
                .OrderByDescending(hd => hd.ky_hoa_don)
                .ToListAsync();
        }

        public async Task<hoa_don?> GetByPhongAndKyAsync(string maPhong, DateOnly kyHoaDon)
        {
            // Lưu ý: ky_hoa_don trong DB là Date, Entity nhận là DateOnly hoặc DateTime tùy cấu hình
            return await context.hoa_dons
                .FirstOrDefaultAsync(h => h.ma_phong == maPhong && h.ky_hoa_don == kyHoaDon);
        }

        public async Task<List<hoa_don>> GetHoaDonNoAsync()
        {
            return await context.hoa_dons
                .Where(hd => hd.trang_thai == 0)
                .Include(hd => hd.ma_phongNavigation)
                .OrderBy(hd => hd.ky_hoa_don)
                .ToListAsync();
        }

        public async Task<List<hoa_don>> GetAllAsync()
        {
            return await context.hoa_dons
                .Include(hd => hd.ma_phongNavigation)
                .OrderByDescending(hd => hd.ky_hoa_don)
                .ToListAsync();
        }

        public async Task CreateHoaDonAsync(hoa_don hoadon)
        {
            bool exists = await context.hoa_dons.AnyAsync(x => x.ma_phong == hoadon.ma_phong && x.ky_hoa_don == hoadon.ky_hoa_don);
            if (exists)
            {
                throw new Exception($"Phòng {hoadon.ma_phong} đã có hóa đơn cho kỳ này rồi.");
            }

            await context.hoa_dons.AddAsync(hoadon);
            await context.SaveChangesAsync();
        }

        public async Task ThanhToanAsync(string maHoaDon, byte phuongThucTT)
        {
            var hd = await context.hoa_dons.FindAsync(maHoaDon);
            if (hd == null) throw new Exception("Không tìm thấy hóa đơn.");
            if (hd.trang_thai == 1) throw new Exception("Hóa đơn này đã được thanh toán rồi.");

            hd.trang_thai = 1;
            hd.ngay_thanh_toan = DateTime.Now;
            hd.phuong_thuc_tt = (short)phuongThucTT; // DB smallint tương ứng short

            context.hoa_dons.Update(hd);
            await context.SaveChangesAsync();
        }
    }
}