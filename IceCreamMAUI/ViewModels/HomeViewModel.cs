using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;

namespace IceCreamMAUI.ViewModels
{

    public partial class HomeViewModel(IIcecreamApi icecreamApi, AuthService authService) : BaseViewModel
    {
        private readonly IIcecreamApi _icecreamApi = icecreamApi;
        private readonly AuthService _authService = authService;
        [ObservableProperty]
        private IceCreamDto[] _icecreams = [];

        [ObservableProperty]
        private string _userName = string.Empty;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            UserName = _authService.User!.Name;

            if (_isInitialized)
                return;

            IsBusy = true;

            try
            {
                // Make Api call to fetch Icecream
                _isInitialized = true;
                Icecreams = await _icecreamApi.GetIcecreamAsync();

            }
            catch (Exception ex)
            {
                _isInitialized = false;
                await ShowErrorAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetailsPageAsync(IceCreamDto icecream)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.Icecream)] = icecream,
            };
            await GoToAsync(nameof(DetailsPage), animate: true, parameter);

        }
    }
}

