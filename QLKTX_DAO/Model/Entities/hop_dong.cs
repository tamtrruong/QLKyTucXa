using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class hop_dong
{
    public string ma_hop_dong { get; set; } = null!;

    public string? ma_sv { get; set; }

    public string? ma_phong { get; set; }

    public DateTime ngay_bat_dau { get; set; }

    public DateTime ngay_ket_thuc { get; set; }

    public int so_thang { get; set; }

    public short? tinh_trang { get; set; }

    public virtual phong? ma_phongNavigation { get; set; }

    public virtual sinh_vien? ma_svNavigation { get; set; }
}
