using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class hoa_don
{
    public string ma_hoa_don { get; set; } = null!;

    public string? ma_phong { get; set; }

    public DateOnly ky_hoa_don { get; set; }

    public decimal tien_phong { get; set; }

    public decimal tien_dien { get; set; }

    public decimal tien_nuoc { get; set; }

    public decimal tien_phat { get; set; }

    public decimal? tong_tien { get; set; }

    public short? trang_thai { get; set; }

    public DateTime? ngay_thanh_toan { get; set; }

    public short? phuong_thuc_tt { get; set; }

    public DateTime? ngay_lap { get; set; }

    public virtual phong? ma_phongNavigation { get; set; }
}
