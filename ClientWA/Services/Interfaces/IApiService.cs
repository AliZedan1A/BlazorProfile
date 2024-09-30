using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;

namespace ClientWA.Services.Interfaces
{
    public interface IApiService
    {
        Task<ServiceReturnModel<string>> refresh();
        Task<ServiceReturnModel<string>> Auth(AuthDto auth);
        Task<ServiceReturnModel<string>> Logout();
    }
}
