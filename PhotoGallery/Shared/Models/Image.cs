using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace PhotoGallery.Models
{
    public class Image
    {
        [JsonPropertyName("Url")]
        public Uri Url { get; set; }

        [JsonPropertyName("DownloadUrl")]
        public Uri DownloadUrl { get; set; }

        [JsonPropertyName("ThumbnailUrl")]
        public Uri ThumbnailUrl { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Size")]
        public long Size { get; set; }
        
        [JsonPropertyName("Height")]
        public long Height { get; set; }

        [JsonPropertyName("Width")]
        public long Width { get; set; }

        [JsonPropertyName("Format")]
        public string Format { get; set; }

        [JsonPropertyName("DateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("FavoritesCount")]
        public int FavoritesCount { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public string NewName { get; set; }
    }
}
