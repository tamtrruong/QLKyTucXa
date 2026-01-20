using System.ComponentModel.DataAnnotations;

namespace QLKTX_DTO.Tke
{
    public class CreateViPham_DTO
    {
        [Required(ErrorMessage = "Vui lòng nhập mã sinh viên")]
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Nội dung vi phạm không được để trống")]
        public string NoiDung { get; set; }
        public string TenMucDo => MucDo switch
        {
            MucDoViPham.Nhe => "Nhẹ",
            MucDoViPham.TrungBinh => "Trung Bình",
            MucDoViPham.Nang => "Nặng",
            _ => "Khác"
        };
        public MucDoViPham MucDo { get; set; }

        public string HinhThucXuLy { get; set; } // Ví dụ: "Cảnh cáo", "Phạt tiền"

        public string? GhiChu { get; set; }

    }
}