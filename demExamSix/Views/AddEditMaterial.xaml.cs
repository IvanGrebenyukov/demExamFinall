using System.Windows;
using demExamSix.Data;
using demExamSix.DTOs;
using demExamSix.Models;
using Microsoft.IdentityModel.Tokens;

namespace demExamSix.Views;

public partial class AddEditMaterial : Window
{
    private readonly AppDbContext _context = new();
    private MaterialDto _materialDto;
    
    public AddEditMaterial()
    {
        InitializeComponent();
        LoadTypeMeasurement();
    }

    public AddEditMaterial(MaterialDto materialDto)
    {
        InitializeComponent();
        LoadTypeMeasurement();
        _materialDto = materialDto;
        FillFields(materialDto);
        AddEditButton.Content = "Сохранить";
        
    }

    private void LoadTypeMeasurement()
    {
        cbType.ItemsSource = _context.MaterialTypes.ToList();
        cbMeasurement.ItemsSource = _context.UnitMeasurements.ToList();
    }

    private void FillFields(MaterialDto materialDto)
    {
        tbTitle.Text = materialDto.Title;
        tbCountInPackage.Text = materialDto.CountInPackage.ToString();
        tbCountInStock.Text = materialDto.CountInStock.ToString();
        tbMinCount.Text  = materialDto.MinCount.ToString();
        tbPrice.Text = materialDto.PricePerUnitOfMaterial.ToString();
        var allTypes = cbType.ItemsSource as List<MaterialType>;
        cbType.SelectedItem = allTypes
            .Where(t => t.TitleMaterialType == materialDto.TypeMaterial)
            .FirstOrDefault();
        
        var allMeasurenment = cbMeasurement.ItemsSource as List<UnitMeasurement>;
        cbMeasurement.SelectedItem = allMeasurenment
            .Where(m => m.TitleMeasurement == materialDto.Measurement)
            .FirstOrDefault();
        
    }
    
    private void AddEditButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (cbType.SelectedItem is not MaterialType selectedType)
        {
            MessageBox.Show("Пожалуйста, выберите тип материала.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (cbMeasurement.SelectedItem is not UnitMeasurement selectedMeasurement)
        {
            MessageBox.Show("Пожалуйста, выберите единицу измерения.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!ValidateString(tbTitle.Text, "Наименование") ||
            !ValidateNumber(tbCountInStock.Text, "Количество на складе") ||
            !ValidateNumber(tbCountInPackage.Text, "Количество в упаковке") ||
            !ValidateNumber(tbMinCount.Text, "Минимальное количество") ||
            !ValidateNumber(tbPrice.Text, "Цена единицы материала"))
        {
            return;
        }

        try
        {
            var newMaterial = new Material
            {
                Title = tbTitle.Text.Trim(),
                TypeMaterialId = selectedType.TypeMaterialId,
                CountInStock = int.Parse(tbCountInStock.Text),
                MeasurementId = selectedMeasurement.MeasurementId,
                CountInPackage = int.Parse(tbCountInPackage.Text),
                MinCount = int.Parse(tbMinCount.Text),
                PricePerUnitOfMaterial = decimal.Parse(tbPrice.Text)
            };

            if (_materialDto != null)
            {
                var material = _context.Materials
                    .FirstOrDefault(m => m.MaterialId == _materialDto.MaterialId);

                if (material != null)
                {
                    material.MaterialId = _materialDto.MaterialId;
                    material.Title = tbTitle.Text.Trim();
                    material.TypeMaterialId = selectedType.TypeMaterialId;
                    material.CountInStock = int.Parse(tbCountInStock.Text);
                    material.CountInPackage = int.Parse(tbCountInPackage.Text);
                    material.MeasurementId = selectedMeasurement.MeasurementId;
                    material.MinCount = int.Parse(tbMinCount.Text);
                    material.PricePerUnitOfMaterial = decimal.Parse(tbPrice.Text);
                    _context.Materials.Update(material);
                    _context.SaveChanges();
                    
                    MessageBox.Show("Материал обновлен.", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }
            else
            {
                newMaterial.MaterialId = _context.Materials.Count() + 2;
                _context.Materials.Add(newMaterial);
                _context.SaveChanges();
                MessageBox.Show("Материал добавлен.", "Успешно",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool ValidateNumber(string number, string fieldName)
    {
        if (!decimal.TryParse(number, out decimal result))
        {
            MessageBox.Show($"Введите число  в поле {fieldName}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (result < 0)
        {
            MessageBox.Show($"Число в поле {fieldName} не может быть меньше 0", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        return true;
    }

    private bool ValidateString(string text, string fieldName)
    {
        if (text.IsNullOrEmpty())
        {
            MessageBox.Show($"Поле {fieldName} пустое", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }
}