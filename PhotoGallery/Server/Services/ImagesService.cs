using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PhotoGallery.Models;

namespace PhotoGallery.Server.Services
{
    public class ImagesService
    {
        private readonly CloudinaryService _cloudinary;
        private readonly IMongoCollection<Image> _imagesCollection;
        private readonly IMongoCollection<FavoriteInfo> _favoritesCollection;

        public ImagesService(IOptions<DatabaseSettings> databaseSettings, CloudinaryService cloudinary)
        {
            _cloudinary = cloudinary;

            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _imagesCollection = mongoDatabase.GetCollection<Image>(
                databaseSettings.Value.ImagesCollectionName);

            _favoritesCollection = mongoDatabase.GetCollection<FavoriteInfo>(
                databaseSettings.Value.FavoritesCollectionName);
        }

        public async Task<List<Image>> ListImagesAsync()
        {
            return await (await _imagesCollection.FindAsync(_ => true)).ToListAsync();
        }

        public async Task SetFavoriteAsync(string userId, FavoriteParams value)
        {
            if (!value.IsFavorite)
            {
                await _favoritesCollection.DeleteOneAsync(
                    Builders<FavoriteInfo>.Filter
                        .Eq(nameof(FavoriteInfo.UserId), userId) &
                    Builders<FavoriteInfo>.Filter
                        .Eq(nameof(FavoriteInfo.ImageId), value.Id)
                );
            }
            else
            {
                await _favoritesCollection.FindOneAndUpdateAsync(
                    Builders<FavoriteInfo>.Filter
                        .Eq(nameof(FavoriteInfo.UserId), userId) &
                    Builders<FavoriteInfo>.Filter
                        .Eq(nameof(FavoriteInfo.ImageId), value.Id),
                    Builders<FavoriteInfo>.Update
                        .SetOnInsert(nameof(FavoriteInfo.Id), ObjectId.GenerateNewId().ToString())
                        .SetOnInsert(nameof(FavoriteInfo.UserId), userId)
                        .SetOnInsert(nameof(FavoriteInfo.ImageId), value.Id),
                    new FindOneAndUpdateOptions<FavoriteInfo, FavoriteInfo>()
                    {
                        IsUpsert = true
                    }
                );
            }
        }

        public async Task<List<string>> GetFavoritesAsync(string userId)
        {
            return (await (await _favoritesCollection.FindAsync(
                Builders<FavoriteInfo>.Filter
                    .Eq(nameof(FavoriteInfo.UserId), userId)
            )).ToListAsync())
            .Select(info => info.ImageId)
            .ToList();
        }

        public async Task<List<bool>> DeleteImagesAsync(List<string> ids)
        {
            var status = await _cloudinary.DeleteImagesAsync(ids);
            var dict = Enumerable.Zip(ids, status).ToDictionary(x => x.Item1, x => x.Item2);
            var delRes = await _imagesCollection.DeleteManyAsync((img) => dict.GetValueOrDefault(img.Id));
            await _favoritesCollection.DeleteManyAsync((fav) => dict.GetValueOrDefault(fav.ImageId));
            return status;
        }

        public async Task<bool> DeleteImageAsync(string id)
        {
            var status = await _cloudinary.DeleteImageAsync(id);
            if (status)
            {
                await _imagesCollection.DeleteOneAsync(
                    Builders<Image>.Filter
                        .Eq(nameof(Image.Id), id)
                    );
                await _favoritesCollection.DeleteOneAsync(
                    Builders<FavoriteInfo>.Filter
                        .Eq(nameof(FavoriteInfo.ImageId), id)
                    );
            }
            return status;
        }

        public async Task<Image> RenameImageAsync(string id, string newName)
        {
            return await _imagesCollection.FindOneAndUpdateAsync(
                Builders<Image>.Filter
                    .Eq("Id", id),
                Builders<Image>.Update
                    .Set("Name", newName),
                new FindOneAndUpdateOptions<Image>()
                {
                    ReturnDocument = ReturnDocument.After
                }
            );
        }

        public async Task<Image> UploadImageAsync(Stream data, string fileName, bool dispose = false)
        {
            var id = ObjectId.GenerateNewId().ToString();
            var cloudImg = await _cloudinary.UploadImageAsync(data, id, dispose);
            var img = new Image()
            {
                Id = id,
                Name = fileName,
                Size = cloudImg.Size,
                Height = cloudImg.Height,
                Width = cloudImg.Width,
                Format = cloudImg.Format,
                DateCreated = cloudImg.DateCreated,
                FavoritesCount = 0,
                Url = cloudImg.Url,
                DownloadUrl = cloudImg.GenerateDownloadUrl(fileName),
                ThumbnailUrl = cloudImg.ThumbnailUrl
            };
            await _imagesCollection.InsertOneAsync(img);
            return img;
        }

        public async Task<List<Image>> SyncImagesAsync()
        {
            var cloudImages = await _cloudinary.ListImagesAsync();
            var existingImages = (await (await _imagesCollection.FindAsync(_ => true)).ToListAsync()).ToDictionary(i => i.Id, i => i);

            var images = new List<Image>();
            foreach (var cloudImg in cloudImages)
            {
                if (existingImages.ContainsKey(cloudImg.Id))
                {
                    var img = existingImages[cloudImg.Id];
                    img.DateCreated = cloudImg.DateCreated;
                    img.Format = cloudImg.Format;
                    img.Height = cloudImg.Height;
                    img.Width = cloudImg.Width;
                    img.Size = cloudImg.Size;
                    img.Url = cloudImg.Url;
                    img.DownloadUrl = cloudImg.GenerateDownloadUrl(img.Name);
                    img.ThumbnailUrl = cloudImg.ThumbnailUrl;
                    images.Add(img);
                }
                else
                {
                    var oldId = cloudImg.Id;
                    var newId = ObjectId.GenerateNewId().ToString();
                    var newCloudImg = await _cloudinary.RenameImageAsync(oldId, newId);
                    var img = new Image()
                    {
                        Id = newId,
                        Name = oldId,
                        Size = newCloudImg.Size,
                        Height = newCloudImg.Height,
                        Width = newCloudImg.Width,
                        Format = newCloudImg.Format,
                        DateCreated = newCloudImg.DateCreated,
                        FavoritesCount = 0,
                        Url = newCloudImg.Url,
                        DownloadUrl = cloudImg.GenerateDownloadUrl(oldId),
                        ThumbnailUrl = newCloudImg.ThumbnailUrl
                    };
                    images.Add(img);
                }
            }

            await _imagesCollection.DeleteManyAsync(Builders<Image>.Filter.Empty);
            await _imagesCollection.InsertManyAsync(images);

            var imagesId = images.Select(i => i.Id).ToHashSet();
            await _favoritesCollection.DeleteManyAsync((fav) => !imagesId.Contains(fav.ImageId));

            return images;
        }
    }
}
