using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using demExamSix.Data;
using demExamSix.DTOs;
using demExamSix.Views;
using Microsoft.EntityFrameworkCore;

namespace demExamSix;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly AppDbContext _context = new ();
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        List<MaterialDto> materials = new();
        
        var materialProduct = _context.Materials
            .AsNoTracking()
            .Include(m => m.TypeMaterial)
            .Include(m => m.Measurement)
            .ToList();

        foreach (var material in materialProduct)
        {
            decimal cost = CalculateCostOfTheLot(
                material.CountInStock,
                material.MinCount,
                material.CountInPackage,
                material.PricePerUnitOfMaterial);
            
            materials.Add(new MaterialDto
            {
                MaterialId = material.MaterialId,
                Title = material.Title,
                TypeMaterial = material.TypeMaterial.TitleMaterialType,
                CountInStock = material.CountInStock,
                Measurement = material.Measurement.TitleMeasurement,
                CountInPackage = material.CountInPackage,
                MinCount = material.MinCount,
                PricePerUnitOfMaterial = material.PricePerUnitOfMaterial,
                CostOfTheLot = cost
            });
            MaterialsListBox.ItemsSource = null;
            MaterialsListBox.ItemsSource = materials;
        }
    }

    private decimal CalculateCostOfTheLot(int countInStock, int minCount, int countInPackage, decimal pricePerUnit)
    {
        if (countInStock >= minCount)
        {
            return 0;
        }
        
        int deficit = minCount - countInStock;

        int packagesToBuy = deficit / countInPackage;
        if (deficit % countInPackage != 0)
        {
            packagesToBuy++;
        }
        
        int totalCountToBuy = packagesToBuy * countInPackage;
        
        decimal cost = totalCountToBuy * pricePerUnit;
        cost = Math.Round(cost, 2);
        
        return Math.Max(0, cost);
    }

    private void AddMaterial_OnClick(object sender, RoutedEventArgs e)
    {
        
        AddEditMaterial window = new AddEditMaterial();
        bool? result = window.ShowDialog();
        if (result == true)
        {
            LoadData();
        }
    }
    

    private void MaterialsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MaterialsListBox.SelectedItem is MaterialDto material)
        {
            AddEditMaterial window = new AddEditMaterial(material);
            if (window.ShowDialog() == true)
            {
                LoadData();
            }
        }
    }
}