using Microsoft.JSInterop;

namespace PhotoSwipe.Blazor
{
    public class PhotoSwipeService
    {
        private readonly IJSRuntime _runtime;

        public PhotoSwipeService(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task OpenAsync(IEnumerable<PhotoSwipeItem> images, PhotoSwipeOptions options = null, string uiConstructor = "")
        {
            options ??= new PhotoSwipeOptions();
            if (images?.Any() ?? false)
            {
                var list = images.ToList();
                if (options.Index >= list.Count)
                {
                    throw new IndexOutOfRangeException("Image index out of range.");
                }
                await _runtime.InvokeVoidAsync("PhotoSwipe.PhotoSwipeService.OpenAsync", list, options, uiConstructor);
            }
        }
    }
}
