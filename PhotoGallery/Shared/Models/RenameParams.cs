using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoGallery.Models
{
    public class RenameParams
    {
        [Required(ErrorMessage = "Id is required")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
}
