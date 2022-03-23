namespace PhotoGallery.Models
{
    public class CloudinaryImage
    {
        public Uri Url { get; set; }

        public Uri DownloadUrl { get; set; }

        public Uri ThumbnailUrl { get; set; }

        public string Id { get; set; }

        public long Size { get; set; }

        public int Height { get; set; }
        
        public int Width { get; set; }

        public string Format { get; set; }

        public DateTime DateCreated { get; set; }

        public Func<string, Uri> GenerateDownloadUrl { get; init; }
    }
}
