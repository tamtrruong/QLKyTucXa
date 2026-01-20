using Microsoft.EntityFrameworkCore;
using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;

namespace QLKTX_DAO
{
    public class DienNuoc_DAO
    {
        private readonly QLKTXContext _context;

        public DienNuoc_DAO(QLKTXContext context)
        {
            _context = context;
        }

        public async Task AddChiSo(dien_nuoc dn)
        {
            if (dn.dien_moi < dn.dien_cu || dn.nuoc_moi < dn.nuoc_cu)
            {
                throw new Exception("Chỉ số mới không được nhỏ hơn chỉ số cũ.");
            }
            // Kiểm tra trùng lặp trong kỳ
            bool exists = await _context.dien_nuocs
                .AnyAsync(x => x.ma_phong == dn.ma_phong && x.ky_ghi_nhan == dn.ky_ghi_nhan);

            if (exists) throw new Exception("Tháng này đã ghi điện nước rồi.");

            await _context.dien_nuocs.AddAsync(dn);
            await _context.SaveChangesAsync();
        }

        public async Task<dien_nuoc?> GetLastChiSo(string maPhong)
        {
            return await _context.dien_nuocs
                .Where(dn => dn.ma_phong == maPhong)
                .OrderByDescending(dn => dn.ky_ghi_nhan)
                .FirstOrDefaultAsync();
        }
    }
}