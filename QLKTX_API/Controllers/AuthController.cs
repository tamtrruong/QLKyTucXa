using QLKTX_BUS;
using QLKTX_DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Auth_BUS _authBus;
    private readonly IConfiguration _config;

    public AuthController(Auth_BUS authBus, IConfiguration config)
    {
        _authBus = authBus;
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authBus.LoginAsync(request);
        if (user == null) return Unauthorized("Sai tài khoản hoặc mật khẩu");

        // Logic tạo JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        // Xác định Role string dựa trên Enum
        string roleClaim = user.Quyen == QuyenNguoiDung.Admin ? "Admin" : "SinhVien";

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.TenDangNhap), // Dùng TenDangNhap làm Name
                new Claim("MaSV", user.MaSV ?? ""),          // Lưu thêm MaSV để tiện lấy
                new Claim(ClaimTypes.Role, roleClaim)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register_DTO dto)
    {
        try
        {
            await _authBus.RegisterAsync(dto);
            return Ok("Đăng ký thành công");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePassword_DTO dto)
    {
        await authBus.ChangePasswordAsync(
            User.Identity!.Name!,
            dto.OldPassword,
            dto.NewPassword
        );

        return Ok("Đổi mật khẩu thành công");
    }
}