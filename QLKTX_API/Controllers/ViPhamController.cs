using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKTX_BUS;
using QLKTX_DTO.Tke;
using System.Security.Claims;

namespace QLKTX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc phải đăng nhập (có Token) mới vào được
    public class ViPhamController : ControllerBase
    {
        private readonly ViPham_BUS _bus;

        public ViPhamController(ViPham_BUS bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> GetViolations([FromQuery] string? maSV)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst("MaSV")?.Value;
            string maSV_CanXem = maSV;
            if (role == "SinhVien")
            {
                maSV_CanXem = currentUserId;
            }
            try
            {
                var result = await _bus.XemLichSu(maSV_CanXem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] // Chặn sinh viên tự ghi vi phạm
        public async Task<IActionResult> CreateViolation([FromBody] CreateViPham_DTO dto)
        {
            try
            {
                string ketQua = await _bus.GhiNhanViPham(dto);

                return Ok(new { message = ketQua });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi ghi nhận: " + ex.Message });
            }
        }
    }
}