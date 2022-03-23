using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoGallery.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; } = "User";

        public string Salt { get; set; }

        public string HashedPassword { get; set; }
    }
}
