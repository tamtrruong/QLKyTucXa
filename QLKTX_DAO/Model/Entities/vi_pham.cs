using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class vi_pham
{
    public int ma_vi_pham { get; set; }

    public string? ma_sv { get; set; }

    public string? noi_dung { get; set; }

    public short? muc_do { get; set; }

    public DateTime? ngay_vi_pham { get; set; }

    public string? hinh_thuc_xu_ly { get; set; }

    public string? ghi_chu { get; set; }

    public virtual sinh_vien? ma_svNavigation { get; set; }
}
