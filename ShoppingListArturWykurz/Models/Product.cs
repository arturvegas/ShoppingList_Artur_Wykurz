using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingListArturWykurz.Models;

public class Product : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _unit = "szt.";
    private int _quantity = 1;
    private bool _isPurchased = false;
    private bool _isOptional = false;

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

    public string Unit
    {
        get => _unit;
        set
        {
            if (_unit != value)
            {
                _unit = value;
                OnPropertyChanged();
            }
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) value = 0;
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPurchased
    {
        get => _isPurchased;
        set
        {
            if (_isPurchased != value)
            {
                _isPurchased = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsOptional
    {
        get => _isOptional;
        set
        {
            if (_isOptional != value)
            {
                _isOptional = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}