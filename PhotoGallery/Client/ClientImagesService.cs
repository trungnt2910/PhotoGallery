using Microsoft.JSInterop;
using PhotoGallery.Models;
using System.Net.Http.Json;

namespace PhotoGallery.Client
{
    public class ClientImagesService
    {
        private static DotNetObjectReference<ClientImagesService> _instance;

        private readonly HttpClient _http;
        private Task _listImagesTask;
        private List<Image> _cache;
        private HashSet<string> _favoritesCache;

        public List<Image> UnsafeCache => _cache;

        public ClientImagesService(HttpClient http, IdentityService identityService)
        {
            _http = http;
            _listImagesTask = _http.GetFromJsonAsync<List<Image>>("api/Images/list").ContinueWith(task =>
            {
                _cache = task.Result;
            });

            identityService.IdentityChanged += (s, a) => _favoritesCache = null;

            if (_instance != null)
            {
                throw new InvalidOperationException($"Singleton class {nameof(ClientImagesService)} shouldn't be constructed twice.");
            }
            _instance = DotNetObjectReference.Create(this);
        }

        [JSInvokable("ClientImagesService.GetInstance")]
        public static DotNetObjectReference<ClientImagesService> GetInstance()
        {
            return _instance;
        }

        public async Task<IReadOnlyList<Image>> ListImagesAsync(bool forceReload = false)
        {
            await WaitForCacheAsync();
            if (forceReload)
            {
                _cache = await _http.GetFromJsonAsync<List<Image>>("api/Images/list");
            }
            return _cache;
        }


        [JSInvokable("ClientImagesService.SetFavoriteAsync")]
        public async Task SetFavoriteAsync(string imageId)
        {
            await SetFavoriteAsync(new FavoriteParams() { Id = imageId, IsFavorite = !_favoritesCache.Contains(imageId) });
        }

        public async Task SetFavoriteAsync(FavoriteParams value)
        {
            var response = await _http.PostAsJsonAsync("api/Images/favorite", value);
            response.EnsureSuccessStatusCode();
            if (_favoritesCache != null)
            {
                if (value.IsFavorite)
                {
                    _favoritesCache.Add(value.Id);
                }
                else
                {
                    _favoritesCache.Remove(value.Id);
                }
            }
        }

        public async Task<IReadOnlySet<string>> GetFavoritesAsync()
        {
            if (_favoritesCache != null)
            {
                return _favoritesCache;
            }
            var list = await _http.GetFromJsonAsync<List<string>>("api/Images/favorites");
            _favoritesCache = list.ToHashSet();
            return _favoritesCache;
        }

        public async Task<IReadOnlyList<bool>> DeleteImagesAsync(List<string> ids)
        {
            var response = await _http.PutAsJsonAsync("api/Images/delete", ids);
            response.EnsureSuccessStatusCode();
            var status = await response.Content.ReadFromJsonAsync<List<bool>>();
            var newCache = new List<Image>();
            var deletedIds = new HashSet<string>();
            for (int i = 0; i < ids.Count; ++i)
            {
                if (status[i])
                {
                    deletedIds.Add(ids[i]);
                }
            }
            await WaitForCacheAsync();
            foreach (var img in _cache)
            {
                if (!deletedIds.Contains(img.Id))
                {
                    newCache.Add(img);
                }
            }
            _cache = newCache;
            return status;
        }

        public async Task<bool> DeleteImageAsync(string id)
        {
            var response = await _http.DeleteAsync($"api/Images/delete?id={id}");
            response.EnsureSuccessStatusCode();
            var status = await response.Content.ReadFromJsonAsync<bool>();
            if (status)
            {
                await WaitForCacheAsync();
                var index = _cache.FindIndex(i => i.Id == id);
                if (index != -1)
                {
                    _cache.RemoveAt(index);
                }
            }
            return status;
        }

        public async Task<Image> RenameImageAsync(string id, string newName)
        {
            return await RenameImageAsync(new RenameParams() { Id = id, Name = newName });
        }

        public async Task<Image> RenameImageAsync(RenameParams value)
        {
            var response = await _http.PutAsJsonAsync("api/Images/rename", value);
            response.EnsureSuccessStatusCode();
            var img = await response.Content.ReadFromJsonAsync<Image>();
            await WaitForCacheAsync();
            var index = _cache.FindIndex(i => i.Id == value.Id);
            _cache[index] = img;
            return img;
        }

        // Upload not implemented here, as it requires sending "form data" or whatever.
        //public async Task<Image> UploadImageAsync(Stream data, string fileName, bool dispose = false)
        //{

        //}

        public async Task<IReadOnlyList<Image>> SyncImagesAsync()
        {
            var response = await _http.PostAsync("api/Images/sync", null);
            response.EnsureSuccessStatusCode();
            var images = await response.Content.ReadFromJsonAsync<List<Image>>();
            _cache = images;
            return images;
        }

        private Task WaitForCacheAsync()
        {
            if (_cache == null)
            {
                return _listImagesTask;
            }
            return Task.CompletedTask;
        }
    }
}
