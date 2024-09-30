using Blazored.LocalStorage;
using ClientWA.Services.Interfaces;
using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;

namespace ClientWA.Services.Class
{
    public class Auth 
    {
        private readonly ILocalStorageService localStorage;
        private readonly IApiService _apiservice;

        public Auth(ILocalStorageService localStorage,IApiService apiservice)
        {
            this.localStorage = localStorage;
            _apiservice = apiservice;
        }

        public  async Task<ServiceReturnModel<JWTDataModel>> GetAuthenticationAsync()
        {
            var jwt = await localStorage.GetItemAsStringAsync("jwt");
            var expirestime = await localStorage.GetItemAsync<DateTime>("ExpiresTime");
            var data = await localStorage.GetItemAsync<JWTDataModel>("data");
            if (jwt != null&& data!=null&& expirestime> DateTime.UtcNow)
            {
                 return new ServiceReturnModel<JWTDataModel> { Comment = "suc",IsSucceeded = true  ,Value = data};
            }
            else
            {

                var res = await _apiservice.refresh();
                if (res.IsSucceeded)
                {
                  return await GetAuthenticationAsync();
                }
                else
                {
                    return new ServiceReturnModel<JWTDataModel> { Comment = "faild", IsSucceeded = false };

                }

            }   

        }
    }
}

