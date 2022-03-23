using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PhotoGallery.Models
{
    public class FavoriteInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ImageId")]
        public string ImageId { get; set; }

        [JsonPropertyName("UserId")]
        public string UserId { get; set; }
    }
}
