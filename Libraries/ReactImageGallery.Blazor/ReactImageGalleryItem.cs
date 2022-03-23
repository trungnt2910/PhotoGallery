using System.Text.Json.Serialization;

namespace ReactImageGallery.Blazor
{
    public class ReactImageGalleryItem
    {
        /// <summary>
        /// image src url
        /// </summary>
        [JsonPropertyName("original")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Uri OriginalUrl { get; set; }

        /// <summary>
        /// image thumbnail src url
        /// </summary>
        [JsonPropertyName("thumbnail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Uri ThumbnailUrl { get; set; }

        /// <summary>
        /// image for fullscreen(defaults to original)
        /// </summary>
        [JsonPropertyName("fullscreen")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Uri FullscreenUrl { get; set; }

        /// <summary>
        /// image height(html5 attribute)
        /// </summary>
        [JsonPropertyName("originalHeight")]
        public int OriginalHeight { get; set; }

        /// <summary>
        /// image width(html5 attribute)
        /// </summary>
        [JsonPropertyName("originalWidth")]
        public int OriginalWidth { get; set; }

        /// <summary>
        /// image loading.Either "lazy" or "eager" (html5 attribute)
        /// </summary>
        [JsonPropertyName("loading")]
        public string Loading { get; set; } = ReactImageGalleryImageLoading.Eager;

        //thumbnailHeight - image height (html5 attribute)
        //thumbnailWidth - image width (html5 attribute)
        //thumbnailLoading - image loading. Either "lazy" or "eager" (html5 attribute)
        //originalClass - custom image class
        //thumbnailClass - custom thumbnail class
        //renderItem - Function for custom rendering a specific slide(see renderItem below)
        //renderThumbInner - Function for custom thumbnail renderer(see renderThumbInner below)
        //originalAlt - image alt
        //thumbnailAlt - thumbnail image alt

        /// <summary>
        /// image title
        /// </summary>
        [JsonPropertyName("originalTitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OriginalTitle { get; set; }

        //thumbnailTitle - thumbnail image title
        //thumbnailLabel - label for thumbnail
        //description - description for image
        //srcSet - image srcset(html5 attribute)
        //sizes - image sizes(html5 attribute)
        //bulletClass - extra class for the bullet of the item
        //bulletOnClick - callback({ item, itemIndex, currentIndex})
        //A function that will be called upon bullet click.
    }
}
