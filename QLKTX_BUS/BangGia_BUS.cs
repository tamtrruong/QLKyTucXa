using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities;
using QLKTX_DTO.BangGia;

public class BangGia_BUS
{
    private readonly BangGia_DAO dao;
    private readonly IMapper mapper;

    public BangGia_BUS(BangGia_DAO dao, IMapper mapper)
    {
        this.dao = dao;
        this.mapper = mapper;
    }

    public async Task<BangGia_DTO?> GetHienTaiAsync()
    {
        var entity = await dao.GetBangGiaHienTaiAsync();
        return entity == null ? null : mapper.Map<BangGia_DTO>(entity);
    }

    public async Task<List<BangGia_DTO>> GetAllAsync()
    {
        var list = await dao.GetAllAsync();
        return mapper.Map<List<BangGia_DTO>>(list);
    }

    public async Task CreateAsync(CreateBangGia_DTO dto)
    {
        // Tắt bảng giá cũ
        await dao.DisableAllAsync();

        var entity = mapper.Map<bang_gium>(dto);
        entity.dang_su_dung = true;

        await dao.AddAsync(entity);
    }
}
