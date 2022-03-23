using System.Text;
using System.Text.Json.Serialization;

namespace PhotoSwipe.Blazor
{
    public class PhotoSwipeOptions
    {
        [JsonPropertyName("index")]
        public int Index { get; set; } = 0;
    }
}
