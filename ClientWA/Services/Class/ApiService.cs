using Blazored.LocalStorage;
using ClientWA.Services.Interfaces;
using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace ClientWA.Services.Class
{
    public class ApiService : IApiService
    {
        private IHttpClientFactory _httpClientFactory;
        private ILocalStorageService _localStorage;

        public ApiService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }
        public async Task<ServiceReturnModel<string>> Logout()
        {
            var client = _httpClientFactory.CreateClient("Server");
            var response = await client.GetAsync("Authenticator/RemoveToken");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await _localStorage.RemoveItemsAsync(["jwt", "ExpiresTime", "data"]);

                return new ServiceReturnModel<string>() { Comment = "Token Removed", IsSucceeded = true };
            }
            else
            {
                return new ServiceReturnModel<string>() { Comment = await response.Content.ReadAsStringAsync(), IsSucceeded = true };

            }
        }
        public async Task<ServiceReturnModel<string>> refresh()
             {
            var client = _httpClientFactory.CreateClient("Server");
            var  response =  await client.GetAsync("Authenticator/RefreshToken");
            if(response.StatusCode ==System.Net.HttpStatusCode.OK)
            {
                var responseobject = await response.Content.ReadFromJsonAsync<RefreshTokenResponseModel>();
                if (responseobject != null)
                {
                    await _localStorage.SetItemAsStringAsync("jwt", responseobject.Jwt);
                    await _localStorage.SetItemAsync<DateTime>("ExpiresTime", responseobject.ExpiresTime);
                    await _localStorage.SetItemAsync<JWTDataModel>("data", responseobject.Data);
                    return new ServiceReturnModel<string>() { Comment = "refreshed", IsSucceeded = true };

                }else
                {
                    return new ServiceReturnModel<string>() { Comment = "responseobject null", IsSucceeded = false };

                }

            }
            else
            {
                return new ServiceReturnModel<string>() { Comment = "Not Authrized", IsSucceeded = false };

            }


        }
        public async Task<ServiceReturnModel<string>> Auth(AuthDto auth)
        {
            var client = _httpClientFactory.CreateClient("Server");
         

            string jsonData = JsonConvert.SerializeObject(auth);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.PostAsync("Authenticator/Auth", content);
            try
            {
                var responseobject = await response.Content.ReadFromJsonAsync<ServiceReturnModel<RefreshTokenResponseModel>>();
                if (responseobject != null)
                {

                    if (responseobject.IsSucceeded)
                    {
                        await _localStorage.SetItemAsStringAsync("jwt", responseobject.Value.Jwt);
                        await _localStorage.SetItemAsync<DateTime>("ExpiresTime", responseobject.Value.ExpiresTime);
                        await _localStorage.SetItemAsync<JWTDataModel>("data", responseobject.Value.Data);


                        return new ServiceReturnModel<string>() { Comment = "نجح تسجيل الدخول", IsSucceeded = true };
                    }
                    else
                    {
                        return new ServiceReturnModel<string>() { Comment = responseobject.Comment, IsSucceeded = false };

                    }

                }
                else
                {
                    return new ServiceReturnModel<string>() { Comment = "response null", IsSucceeded = false };
                }

            }
            catch
            {
                return new ServiceReturnModel<string>() { Comment = "Json Read Failed", IsSucceeded = false };

            }

        }
    }
}
