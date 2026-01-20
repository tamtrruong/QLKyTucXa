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

            var account = new tai_khoan
            {
                // Phải viết hoa chữ cái đầu theo đúng file Model/TaiKhoan.cs
                ten_dang_nhap = dto.TenDangNhap,
                mat_khau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
                quyen = (short)dto.VaiTro, // Ép kiểu về short cho khớp Postgres SmallInt
                ma_sv = dto.MaSV
            };

            await dao.AddAccountAsync(account);
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await dao.GetByUsernameAsync(request.Username);
            if (user == null) return null;

            // ✅ Verify bcrypt
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.mat_khau
            );

            if (!isValidPassword)
                return null;

            return new LoginResponse
            {
                TenDangNhap = user.ten_dang_nhap,
                MaSV = user.ma_sv,
                HoTen = user.ma_svNavigation?.ho_ten,
                Quyen = (QuyenNguoiDung)(byte)user.quyen,
                Token = "jwt_se_tao_sau",
                Expiration = DateTime.UtcNow.AddHours(2)
            };
        }
    }
}