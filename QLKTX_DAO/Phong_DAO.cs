using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace QLKTX_DAO
{
    public class Phong_DAO
    {
        private readonly QLKTXContext _context;

        public Phong_DAO(QLKTXContext context)
        {
            _context = context;
        }

        public async Task<List<phong>> GetAllPhongWithToaNha()
        {
            return await _context.phongs
                .Include(p => p.ma_toaNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // Tìm phòng trống (so_nguoi_hien_tai < suc_chua)
        public async Task<List<phong>> GetPhongTrongAsync()
        {
            return await _context.phongs
                .Where(p => p.so_nguoi_hien_tai < p.suc_chua && p.trang_thai == 0)
                .Include(p => p.ma_toaNavigation)
                .ToListAsync();
        }

        public async Task<phong?> GetByIdAsync(string maPhong)
        {
            return await _context.phongs.FindAsync(maPhong);
        }

        public async Task<List<phong>> GetByMaToaAsync(string maToa)
        {
            return await _context.phongs
                .Where(p => p.ma_toa == maToa)
                .ToListAsync();
        }
    }
}