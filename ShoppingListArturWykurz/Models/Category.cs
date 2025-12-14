using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingListArturWykurz.Models;

public class Category : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private bool _isExpanded = false;
    private ObservableCollection<Product> _products = new();

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Product> Products
    {
        get => _products;
        set
        {
            if (_products != value)
            {
                _products = value;
                OnPropertyChanged();
            }
        }
    }

    public Category()
    {
        Name = string.Empty;
        Products = new ObservableCollection<Product>();
        IsExpanded = false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}