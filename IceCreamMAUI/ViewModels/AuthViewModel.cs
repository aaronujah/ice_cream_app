using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;

namespace IceCreamMAUI.ViewModels;

public partial class AuthViewModel(IAuthApi authApi, AuthService authService) : BaseViewModel
{
    private readonly IAuthApi _authApi = authApi;
    private readonly AuthService _authService = authService;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _name;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _email;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _password;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _address;

    public bool CanSignin => !string.IsNullOrEmpty(Name) &&
                             !string.IsNullOrEmpty(Email);

    public bool CanSignup => CanSignin &&
                             !string.IsNullOrEmpty(Password) &&
                             !string.IsNullOrEmpty(Address);

    [RelayCommand]
    private async Task SignupAsync()
    {
        IsBusy = true;
        try
        {
            var signupDto = new SignupRequestDto(Name, Email, Password, Address);

            //Make Api call
            var result = await _authApi.SignupAsync(signupDto);

            if (result.IsSuccess)
            {
                _authService.Signin(result.Data);

                // Navigate to home page
                await GoToAsync($"//{nameof(HomePage)}", animate: true);
            }
            else
            {
                // Display error alert 
                await ShowErrorAsync(result.ErrorMessage ?? "Unknown error in signing up");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex.Message);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SigninAsync()
    {
        IsBusy = true;
        try
        {
            var signinDto = new SigninRequestDto(Email, Password);

            //Make Api call
            var result = await _authApi.SigninAsync(signinDto);

            if (result.IsSuccess)
            {
                _authService.Signin(result.Data);

                // Navigate to home page
                await GoToAsync($"//{nameof(HomePage)}", animate: true);
            }
            else
            {
                // Display error alert 
                await ShowErrorAsync(result.ErrorMessage ?? "Unknown error in signing in");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex.Message);
        }
        finally { IsBusy = false; }
    }

}

