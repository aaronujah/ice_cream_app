using IceCreamMAUI.Shared.Dtos;
using Refit;

namespace IceCreamMAUI.Services;

public interface IAuthApi
{
    [Post("/api/signup")]
    Task<ResultWithDataDto<AuthResponseDto>> SignupAsync(SignupRequestDto dto);

    [Post("/api/signin")]
    Task<ResultWithDataDto<AuthResponseDto>> SigninAsync(SigninRequestDto dto);
}
