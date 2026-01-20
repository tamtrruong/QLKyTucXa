using QLKTX_DAO;
using QLKTX_DAO.Model.Entities; // Cập nhật namespace
using QLKTX_DTO.Bill;
using QLKTX_DTO.Tke;

namespace QLKTX_BUS
{
    public class Thongke_BUS
    {
        private readonly ThongKe_DAO dao;

        public Thongke_BUS(ThongKe_DAO dao)
        {
            this.dao = dao;
        }

        public async Task<List<DoanhThu_DTO>> GetDoanhThuNamAsync(int nam)
        {
            return await dao.GetDoanhThuNam(nam);
        }

        public async Task<List<HoaDon_DTO>> GetTyLeThanhToanAsync()
        {
            return await dao.GetThongKeTrangThaiHoaDon();
        }
    }
}