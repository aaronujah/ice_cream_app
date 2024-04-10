using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Models;
using IceCreamMAUI.Shared.Dtos;

namespace IceCreamMAUI.ViewModels;

[QueryProperty(nameof(Icecream), nameof(Icecream))]
public partial class DetailsViewModel : BaseViewModel
{
    public DetailsViewModel(CartViewModel cartViewModel)
    {
        _cartViewModel = cartViewModel;
    }

    [ObservableProperty]
    private IceCreamDto? _icecream;

    [ObservableProperty]
    private int _quantity;

    [ObservableProperty]
    private IcecreamOption[] _options = [];
    private readonly CartViewModel _cartViewModel;

    partial void OnIcecreamChanged(IceCreamDto? value)
    {
        Options = [];
        if (value is null)
            return;

        Options = value.Options.Select(o => new IcecreamOption
        {
            Flavor = o.Flavor,
            Topping = o.Topping,
            IsSelected = false
        })
         .ToArray();

    }

    [RelayCommand]
    private void IncreaseQuantity() => Quantity++;

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 0)
            Quantity--;
    }

    [RelayCommand]
    private async Task GoBackAsync() => await GoToAsync("..", animate: true);

    private void SelectOption(IcecreamOption newOption)
    {
        var newIsSelected = !newOption.IsSelected;
        // Deselect all options
        Options = [.. Options.Select(o => { o.IsSelected = false; return o; })];
        newOption.IsSelected = newIsSelected;
    }

    [RelayCommand]
    private void AddToCart()
    {
        var selectedOption = Options.FirstOrDefault(o => o.IsSelected) ?? Options[0];
        _cartViewModel.AddItemToCart(Icecream!, Quantity, selectedOption.Flavor, selectedOption.Topping);
    }
}