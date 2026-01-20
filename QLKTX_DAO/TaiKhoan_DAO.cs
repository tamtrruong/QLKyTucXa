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

        public async Task ChangePassword(string username, string oldPass, string newPass)
        {
            // Trong DB của bạn, DbSet sẽ là 'tai_khoans' (số nhiều) 
            // và khóa chính là 'ten_dang_nhap'
            var user = await context.tai_khoans.FindAsync(username);

            if (user == null)
                throw new Exception("Tài khoản không tồn tại");

            // Kiểm tra mật khẩu (Sử dụng cột 'mat_khau' từ script DB)
            if (user.mat_khau != oldPass)
                throw new Exception("Mật khẩu cũ không đúng");

            user.mat_khau = newPass;

            context.tai_khoans.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task<tai_khoan?> GetByUsernameAsync(string username)
        {
            // Sửa 'TenDangNhap' thành 'ten_dang_nhap' cho đúng với script DB
            return await context.tai_khoans
                .FirstOrDefaultAsync(u => u.ten_dang_nhap == username);
        }

        public async Task AddAccountAsync(tai_khoan account)
        {
            await context.tai_khoans.AddAsync(account);
            await context.SaveChangesAsync();
        }
    }
}