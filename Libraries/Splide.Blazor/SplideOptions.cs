using System.Text.Json.Serialization;

namespace Splide.Blazor
{
    public class SplideOptions
    {
        /// <summary>
        /// The type of the slider.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(SplideSliderTypeJsonConverter))]
        public SplideSliderType Type { get; set; } = SplideSliderType.Slide;

        /// <summary>
        /// Determines whether to rewind the slider or not. This does not work in the loop mode.
        /// </summary>
        [JsonPropertyName("rewind")]
        public bool Rewind { get; set; } = false;

        /// <summary>
        /// The transition speed in milliseconds. If <c>0</c>, the slider immediately jumps to the target slide.
        /// </summary>
        [JsonPropertyName("speed")]
        public int Speed { get; set; } = 400;

        /// <summary>
        /// The transition speed on rewind in milliseconds. The <see cref="Speed"/> value is used as default.
        /// </summary>
        [JsonPropertyName("rewindSpeed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? RewindSpeed { get; set; } = null;

        /// <summary>
        /// Defines the slider max width, accepting the CSS format such as 10em, 80vw.
        /// </summary>
        [JsonPropertyName("width")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Width { get; set; } = null;

        /// <summary>
        /// Defines the slide height, accepting the CSS format except for %.
        /// </summary>
        [JsonPropertyName("height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Height { get; set; } = null;

    }
}
