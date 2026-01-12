using ShoppingListArturWykurz.Models;
using ShoppingListArturWykurz.Services;

namespace ShoppingListArturWykurz.Views;

public partial class MainPage : ContentPage
{
    private JsonDataService _dataService;
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