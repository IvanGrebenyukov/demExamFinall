using System;
using System.Collections.Generic;

namespace demExamSix.Models;

public partial class UnitMeasurement
{
    public int MeasurementId { get; set; }

    public string TitleMeasurement { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
