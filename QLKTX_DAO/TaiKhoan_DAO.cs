using Microsoft.EntityFrameworkCore;
using QLKTX_DAO.Model;
using QLKTX_DAO.Model.Entities;

namespace QLKTX_DAO
{
    public class TaiKhoan_DAO
    {
        private readonly QLKTXContext context;

        public TaiKhoan_DAO(QLKTXContext context)
        {
            this.context = context;
        }

        public async Task<tai_khoan?> GetByUsernameAsync(string username)
        {
             return await context.tai_khoans
                    .Include(x => x.ma_svNavigation)
                    .FirstOrDefaultAsync(x => x.ten_dang_nhap == username);
        }

        public async Task AddAccountAsync(tai_khoan account)
        {
            await context.tai_khoans.AddAsync(account);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(tai_khoan account)
        {
            context.tai_khoans.Update(account);
            await context.SaveChangesAsync();
        }
    }
}