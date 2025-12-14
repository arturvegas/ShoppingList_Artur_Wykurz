using ShoppingListArturWykurz.Models;
using ShoppingListArturWykurz.Services;

namespace ShoppingListArturWykurz.Views;

public partial class MainPage : ContentPage
{
    private IDataService _dataService;
    private List<Category> _categories = new();

    public MainPage()
    {
        InitializeComponent();
        _dataService = new JsonDataService();
        LoadCategoriesAsync();
    }

    private async void LoadCategoriesAsync()
    {
        try
        {
            _categories = await _dataService.LoadDataAsync();
            DisplayCategories();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Nie udało się załadować danych: {ex.Message}", "OK");
        }
    }

    private void DisplayCategories()
    {
        CategoriesContainer.Children.Clear();

        foreach (var category in _categories)
        {
            var categoryView = new CategoryView();
            categoryView.SetCategory(
                category,
                onDeleteProduct: (product, category_) => { SaveCategoriesAsync(); },
                onDeleteCategory: (category_) =>
                {
                    _categories.Remove(category_);
                    DisplayCategories();
                    SaveCategoriesAsync();
                });

            CategoriesContainer.Children.Add(categoryView);
        }
    }

    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        string? categoryName = await DisplayPromptAsync("Nowa kategoria", "Nazwa kategorii:");

        if (string.IsNullOrWhiteSpace(categoryName)) return;

        var newCategory = new Category{Name = categoryName.Trim()};

        _categories.Add(newCategory);
        DisplayCategories();
        await SaveCategoriesAsync();
    }

    private async void OnExportClicked(object sender, EventArgs e)
    {
        try
        {
            string? fileName = await DisplayPromptAsync("Eksport", "Nazwa pliku (bez rozszerzenia):", "Export");
            if (string.IsNullOrWhiteSpace(fileName)) return;

            string json = await _dataService.ExportDataAsync(_categories);
            string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{fileName.Trim()}.json");
            await File.WriteAllTextAsync(filePath, json);

            await DisplayAlert("Sukces", $"Plik został zapisany:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd eksportu: {ex.Message}", "OK");
        }
    }

    private async void OnImportClicked(object sender, EventArgs e)
    {
        try
        {
            string? filePath = await DisplayPromptAsync("Import", 
                $"Pełna ścieżka z nazwą pliku do importu: \n⚠️UWAGA: po zaimportowaniu pliku, aktualna lista zostanie usunięta!⚠️\n" +
                $"⚠️Wyeksportuj aktualną listę, aby jej nie stracić!⚠️", "Import");

            if (!File.Exists(filePath))
            {
                await DisplayAlert("Błąd", "Plik nie istnieje!", "OK");
                return;
            }

            string json = await File.ReadAllTextAsync(filePath);
            _categories = await _dataService.ImportDataAsync(json);

            if (_categories.Count > 0)
            {
                DisplayCategories();
                await SaveCategoriesAsync();
                await DisplayAlert("Sukces", "Lista została zaimportowana!", "OK");
            }
            else
            {
                await DisplayAlert("Błąd", "Plik nie zawiera poprawnych danych!", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd importu: {ex.Message}", "OK");
        }
    }

    private async Task SaveCategoriesAsync()
    {
       await _dataService.SaveDataAsync(_categories);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await SaveCategoriesAsync();
    }
}