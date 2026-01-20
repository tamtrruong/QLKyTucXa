using Microsoft.EntityFrameworkCore;
using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;

namespace QLKTX_DAO
{
    public class BangGia_DAO
    {
        private readonly QLKTXContext context;
        public BangGia_DAO(QLKTXContext ct) => this.context = ct;

        public async Task<bang_gium?> GetBangGiaHienTaiAsync()
        {
            return await context.bang_gia
                                 .Where(b => b.dang_su_dung == true)
                                 .OrderByDescending(b => b.ngay_ap_dung)
                                 .FirstOrDefaultAsync();
        }
    }
}