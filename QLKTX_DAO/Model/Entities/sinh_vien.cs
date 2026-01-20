using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class sinh_vien
{
    public string ma_sv { get; set; } = null!;

    public string ho_ten { get; set; } = null!;

    public DateOnly? ngay_sinh { get; set; }

    public short? gioi_tinh { get; set; }

    public string? sdt { get; set; }

    public string? dia_chi { get; set; }

    public string? lop { get; set; }

    public virtual ICollection<hop_dong> hop_dongs { get; set; } = new List<hop_dong>();

    public virtual ICollection<tai_khoan> tai_khoans { get; set; } = new List<tai_khoan>();

    public virtual ICollection<vi_pham> vi_phams { get; set; } = new List<vi_pham>();
}
