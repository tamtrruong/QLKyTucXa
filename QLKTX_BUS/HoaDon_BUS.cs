using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities;
using QLKTX_DTO.Bill;

namespace QLKTX_BUS
{
    public class HoaDon_BUS
    {
        private readonly HoaDon_DAO hddao;
        private readonly IMapper map;
        private readonly BangGia_DAO banggiadao;
        private readonly Phong_DAO phongdao;

        public HoaDon_BUS(HoaDon_DAO hoadonDAO, BangGia_DAO bangGiaDAO, Phong_DAO phongDAO, IMapper mapper)
        {
            hddao = hoadonDAO;
            banggiadao = bangGiaDAO;
            phongdao = phongDAO;
            map = mapper;
        }

        public async Task<List<HoaDon_DTO>> GetHoaDonChuaThanhToanAsync()
        {
            var list = await hddao.GetHoaDonNoAsync();
            return map.Map<List<HoaDon_DTO>>(list);
        }

        public async Task<List<HoaDon_DTO>> GetLichSuHoaDonAsync(string maPhong)
        {
            var list = await hddao.GetByPhongAsync(maPhong);
            return map.Map<List<HoaDon_DTO>>(list);
        }

        public async Task CreateHoaDonAsync(CreateHD_DTO dto)
        {
            // Lấy bảng giá (đã sửa return bang_gia)
            var bangGia = await banggiadao.GetBangGiaHienTaiAsync();
            if (bangGia == null) throw new Exception("Chưa có bảng giá áp dụng!");

            var phong = await phongdao.GetByIdAsync(dto.MaPhong);
            if (phong == null) throw new Exception("Phòng không tồn tại!");

            if (dto.DienMoi < dto.DienCu) throw new Exception("Chỉ số điện mới nhỏ hơn số cũ!");
            if (dto.NuocMoi < dto.NuocCu) throw new Exception("Chỉ số nước mới nhỏ hơn số cũ!");

            DateOnly kyHoaDon = new DateOnly(dto.Nam, dto.Thang, 1);
            var existBill = await hddao.GetByPhongAndKyAsync(dto.MaPhong, kyHoaDon);
            if (existBill != null) throw new Exception($"Hóa đơn tháng {dto.Thang}/{dto.Nam} của phòng này đã được tạo rồi.");

            // Map sang hoa_don
            var hoadon = map.Map<hoa_don>(dto);

            // --- GÁN DỮ LIỆU (Sửa thành snake_case) ---
            hoadon.ma_phong = dto.MaPhong;
            hoadon.ky_hoa_don = kyHoaDon;
            hoadon.ngay_lap = DateTime.Now;

            int soDien = dto.DienMoi - dto.DienCu;
            int soNuoc = dto.NuocMoi - dto.NuocCu;

            // Tính tiền dựa trên thuộc tính của bang_gia (snake_case)
            hoadon.tien_dien = soDien * bangGia.don_gia_dien;
            hoadon.tien_nuoc = soNuoc * bangGia.don_gia_nuoc;
            hoadon.tien_phong = bangGia.don_gia_phong;
            hoadon.tien_phat = bangGia.phi_rac;

            hoadon.trang_thai = 0; // 0: Chưa thanh toán
            hoadon.phuong_thuc_tt = null;
            hoadon.ngay_thanh_toan = null;

            // Format chuỗi
            hoadon.ma_hoa_don = $"HD_{dto.MaPhong}_{hoadon.ky_hoa_don:yyyyMM}";

            await hddao.CreateHoaDonAsync(hoadon);
        }

        public async Task ThanhToanAsync(string maHoaDon, PaymentMethod phuongThuc)
        {
            await hddao.ThanhToanAsync(maHoaDon, (byte)phuongThuc);
        }
    }
}