using Blazored.LocalStorage;
using ClientWA.Services.Interfaces;
using Domain.DTO;
using Domain.Models;

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

        public  async Task<JWTDataModel> GetAuthenticationAsync()
        {

            var jwt = await localStorage.GetItemAsStringAsync("jwt");
            var expirestime = await localStorage.GetItemAsync<DateTime>("ExpiresTime");
            var data = await localStorage.GetItemAsync<JWTDataModel>("data");

            if (jwt != null&& data!=null&& expirestime> DateTime.UtcNow)
            {
                 return data;
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
                    return null;

                }

            }   

        }
    }
}

