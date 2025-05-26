namespace demExamSix.DTOs;

public class MaterialDto
{
    public int MaterialId { get; set; }

    public string Title { get; set; } = null!;

    public string TypeMaterial { get; set; }

    public int CountInStock { get; set; }

    public string Measurement { get; set; }

    public int CountInPackage { get; set; }

    public int MinCount { get; set; }

    public decimal PricePerUnitOfMaterial { get; set; }

    public decimal CostOfTheLot { get; set; }
}