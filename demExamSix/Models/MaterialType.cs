using System;
using System.Collections.Generic;

namespace demExamSix.Models;

public partial class MaterialType
{
    public int TypeMaterialId { get; set; }

    public string TitleMaterialType { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
