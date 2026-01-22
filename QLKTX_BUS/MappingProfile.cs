using AutoMapper;
using QLKTX_DAO.Model.Entities;
using QLKTX_DTO.Auth;
using QLKTX_DTO.Bill;
using QLKTX_DTO.Hopdong;
using QLKTX_DTO.Rooms;
using QLKTX_DTO.SV;
using QLKTX_DTO.Tke;

namespace QLKTX_BUS
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<sinh_vien, SinhVien_DTO>().ReverseMap();

            // Map cho Create (thường chỉ cần 1 chiều từ DTO -> Entity để lưu xuống DB)
            CreateMap<CreateSV_DTO, sinh_vien>();
            CreateMap<phong, Phong_DTO>().ReverseMap();
            CreateMap<toa_nha, ToaNha_DTO>().ReverseMap();

            CreateMap<hop_dong, HopDong_DTO>().ReverseMap();
            // Map dữ liệu từ form tạo hợp đồng sang Entity
            CreateMap<CreateHopDong_DTO, hop_dong>()
                .ForMember(dest => dest.ngay_bat_dau, opt => opt.MapFrom(src => src.NgayBatDau));
            CreateMap<GiaHanHopDong_DTO, hop_dong>();

            // 4. HÓA ĐƠN & ĐIỆN NƯỚC (Folder Bill)
            CreateMap<hoa_don, HoaDon_DTO>()
            .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.ma_phongNavigation.ten_phong));
            CreateMap<CreateHD_DTO, hoa_don>()
                .ForMember(dest => dest.ma_phong, opt => opt.MapFrom(src => src.MaPhong));

            // 5. VI PHẠM & THỐNG KÊ (Folder Tke)
            CreateMap<vi_pham, ViPham_DTO>().ReverseMap();

            CreateMap<Register_DTO, tai_khoan>()
                .ForMember(dest => dest.ten_dang_nhap, opt => opt.MapFrom(src => src.TenDangNhap))
                .ForMember(dest => dest.mat_khau, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.MatKhau)))
                .ForMember(dest => dest.quyen, opt => opt.MapFrom(src => (short)src.VaiTro))
                .ForMember(dest => dest.ma_sv, opt => opt.MapFrom(src => src.MaSV));

            CreateMap<tai_khoan, LoginResponse>()
                .ForMember(dest => dest.TenDangNhap,opt => opt.MapFrom(src => src.ten_dang_nhap))
                .ForMember(dest => dest.MaSV,opt => opt.MapFrom(src => src.ma_sv))
                .ForMember(dest => dest.Quyen,opt => opt.MapFrom(src => (QuyenNguoiDung)src.quyen))
                .ForMember(dest => dest.HoTen,opt => opt.MapFrom(src =>
                        src.quyen == 0
                            ? src.ho_ten
                            : src.ma_svNavigation != null
                                ? src.ma_svNavigation.ho_ten
                                : null
                ));
       

            // Map từ DTO tạo mới sang Entity
            CreateMap<CreateViPham_DTO, vi_pham>()
                .ForMember(dest => dest.ma_sv, opt => opt.MapFrom(src => src.MaSV))
                .ForMember(dest => dest.noi_dung, opt => opt.MapFrom(src => src.NoiDung))
                .ForMember(dest => dest.muc_do, opt => opt.MapFrom(src => (short)src.MucDo)) // Ép kiểu Enum sang short
                .ForMember(dest => dest.hinh_thuc_xu_ly, opt => opt.MapFrom(src => src.HinhThucXuLy))
                .ForMember(dest => dest.ghi_chu, opt => opt.MapFrom(src => src.GhiChu));

            CreateMap<vi_pham, ViPham_DTO>()
                .ForMember(dest => dest.MaSV, opt => opt.MapFrom(src => src.ma_sv))
                .ForMember(dest => dest.HoTenSV, opt => opt.MapFrom(src => src.ma_svNavigation.ho_ten)) // Lấy tên SV
                .ForMember(dest => dest.NoiDung, opt => opt.MapFrom(src => src.noi_dung))
                .ForMember(dest => dest.NgayViPham, opt => opt.MapFrom(src => src.ngay_vi_pham))
                .ForMember(dest => dest.MucDo, opt => opt.MapFrom(src => (MucDoViPham)src.muc_do)); // Ép kiểu short sang Enum
        }
    }
}
