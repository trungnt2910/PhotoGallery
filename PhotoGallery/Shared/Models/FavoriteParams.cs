using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoGallery.Models
{
    public class FavoriteParams
    {
        [Required(ErrorMessage = "Id is required")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("IsFavorite")]
        public bool IsFavorite { get; set; } = true;
    }
}
