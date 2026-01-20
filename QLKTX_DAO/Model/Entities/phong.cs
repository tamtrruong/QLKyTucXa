using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class phong
{
    public string ma_phong { get; set; } = null!;

    public string ten_phong { get; set; } = null!;

    public int? tang { get; set; }

    public int suc_chua { get; set; }

    public int so_nguoi_hien_tai { get; set; }

    public short? trang_thai { get; set; }

    public string? ma_toa { get; set; }

    public string? loai_phong { get; set; }

    public virtual ICollection<dien_nuoc> dien_nuocs { get; set; } = new List<dien_nuoc>();

    public virtual ICollection<hoa_don> hoa_dons { get; set; } = new List<hoa_don>();

    public virtual ICollection<hop_dong> hop_dongs { get; set; } = new List<hop_dong>();

    public virtual toa_nha? ma_toaNavigation { get; set; }
}
