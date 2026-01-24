using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace QLKTX_DAO
{
    public class Sinhvien_DAO
    {
        private readonly QLKTXContext _context;

        public Sinhvien_DAO(QLKTXContext context)
        {
            _context = context;
        }

        public async Task<List<sinh_vien>> GetAllAsync()
        {
            return await _context.sinh_viens
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<sinh_vien?> GetByIdAsync(string maSV)
        {
            return await _context.sinh_viens.FindAsync(maSV);
        }

        public async Task AddAsync(sinh_vien sv)
        {
            if (await ExistsAsync(sv.ma_sv))
            {
                throw new Exception("Mã sinh viên đã tồn tại!");
            }

            await _context.sinh_viens.AddAsync(sv);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(sinh_vien sv)
        {
            var existingSv = await _context.sinh_viens.FindAsync(sv.ma_sv);
            if (existingSv != null)
            {
                existingSv.ho_ten = sv.ho_ten;
                existingSv.ngay_sinh = sv.ngay_sinh;
                existingSv.gioi_tinh = sv.gioi_tinh;
                existingSv.sdt = sv.sdt;
                existingSv.dia_chi = sv.dia_chi;
                existingSv.lop = sv.lop;

                _context.sinh_viens.Update(existingSv);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string maSV)
        {
            var sv = await _context.sinh_viens.FindAsync(maSV);
            if (sv != null)
            {
                _context.sinh_viens.Remove(sv);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string maSV)
        {
            return await _context.sinh_viens.AnyAsync(x => x.ma_sv == maSV);
        }

    }
}