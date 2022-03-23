using Blazored.LocalStorage;
using PhotoGallery.Models;
using System.Net.Http.Headers;

namespace PhotoGallery.Client
{
    public class IdentityService
    {
        const string IdentityKey = "Identity";
        const string UsernameKey = "Username";

        private readonly ISyncLocalStorageService _localStorage;
        private readonly HttpClient _client;

        public IdentityService(ISyncLocalStorageService localStorage, HttpClient client)
        {
            _localStorage = localStorage;
            _client = client;
            var storedIdentity = GetIdentity();
            if (storedIdentity != null)
            {
                ConfigureAuthorizationHeader(storedIdentity.Token);
            }
        }

        public event EventHandler IdentityChanged;
        
        public Identity GetIdentity()
        {
            var identity = _localStorage.GetItem<Identity>(IdentityKey);
            if (identity == null)
            {
                return identity;
            }
            if (identity.Expiration < DateTime.UtcNow)
            {
                _localStorage.RemoveItem(IdentityKey);
                return null;
            }
            return identity;
        }

        public Task<Identity> GetIdentityAsync()
        {
            return Task.FromResult(GetIdentity());
        }

        public void SetIdentity(Identity value)
        {
            _localStorage.SetItem(IdentityKey, value);
            ConfigureAuthorizationHeader(value.Token);
            IdentityChanged?.Invoke(this, EventArgs.Empty);
        }

        public Task SetIdentityAsync(Identity value)
        {
            SetIdentity(value);
            return Task.CompletedTask;
        }

        public string GetUsername()
        {
            return _localStorage.GetItem<string>(UsernameKey);
        }

        public Task<string> GetUsernameAsync()
        {
            return Task.FromResult(GetUsername());
        }

        public void SetUsername(string value)
        {
            _localStorage.SetItem(UsernameKey, value);
        }

        public Task SetUsernameAsync(string value)
        {
            SetUsername(value);
            return Task.CompletedTask;
        }

        private void ConfigureAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
