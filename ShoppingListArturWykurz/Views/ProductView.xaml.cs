using ShoppingListArturWykurz.Models;

namespace ShoppingListArturWykurz.Views;

public partial class ProductView : ContentView
{
    private Product? _product;

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

        NameLabel.Text = _product.Name;
        UnitLabel.Text = $"({_product.Unit})";
        OptionalLabel.Text = _product.IsOptional ? "⚠ Opcjonalnie" : string.Empty;
        QuantityLabel.Text = _product.Quantity.ToString();
        PurchasedCheckBox.IsChecked = _product.IsPurchased;

        if (_product.IsPurchased)
        {
            NameLabel.TextDecorations = TextDecorations.Strikethrough;
            NameLabel.Opacity = 0.5;
            UnitLabel.Opacity = 0.5;
            OptionalLabel.Opacity = 0.5;
        }
        else
        {
            NameLabel.TextDecorations = TextDecorations.None;
            NameLabel.Opacity = 1.0;
            UnitLabel.Opacity = 1.0;
            OptionalLabel.Opacity = 1.0;
        }
    }

    private void OnPurchasedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_product != null)
        {
            _product.IsPurchased = e.Value;
            UpdateUI();
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