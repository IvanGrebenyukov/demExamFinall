using System;
using System.Collections.Generic;

namespace demExamSix.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string Title { get; set; } = null!;

    public int TypeMaterialId { get; set; }

    public int CountInStock { get; set; }

    public int MeasurementId { get; set; }

    public int CountInPackage { get; set; }

    public int MinCount { get; set; }

    public decimal PricePerUnitOfMaterial { get; set; }

    public virtual UnitMeasurement Measurement { get; set; } = null!;

    public virtual MaterialType TypeMaterial { get; set; } = null!;
}
