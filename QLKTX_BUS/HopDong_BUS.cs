using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities; // Sửa namespace
using QLKTX_DTO.Hopdong;

namespace QLKTX_BUS
{
    public class HopDong_BUS
    {
        private readonly HopDong_DAO hddao;
        private readonly Phong_DAO phongdao;
        private readonly IMapper map;

        public HopDong_BUS(HopDong_DAO hopDongDAO, Phong_DAO phongDAO, IMapper mapper)
        {
            hddao = hopDongDAO;
            phongdao = phongDAO;
            map = mapper;
        }

        public async Task<List<HopDong_DTO>> GetAllAsync()
        {
            var list = await hddao.GetAllAsync();
            return map.Map<List<HopDong_DTO>>(list);
        }


        public async Task CreateHopDongAsync(CreateHopDong_DTO dto)
        {
            bool isHasRoom = await hddao.IsSinhVienCoPhong(dto.MaSV);
            if (isHasRoom) throw new Exception("Sinh viên này đang có hợp đồng hiệu lực!");

            // Map sang hop_dong
            var entity = map.Map<hop_dong>(dto);

            // Sửa tên thuộc tính
            entity.ma_hop_dong = $"HD_{dto.MaSV}_{dto.MaPhong}";
            entity.tinh_trang = 1; // 1: Hiệu lực

            await hddao.CreateHopDong(entity);
        }

        public async Task ThanhLyHopDongAsync(string maHD)
        {
            await hddao.ThanhLyHopDong(maHD);
        }

        public async Task<List<HopDong_DTO>> GetHopDongSapHetHanAsync()
        {
            var list = await hddao.GetHopDongSapHetHan();
            return map.Map<List<HopDong_DTO>>(list);
        }
    }
}