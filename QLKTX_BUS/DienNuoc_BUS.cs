using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities; // Sửa namespace
using QLKTX_DTO.Bill;

namespace QLKTX_BUS
{
    public class DienNuoc_BUS
    {
        private readonly DienNuoc_DAO dao;
        private readonly IMapper map;

        public DienNuoc_BUS(DienNuoc_DAO dao, IMapper mapper)
        {
            this.dao = dao;
            map = mapper;
        }

        public async Task GhiChiSoAsync(CreateHD_DTO dto)
        {
            if (dto.DienMoi < dto.DienCu)
                throw new Exception("Chỉ số điện mới không được nhỏ hơn chỉ số cũ");

            if (dto.NuocMoi < dto.NuocCu)
                throw new Exception("Chỉ số nước mới không được nhỏ hơn chỉ số cũ");

            // Map sang entity dien_nuoc (snake_case)
            var entity = map.Map<dien_nuoc>(dto);

            // Sửa NgayChot -> ngay_chot
            entity.ngay_chot = DateTime.Now;

            await dao.AddChiSo(entity);
        }
    }
}