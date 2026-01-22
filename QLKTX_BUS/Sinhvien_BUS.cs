using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities; // Cập nhật namespace
using QLKTX_DTO.SV;

namespace QLKTX_BUS
{
    public class Sinhvien_BUS
    {
        private readonly Sinhvien_DAO dao;
        private readonly IMapper map;

        public Sinhvien_BUS(Sinhvien_DAO dao, IMapper mapper)
        {
            this.dao = dao;
            map = mapper;
        }

        public async Task<List<SinhVien_DTO>> GetAllAsync()
        {
            var listEntity = await dao.GetAllAsync();
            return map.Map<List<SinhVien_DTO>>(listEntity);
        }

        public async Task<SinhVien_DTO?> GetByIdAsync(string maSV)
        {
            var entity = await dao.GetByIdAsync(maSV);
            return map.Map<SinhVien_DTO>(entity);
        }

        public async Task CreateAsync(CreateSV_DTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaSV))
                throw new Exception("Mã sinh viên không được để trống");
            if (await dao.ExistsAsync(dto.MaSV))
                throw new Exception("Mã sinh viên đã tồn tại");
            var entity = map.Map<sinh_vien>(dto);
            await dao.AddAsync(entity);
        }


        public async Task UpdateAsync(SinhVien_DTO dto)
        {
            var entity = map.Map<sinh_vien>(dto);
            await dao.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string maSV)
        {
            await dao.DeleteAsync(maSV);
        }
    }
} 