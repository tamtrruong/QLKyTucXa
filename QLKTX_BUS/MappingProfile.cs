using AutoMapper;
using QLKTX_DAO.Model.Entities;
using QLKTX_DTO.Auth;
using QLKTX_DTO.Bill;
using QLKTX_DTO.Hopdong;
using QLKTX_DTO.Rooms;
using QLKTX_DTO.SV;
using QLKTX_DTO.Tke;
using QLKTX_DTO.BangGia;

namespace QLKTX_BUS
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<sinh_vien, SinhVien_DTO>()
                .ForMember(d => d.GioiTinh,
                    o => o.MapFrom(s => (Genders?)s.gioi_tinh));
            CreateMap<CreateSV_DTO, sinh_vien>()
                .ForMember(d => d.ho_ten, o => o.MapFrom(s => s.HoTen))
                .ForMember(d => d.ma_sv, opt => opt.MapFrom(src => src.MaSV))
                .ForMember(d => d.gioi_tinh, o => o.MapFrom(s => (short)s.GioiTinh))
                .ForMember(d => d.ngay_sinh,
                    opt => opt.MapFrom(src =>
                        src.NgaySinh.HasValue
                            ? (DateOnly?)DateOnly.FromDateTime(src.NgaySinh.Value)
                            : null
                    ));          

            CreateMap<phong, Phong_DTO>().ReverseMap();
            CreateMap<toa_nha, ToaNha_DTO>().ReverseMap();

            CreateMap<hop_dong, HopDong_DTO>();
            // Map dữ liệu từ form tạo hợp đồng sang Entity
            CreateMap<CreateHopDong_DTO, hop_dong>()
                .ForMember(d => d.ma_hd, o => o.Ignore()) // identity
                .ForMember(d => d.trang_thai, o => o.MapFrom(_ => true))
                .ForMember(d => d.ngay_tao, o => o.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.ngay_bat_dau, o => o.MapFrom(s => s.NgayBatDau))
                .ForMember(d => d.ngay_ket_thuc, o => o.MapFrom(s => s.NgayKetThuc));
            CreateMap<GiaHanHopDong_DTO, hop_dong>()
                .ForMember(d => d.ngay_ket_thuc, o => o.MapFrom(s => s.NgayKetThuc))
                .ForAllOtherMembers(o => o.Ignore());




            // 4. HÓA ĐƠN & ĐIỆN NƯỚC (Folder Bill)
            CreateMap<hoa_don, HoaDon_DTO>()
            .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.ma_phongNavigation.ten_phong));
            CreateMap<CreateHD_DTO, hoa_don>()
                .ForMember(dest => dest.ma_phong, opt => opt.MapFrom(src => src.MaPhong));

            CreateMap<bang_gium, BangGia_DTO>()
                .ForMember(d => d.MaBangGia, o => o.MapFrom(s => s.ma_bang_gia))
                .ForMember(d => d.LoaiPhong, o => o.MapFrom(s => s.loai_phong))
                .ForMember(d => d.DonGiaPhong, o => o.MapFrom(s => s.don_gia_phong))
                .ForMember(d => d.DonGiaDien, o => o.MapFrom(s => s.don_gia_dien))
                .ForMember(d => d.DonGiaNuoc, o => o.MapFrom(s => s.don_gia_nuoc))
                .ForMember(d => d.PhiRac, o => o.MapFrom(s => s.phi_rac))
                .ForMember(d => d.NgayApDung, o => o.MapFrom(s => s.ngay_ap_dung))
                .ForMember(d => d.DangSuDung, o => o.MapFrom(s => s.dang_su_dung));
            CreateMap<CreateBangGia_DTO, bang_gium>()
                .ForMember(d => d.ma_bang_gia, o => o.Ignore())
                .ForMember(d => d.dang_su_dung, o => o.Ignore())
                .ForMember(d => d.ngay_ap_dung,
                    o => o.MapFrom(s => s.NgayApDung ?? DateTime.Now));


            // 5. VI PHẠM & THỐNG KÊ (Folder Tke)
            CreateMap<vi_pham, ViPham_DTO>().ReverseMap();

            CreateMap<Register_DTO, tai_khoan>()
                .ForMember(dest => dest.ten_dang_nhap,
                    opt => opt.MapFrom(src => src.TenDangNhap))
                .ForMember(dest => dest.mat_khau,
                    opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.MatKhau)))
                .ForMember(dest => dest.quyen,
                    opt => opt.MapFrom(src => (short)src.VaiTro))
                .ForMember(dest => dest.ma_sv,
                    opt => opt.MapFrom(src =>
                        src.VaiTro == QuyenNguoiDung.SinhVien
                            ? src.MaSV
                            : null));
            CreateMap<tai_khoan, LoginResponse>()
                .ForMember(dest => dest.TenDangNhap,opt => opt.MapFrom(src => src.ten_dang_nhap))
                .ForMember(dest => dest.MaSV,opt => opt.MapFrom(src => src.ma_sv))
                .ForMember(dest => dest.Quyen,opt => opt.MapFrom(src => (QuyenNguoiDung)src.quyen))
                .ForMember(dest => dest.HoTen,
                    opt => opt.MapFrom(src =>
                        (QuyenNguoiDung)src.quyen == QuyenNguoiDung.Admin
                            ? "Quản trị viên"
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
