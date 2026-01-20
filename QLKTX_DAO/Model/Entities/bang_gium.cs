using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class bang_gium
{
    public int ma_bang_gia { get; set; }

    public string loai_phong { get; set; } = null!;

    public decimal don_gia_phong { get; set; }

    public decimal don_gia_dien { get; set; }

    public decimal don_gia_nuoc { get; set; }

    public decimal phi_rac { get; set; }

    public DateTime? ngay_ap_dung { get; set; }

    public bool? dang_su_dung { get; set; }
}
