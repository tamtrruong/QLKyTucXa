using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKTX_BUS;
using QLKTX_DTO.Bill;

namespace QLKTX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được nhập điện nước
    public class DienNuocController : ControllerBase
    {
        private readonly DienNuoc_BUS _bus;

        public DienNuocController(DienNuoc_BUS bus)
        {
            _bus = bus;
        }

        // POST: api/DienNuoc/record
        [HttpPost("record")]
        public async Task<IActionResult> GhiChiSo([FromBody] CreateHD_DTO dto)
        {
            try
            {
                await _bus.GhiChiSoAsync(dto);
                return Ok("Ghi chỉ số điện nước thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}