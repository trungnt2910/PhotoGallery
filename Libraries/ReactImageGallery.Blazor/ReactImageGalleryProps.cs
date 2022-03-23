using System.Text.Json.Serialization;

namespace ReactImageGallery.Blazor
{
    class ReactImageGalleryProps
    {
        /// <summary>
        /// (required) Array of objects, see example above,
        /// </summary>
        [JsonPropertyName("items")]
        public IList<ReactImageGalleryItem> Items { get; set; }

        /// <summary>
        ///  Boolean, default true
        ///  - infinite sliding
        /// </summary>
        [JsonPropertyName("infinite")]
        public bool Infinite { get; set; } = true;

        /// <summary>
        /// Boolean, default false
        /// </summary>
        [JsonPropertyName("lazyLoad")]
        public bool LazyLoad { get; set; } = false;

        /// <summary>
        /// Boolean, default true
        /// </summary>
        [JsonPropertyName("showNav")]
        public bool ShowNav { get; set; } = true;

        /// <summary>
        /// Boolean, default true
        /// </summary>
        [JsonPropertyName("showThumbnails")]
        public bool ShowThumbnails { get; set; } = true;

        /// <summary>
        /// String, default <see cref="ReactImageGalleryPosition.Bottom"/><br/>
        /// available positions: top, right, bottom, left
        /// </summary>
        [JsonPropertyName("thumbnailPosition")]
        public string ThumbnailPosition { get; set; } = ReactImageGalleryPosition.Bottom;

        /// <summary>
        /// Boolean, default true
        /// </summary>
        [JsonPropertyName("showFullscreenButton")]
        public bool ShowFullscreenButton { get; set; } = true;

        /// <summary>
        /// Boolean, default true<br/>
        /// if false, fullscreen will be done via css within the browser
        /// </summary>
        [JsonPropertyName("useBrowserFullscreen")]
        public bool UseBrowserFullscreen { get; set; } = true;

        //useTranslate3D: Boolean, default true
        //if false, will use translate instead of translate3d css property to transition slides

        //showPlayButton: Boolean, default true

        //isRTL: Boolean, default false
        //if true, gallery's direction will be from right-to-left (to support right-to-left languages)

        /// <summary>
        /// Boolean, default false
        /// </summary>
        [JsonPropertyName("showBullets")]
        public bool ShowBullets { get; set; } = false;

        //showIndex: Boolean, default false

        /// <summary>
        /// Boolean, default false
        /// </summary>
        [JsonPropertyName("autoPlay")]
        public bool AutoPlay { get; set; } = false;

        //disableThumbnailScroll: Boolean, default false
        //disables the thumbnail container from adjusting

        //disableKeyDown: Boolean, default false
        //disables keydown listener for keyboard shortcuts (left arrow, right arrow, esc key)

        //disableSwipe: Boolean, default false

        //disableThumbnailSwipe: Boolean, default false

        //onErrorImageURL: String, default undefined
        //an image src pointing to your default image if an image fails to load
        //handles both slide image, and thumbnail image

        //indexSeparator: String, default ' / ', ignored if showIndex is false

        /// <summary>
        /// Number, default 450<br/>
        /// transition duration during image slide in milliseconds
        /// </summary>
        [JsonPropertyName("slideDuration")]
        public int SlideDuration { get; set; } = 450;

        //swipingTransitionDuration: Number, default 0
        //transition duration while swiping in milliseconds

        /// <summary>
        /// Number, default 3000
        /// </summary>
        [JsonPropertyName("slideInterval")]
        public int SlideInterval { get; set; } = 3000;

        //slideOnThumbnailOver: Boolean, default false

        //flickThreshold: Number (float), default 0.4
        //Determines the max velocity of a swipe before it's considered a flick (lower = more sensitive)

        //swipeThreshold: Number, default 30
        //A percentage of how far the offset of the current slide is swiped to trigger a slide event. e.g. If the current slide is swiped less than 30% to the left or right, it will not trigger a slide event.

        //stopPropagation: Boolean, default false
        //Automatically calls stopPropagation on all 'swipe' events.

        /// <summary>
        /// Number, default 0
        /// </summary>
        [JsonPropertyName("startIndex")]
        public int StartIndex { get; set; } = 0;
    }
}
