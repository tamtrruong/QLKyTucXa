using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QLKTX_BUS;
using QLKTX_DTO.Hopdong;

namespace QLKTX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly HopDong_BUS _bus;

        public HopDongController(HopDong_BUS bus)
        {
            _bus = bus;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bus.GetAllAsync();
            return Ok(result);
        }


        // 1. Đăng ký hợp đồng mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHopDong_DTO dto)
        {
            try
            {
                await _bus.CreateHopDongAsync(dto);
                return Ok(new { message = "Đăng ký phòng thành công!" });
            }
            catch (Exception ex)
            {
                var loiChiTiet = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = "Lỗi Database: " + loiChiTiet });
            }
        }

        // 2. Thanh lý hợp đồng
        [HttpDelete("{maHD}")]
        public async Task<IActionResult> ThanhLy(string maHD)
        {
            try
            {
                await _bus.ThanhLyHopDongAsync(maHD);
                return Ok(new { message = "Đã thanh lý hợp đồng và trả phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi: " + ex.Message });
            }
        }

        // 3. Xem hợp đồng sắp hết hạn
        [HttpGet("sap-het-han")]
        public async Task<IActionResult> GetSapHetHan()
        {
            try
            {
                var list = await _bus.GetHopDongSapHetHanAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

    }
}