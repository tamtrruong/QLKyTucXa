using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKTX_BUS;
using QLKTX_DTO.Bill;

namespace QLKTX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDon_BUS _bus;

        public HoaDonController(HoaDon_BUS bus)
        {
            _bus = bus;
        }

        // 1. Admin bấm nút "Tính tiền" hàng tháng
        [HttpPost("generate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMonthlyBill([FromBody] CreateHD_DTO dto)
        {
            try
            {
                await _bus.CreateHoaDonAsync(dto);
                return Ok("Đã xuất hóa đơn thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. Sinh viên thanh toán
        [HttpPatch("{maHoaDon}/pay")]
        public async Task<IActionResult> ThanhToan(string maHoaDon, [FromBody] PaymentMethod method)
        {
            try
            {
                // Gọi BUS
                await _bus.ThanhToanAsync(maHoaDon, method);
                return Ok(new { message = "Thanh toán thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 3. Xem danh sách nợ (Admin)
        [HttpGet("chua-thanh-toan")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUnpaid()
        {
            return Ok(await _bus.GetHoaDonChuaThanhToanAsync());
        }

        // 4. Xem lịch sử hóa đơn của 1 phòng
        [HttpGet("history/{maPhong}")]
        public async Task<IActionResult> GetHistory(string maPhong)
        {
            return Ok(await _bus.GetLichSuHoaDonAsync(maPhong));
        }
    }
}