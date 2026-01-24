namespace QLKTX_DTO.BangGia
{
    public class CreateBangGia_DTO
    {

        public string LoaiPhong { get; set; } = null!;
        public decimal DonGiaPhong { get; set; }
        public decimal DonGiaDien { get; set; }
        public decimal DonGiaNuoc { get; set; }
        public decimal PhiRac { get; set; }     // 👈 thêm
        public DateTime? NgayApDung { get; set; } // optional
    }
}

