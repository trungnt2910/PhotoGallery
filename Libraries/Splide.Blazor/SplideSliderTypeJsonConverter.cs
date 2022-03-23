using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splide.Blazor
{
    class SplideSliderTypeJsonConverter : JsonConverter<SplideSliderType>
    {
        public SplideSliderTypeJsonConverter()
        {
            
        }

        public override SplideSliderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            switch (stringValue)
            {
                case "slide":
                    return SplideSliderType.Slide;
                case "loop":
                    return SplideSliderType.Loop;
                case "fade":
                    return SplideSliderType.Fade;
                default:
                    throw new ArgumentOutOfRangeException("Unknown splider type.");
            }
        }

        public override void Write(Utf8JsonWriter writer, SplideSliderType value, JsonSerializerOptions options)
        {
            var stringValue = value switch
            {
                SplideSliderType.Slide => "slide",
                SplideSliderType.Loop => "loop",
                SplideSliderType.Fade => "fade",
                _ => "invalid",
            };
            writer.WriteStringValue(stringValue);
        }
    }
}
