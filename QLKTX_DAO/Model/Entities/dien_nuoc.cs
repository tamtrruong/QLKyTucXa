using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class dien_nuoc
{
    public int id { get; set; }

    public string? ma_phong { get; set; }

    public DateOnly ky_ghi_nhan { get; set; }

    public int dien_cu { get; set; }

    public int dien_moi { get; set; }

    public int nuoc_cu { get; set; }

    public int nuoc_moi { get; set; }

    public DateTime? ngay_chot { get; set; }

    public virtual phong? ma_phongNavigation { get; set; }
}
