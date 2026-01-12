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
        QuantityLabel.Text = _product.Quantity.ToString();
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