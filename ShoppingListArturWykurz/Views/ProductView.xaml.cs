using ShoppingListArturWykurz.Models;

namespace ShoppingListArturWykurz.Views;

public partial class ProductView : ContentView
{
    private Product? _product;
    private bool _isUpdatingUI = false;

    public ProductView()
    {
        InitializeComponent();
        this.MinimumHeightRequest = 70;
    }

    public void SetProduct(Product product, Action<Product>? onDelete = null)
    {
        _product = product;
        UpdateUI();

        if (onDelete != null)
        {
            _onDeleteCallback = onDelete;
        }

        if (_product != null)
        {
            _product.PropertyChanged += (s, e) =>
            {
                UpdateUI();
            };
        }
    }

    private Action<Product>? _onDeleteCallback;

    private void UpdateUI()
    {
        if (_product == null) return;

        _isUpdatingUI = true;

        NameLabel.Text = _product.Name;
        UnitLabel.Text = $"({_product.Unit})";
        QuantityEntry.Text = _product.Quantity.ToString();

        UpdateVisualState();

        _isUpdatingUI = false;
    }

    private void UpdateVisualState()
    {
        if (_product == null) return;

        if (_product.IsPurchased)
        {
            CheckButton.Text = "☑";
            NameLabel.TextDecorations = TextDecorations.Strikethrough;
            NameLabel.TextColor = Colors.LightGray;
            UnitLabel.TextColor = Colors.LightGray;
            ProductBorder.BackgroundColor = Colors.WhiteSmoke;
        }
        else
        {
            CheckButton.Text = "☐";
            NameLabel.TextDecorations = TextDecorations.None;
            NameLabel.TextColor = Colors.Black;
            UnitLabel.TextColor = Color.FromArgb("#888888");
            ProductBorder.BackgroundColor = Colors.White;
        }
    }

    private void OnCheckClicked(object sender, EventArgs e)
    {
        if (_product != null)
        {
            _product.IsPurchased = !_product.IsPurchased;
            UpdateVisualState();
        }
    }

    private void OnQuantityTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdatingUI || _product == null) return;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _product.Quantity = 0;
            return;
        }

        if (int.TryParse(e.NewTextValue, out int quantity))
        {
            _product.Quantity = quantity;
        }
        else
        {
            _isUpdatingUI = true;
            QuantityEntry.Text = _product.Quantity.ToString();
            _isUpdatingUI = false;
        }
    }

    private void OnDecreaseClicked(object sender, EventArgs e)
    {
        if (_product != null && _product.Quantity > 0)
        {
            _product.Quantity--;
        }
    }

    private void OnIncreaseClicked(object sender, EventArgs e)
    {
        if (_product != null)
        {
            _product.Quantity++;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_product != null)
        {
            _onDeleteCallback?.Invoke(_product);
        }
    }
}