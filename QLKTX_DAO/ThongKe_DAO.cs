using QLKTX_DAO.Model;
using Microsoft.EntityFrameworkCore;
using QLKTX_DTO.Tke;
using QLKTX_DTO.Bill;

namespace QLKTX_DAO
{
    public class ThongKe_DAO
    {
        private readonly QLKTXContext _context;

        public ThongKe_DAO(QLKTXContext context)
        {
            _context = context;
        }

        public async Task<List<DoanhThu_DTO>> GetDoanhThuNam(int nam)
        {
            // PostgreSQL: Gọi function trả về table
            // Tên function trong DB script của bạn là: sp_thong_ke_doanh_thu_theo_nam
            var result = await _context.Database
                .SqlQuery<DoanhThu_DTO>($"SELECT * FROM sp_thong_ke_doanh_thu_theo_nam({nam})")
                .ToListAsync();

            return result;
        }

        public async Task<List<HoaDon_DTO>> GetThongKeTrangThaiHoaDon()
        {
            // Giả sử bạn có function này trong DB. Nếu chưa có, bạn cần tạo nó trong PostgreSQL.
            // Cú pháp gọi vẫn là SELECT * FROM...
            FormattableString sql = $"SELECT * FROM sp_thong_ke_hoa_don_theo_trang_thai()";

            return await _context.Database
                .SqlQuery<HoaDon_DTO>(sql)
                .ToListAsync();
        }
    }
}