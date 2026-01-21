using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities;
using QLKTX_DTO.Auth;
using BCrypt.Net;


namespace QLKTX_BUS
{
    public class Auth_BUS
    {
        private readonly TaiKhoan_DAO dao;
        private readonly IMapper map;

        public Auth_BUS(TaiKhoan_DAO dao, IMapper mapper)
        {
            this.dao = dao;
            map = mapper;
        }

        public async Task RegisterAsync(Register_DTO dto)
        {
            var existing = await dao.GetByUsernameAsync(dto.TenDangNhap);
            if (existing != null)
                throw new Exception("Tên đăng nhập đã tồn tại");

            var account = map.Map<tai_khoan>(dto);

            await dao.AddAccountAsync(account);
        }


        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await dao.GetByUsernameAsync(request.Username);
            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password,user.mat_khau);
            if (!isValid) return null;
            var response = map.Map<LoginResponse>(user);
            response.Token = "jwt_se_tao_sau";
            response.Expiration = DateTime.UtcNow.AddHours(2);

            return response;
        }

        public async Task ChangePassword(string username, string oldPass, string newPass)
        {
            var user = await dao.GetByUsernameAsync(username);

            if (user == null)
                throw new Exception("Tài khoản không tồn tại");

            bool isValid = BCrypt.Net.BCrypt.Verify(oldPass,user.mat_khau);
            if (!isValid)
                throw new Exception("Mật khẩu cũ không đúng");

            user.mat_khau = BCrypt.Net.BCrypt.HashPassword(newPass);

            await dao.UpdateAsync(user);        
        }
    }
}