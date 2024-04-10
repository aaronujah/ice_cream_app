using IceCreamMAUI.ViewModels;

namespace IceCreamMAUI.Pages;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(AuthViewModel authViewModel)
    {

        InitializeComponent();
        BindingContext = authViewModel;
    }

    private async void SigninLabel_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignInPage));
    }
}