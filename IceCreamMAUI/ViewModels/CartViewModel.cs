﻿using IceCreamMAUI.Models;
using IceCreamMAUI.Shared.Dtos;
using System.Collections.ObjectModel;

namespace IceCreamMAUI.ViewModels;

public partial class CartViewModel : BaseViewModel
{
    public ObservableCollection<CartItem> CartItems { get; set; } = [];

    public static int TotalCartCount { get; set; }
    public static event EventHandler<int>? TotalCartCountChanged;
    public async void AddItemToCart(IceCreamDto icecream, int quantity, string flavor, string topping)
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.IcecreamId == icecream.Id);
        if (existingItem is not null)
        {
            if (quantity <= 0)
            {
                // Remove this from cart
                CartItems.Remove(existingItem);
                await ShowToastAsync("Icecream removed from the cart");
            }
            else
            {
                existingItem.Quantity = quantity;
                await ShowToastAsync("Quantity updated in the cart");
            }
        }
        else
        {
            var cartItem = new CartItem
            {
                FlavorName = flavor,
                IcecreamId = icecream.Id,
                Name = icecream.Name,
                Price = icecream.Price,
                Quantity = quantity,
                ToppingName = topping
            };

            CartItems.Add(cartItem);
            await ShowToastAsync("Icecream added to cart");
        }

        TotalCartCount = CartItems.Sum(i => i.Quantity);
        TotalCartCountChanged?.Invoke(null, TotalCartCount);
    }
}
