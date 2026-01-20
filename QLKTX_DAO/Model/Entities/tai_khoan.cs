using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class tai_khoan
{
    public string ten_dang_nhap { get; set; } = null!;

    public string mat_khau { get; set; } = null!;

    public short quyen { get; set; }

    public string? ma_sv { get; set; }

    public virtual sinh_vien? ma_svNavigation { get; set; }
}
