using ShoppingListArturWykurz.Models;

namespace ShoppingListArturWykurz.Views;

public partial class CategoryView : ContentView
{
    private Category? _category;
    private Action<Product, Category>? _onDeleteProduct;
    private Action<Category>? _onDeleteCategory;

    public CategoryView()
    {
        InitializeComponent();
    }

    public void SetCategory(Category category, Action<Product, Category>? onDeleteProduct = null, Action<Category>? onDeleteCategory = null)
    {
        _category = category;
        _onDeleteProduct = onDeleteProduct;
        _onDeleteCategory = onDeleteCategory;
        UpdateUI();

        if (_category != null)
        {
            _category.Products.CollectionChanged += (s, e) =>
            {
                UpdateProductsList();
            };

            _category.PropertyChanged += (s, e) =>
            {
                UpdateUI();
            };
        }
    }

    private void UpdateUI()
    {
        if (_category == null) return;

        CategoryNameLabel.Text = _category.Name;
        ExpandButton.Text = _category.IsExpanded ? "▼" : "▶";
        ProductsContainer.IsVisible = _category.IsExpanded;

        UpdateProductsList();
    }

    private void UpdateProductsList()
    {
        if (_category == null) return;

        ProductsContainer.Children.Clear();

        var sortedProducts = _category.Products.ToList();

        foreach (var product in sortedProducts)
        {
            var productView = new ProductView();
            productView.SetProduct(product, (deletedProduct) =>
             {
                 _category.Products.Remove(deletedProduct);
                 UpdateProductsList();
                 _onDeleteProduct?.Invoke(deletedProduct, _category);
             });

            productView.HorizontalOptions = LayoutOptions.FillAndExpand;

            ProductsContainer.Children.Add(productView);
        }
    }

    private void OnExpandClicked(object sender, EventArgs e)
    {
        if (_category != null)
        {
            _category.IsExpanded = !_category.IsExpanded;
        }
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        if (_category == null) return;

        string? productName = await Application.Current!.MainPage!.DisplayPromptAsync("Nowy produkt", "Nazwa produktu:");
        if (string.IsNullOrWhiteSpace(productName)) return;

        string? unit = await Application.Current!.MainPage!.DisplayPromptAsync("Jednostka", "Jednostka miary (np. szt., l, kg):", initialValue: "szt.");
        if (string.IsNullOrWhiteSpace(unit)) unit = "szt.";

        string? quantityString = await Application.Current!.MainPage!.DisplayPromptAsync("Ilość", "Podaj ilość:", initialValue: "1", keyboard: Keyboard.Numeric);
        int quantity = 1;
        if (!string.IsNullOrWhiteSpace(quantityString) && int.TryParse(quantityString, out int parsedQuantity) && parsedQuantity > 0)
        {
            quantity = parsedQuantity;
        }

        var newProduct = new Product
        {
            Name = productName.Trim(),
            Unit = unit.Trim(),
            Quantity = quantity
        };

        _category.Products.Add(newProduct);
        UpdateProductsList();
    }

    private async void OnDeleteCategoryClicked(object sender, EventArgs e)
    {
        if (_category == null) return;

        bool confirm = await Application.Current!.MainPage!.DisplayAlert("Usunąć kategorię?", 
            $"Czy na pewno chcesz usunąć kategorię '{_category.Name}'?\nTe akcje nie można cofnąć!", "Tak, usuń", "Anuluj");

        if (confirm)
        {
            _onDeleteCategory?.Invoke(_category);
        }
    }
}