using System.Text.Json.Serialization;

namespace PhotoGallery.Models
{
    public class Response
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; }
        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}
