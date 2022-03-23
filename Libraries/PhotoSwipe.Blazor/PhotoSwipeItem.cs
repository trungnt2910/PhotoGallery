using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoSwipe.Blazor
{
    public class PhotoSwipeItem
    {
        [JsonPropertyName("src")]
        public Uri Source { get; set; }

        [JsonPropertyName("msrc")]
        public Uri ThumbnailSource { get; set; }

        [JsonPropertyName("h")]
        public int Height { get; set; }

        [JsonPropertyName("w")]
        public int Width { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("pid")]
        public string PictureIndex { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Properties { get; set; }
    }
}
