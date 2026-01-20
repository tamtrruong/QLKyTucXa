using System;
using System.Collections.Generic;

namespace QLKTX_DAO.Model.Entities;

public partial class toa_nha
{
    public string ma_toa { get; set; } = null!;

    public string ten_toa { get; set; } = null!;

    public short loai_toa { get; set; }

    public virtual ICollection<phong> phongs { get; set; } = new List<phong>();
}
