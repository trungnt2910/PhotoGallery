namespace PhotoGallery.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string UsersCollectionName { get; set; }

        public string ImagesCollectionName { get; set; }

        public string FavoritesCollectionName { get; set; }
    }
}
