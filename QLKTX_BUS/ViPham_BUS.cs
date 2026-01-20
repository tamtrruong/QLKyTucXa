using AutoMapper;
using QLKTX_DAO;
using QLKTX_DAO.Model.Entities; // Namespace Entity
using QLKTX_DTO.Tke;

namespace QLKTX_BUS
{
    public class ViPham_BUS
    {
        private readonly ViPham_DAO _vpDao;
        private readonly HopDong_DAO _hdDao;
        private readonly IMapper _mapper;

        public ViPham_BUS(ViPham_DAO vpDao, HopDong_DAO hdDao, IMapper mapper)
        {
            _vpDao = vpDao;
            _hdDao = hdDao;
            _mapper = mapper;
        }

        public async Task<string> GhiNhanViPham(CreateViPham_DTO dto)
        {
            // 1. Map DTO sang Entity (vi_pham)
            var entity = _mapper.Map<vi_pham>(dto);

            // Set thời gian hiện tại nếu DTO không có
            entity.ngay_vi_pham = DateTime.Now;
            entity.muc_do = (short)dto.MucDo; // Ép kiểu Enum sang short

            // Lưu vào DB
            await _vpDao.CreateViPham(entity);

            // 2. LOGIC XỬ PHẠT TỰ ĐỘNG
            string thongBao = "Đã ghi nhận vi phạm.";

            // Trường hợp 1: Mức độ NẶNG (Giả sử Enum: 2 = Nang) -> Đuổi luôn
            if (dto.MucDo == MucDoViPham.Nang)
            {
                await ThanhLyHopDongCuaSV(dto.MaSV);
                thongBao += " Mức độ NẶNG -> Hệ thống đã tự động CHẤM DỨT HỢP ĐỒNG!";
            }
            // Trường hợp 2: Mức độ TRUNG BÌNH (Giả sử Enum: 1 = TrungBinh)
            else if (dto.MucDo == MucDoViPham.TrungBinh)
            {
                // Vì đã SaveChanges ở bước 1, nên hàm Count sẽ bao gồm cả lỗi vừa thêm
                int tongSoLan = await _vpDao.CountViPhamByUser(dto.MaSV, MucDoViPham.TrungBinh);

                if (tongSoLan >= 3)
                {
                    await ThanhLyHopDongCuaSV(dto.MaSV);
                    thongBao += $" Đây là lần thứ {tongSoLan} vi phạm Trung bình -> Hệ thống đã tự động CHẤM DỨT HỢP ĐỒNG!";
                }
                else
                {
                    thongBao += $" Đây là lần thứ {tongSoLan} vi phạm Trung bình. (Quá 3 lần sẽ bị buộc thôi học).";
                }
            }
            // Trường hợp 3: Nhẹ -> Chỉ nhắc nhở
            else
            {
                thongBao += " Hình thức: Nhắc nhở.";
            }

            return thongBao;
        }

        // Hàm phụ trợ: Tìm hợp đồng đang ở và cắt
        private async Task ThanhLyHopDongCuaSV(string maSV)
        {
            // Dùng hàm mới thêm ở HopDong_DAO
            var hdActive = await _hdDao.GetHopDongHienTaiCuaSV(maSV);

            if (hdActive != null)
            {
                // Gọi hàm thanh lý (truyền ma_hop_dong)
                await _hdDao.ThanhLyHopDong(hdActive.ma_hop_dong);
            }
        }

        public async Task<List<ViPham_DTO>> XemLichSu(string maSV)
        {
            var list = await _vpDao.GetHistory(maSV);
            return _mapper.Map<List<ViPham_DTO>>(list);
        }
    }
}