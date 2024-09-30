using Blazored.LocalStorage;
using ClientWA.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ClientWA.Services.Class
{
    public class CookieHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IApiService apiService;

        public CookieHandler(ILocalStorageService localStorage, IApiService apiService)
        {
            _localStorage = localStorage;
            this.apiService = apiService;
        }
        protected override  async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);

            if(request.RequestUri.AbsoluteUri.Contains("Authenticator"))
            {
                return await base.SendAsync(request, cancellationToken);

            }
            else
            {
                var expirestime = await _localStorage.GetItemAsync<DateTime>("ExpiresTime");
                if (expirestime < DateTime.UtcNow)
                {
                    var x = await apiService.refresh();
                    if (x.IsSucceeded)
                    {

                    }

                }
                return await base.SendAsync(request, cancellationToken);
            }
           
        }
    }
}
