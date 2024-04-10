using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;

namespace IceCreamMAUI;

public partial class AppShell : Shell
{
    public AppShell(AuthService authService)
    {
        InitializeComponent();

        RegisterRoutes();
        _authService = authService;
    }

    private readonly static Type[] _routablePageTypes =
        [
            typeof(SignInPage),
            typeof(SignUpPage),
            typeof(OrderDetailsPage),
            typeof(MyOrdersPage),
            typeof(DetailsPage),
        ];
    private readonly AuthService _authService;

    private static void RegisterRoutes()
    {
        foreach (var pageType in _routablePageTypes)
        {
            Routing.RegisterRoute(pageType.Name, pageType);
        }
    }

    private async void FlyoutFooter_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://www.github.com/aaronujah");
    }

    private async void SignoutMenuItem_Clicked(object sender, EventArgs e)
    {
        // await Shell.Current.DisplayAlert("Alert", "Signout menu item clicked", "Ok");
        _authService.Signout();
        await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}");
    }
}
