using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities; // Namespace chứa entity vi_pham
using Microsoft.EntityFrameworkCore;
using QLKTX_DTO.Tke;

namespace QLKTX_DAO
{
    public class ViPham_DAO
    {
        private readonly QLKTXContext _context;

        public ViPham_DAO(QLKTXContext context)
        {
            _context = context;
        }

        // 1. Thêm vi phạm mới
        public async Task CreateViPham(vi_pham vp)
        {
            await _context.vi_phams.AddAsync(vp);
            await _context.SaveChangesAsync();
        }

        // 2. Đếm số lần vi phạm của sinh viên theo mức độ
        public async Task<int> CountViPhamByUser(string maSV, MucDoViPham mucDo)
        {
            // DB lưu smallint (short), Enum cần ép kiểu về short
            short mucDoDb = (short)mucDo;

            return await _context.vi_phams
                .CountAsync(v => v.ma_sv == maSV && v.muc_do == mucDoDb);
        }

        // 3. Lấy lịch sử vi phạm
        public async Task<List<vi_pham>> GetHistory(string maSV)
        {
            var query = _context.vi_phams
                .Include(v => v.ma_svNavigation) // Tên navigation property (thường là ma_svNavigation hoặc ma_sv_navigation)
                .OrderByDescending(v => v.ngay_vi_pham)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maSV))
            {
                query = query.Where(v => v.ma_sv == maSV);
            }

            return await query.ToListAsync();
        }
    }
}