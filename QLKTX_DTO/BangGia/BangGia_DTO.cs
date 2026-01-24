namespace QLKTX_DTO.BangGia
{
    public class BangGia_DTO
    {
        public int MaBangGia { get; set; }
        public string LoaiPhong { get; set; } = null!;
        public decimal DonGiaPhong { get; set; }
        public decimal DonGiaDien { get; set; }
        public decimal DonGiaNuoc { get; set; }
        public decimal PhiRac { get; set; }     // 👈 thêm
        public DateTime NgayApDung { get; set; }
        public bool DangSuDung { get; set; }
    }
}

