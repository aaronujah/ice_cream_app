using CommunityToolkit.Maui;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;
#if ANDROID
using System.Net.Security;
using Xamarin.Android.Net;
#elif IOS
using Security;
#endif

namespace IceCreamMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .ConfigureMauiHandlers(h =>
            {
#if ANDROID || IOS
                h.AddHandler<Shell, TabbarBadgeRenderer>();
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<AuthViewModel>()
                        .AddTransient<SignUpPage>()
                        .AddTransient<SignInPage>();

        builder.Services.AddSingleton<AuthService>();

        builder.Services.AddTransient<OnboardingPage>();

        builder.Services.AddSingleton<HomeViewModel>()
                        .AddSingleton<HomePage>();

        builder.Services.AddTransient<DetailsViewModel>()
                        .AddTransient<DetailsPage>();

        builder.Services.AddSingleton<CartViewModel>();

        ConfigureRefit(builder.Services);

        return builder.Build();
    }

    private static void ConfigureRefit(IServiceCollection services)
    {
        var refitSettings = new RefitSettings
        {
            HttpMessageHandlerFactory = () =>
            {
                //return HttpHandler
#if ANDROID
                return new AndroidMessageHandler
                {
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
                    {
                        return certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;
                    }
                };
#elif IOS
                return new NSUrlSessionHandler
                {
                    TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust) =>
                        url.StartsWith("https://localhost")
                };
#endif
                return null;
            }
        };

        services.AddRefitClient<IAuthApi>(refitSettings)
            .ConfigureHttpClient(SetHttpClient);

        services.AddRefitClient<IIcecreamApi>(refitSettings)
            .ConfigureHttpClient(SetHttpClient);

        static void SetHttpClient(HttpClient httpClient)
        {
            var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                                ? "https://10.0.2.2:7194"
                                : "https://localhost:7194";

            if (DeviceInfo.DeviceType == DeviceType.Physical)
            {
                baseUrl = "";
            }

            httpClient.BaseAddress = new Uri(baseUrl);
        }
    }
}
