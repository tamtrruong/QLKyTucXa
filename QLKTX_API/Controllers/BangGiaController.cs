using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QLKTX_DTO.BangGia;


[ApiController]
[Route("api/[controller]")]
public class BangGiaController : ControllerBase
{
    private readonly BangGia_BUS bus;

    public BangGiaController(BangGia_BUS bus)
    {
        this.bus = bus;
    }

    [HttpGet("hien-tai")]
    public async Task<IActionResult> GetHienTai()
    {
        var result = await bus.GetHienTaiAsync();
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await bus.GetAllAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBangGia_DTO dto)
    {
        await bus.CreateAsync(dto);
        return Ok("Tạo bảng giá mới thành công");
    }
}
