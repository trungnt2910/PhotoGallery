using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PhotoGallery.Models;
using RenameParams = CloudinaryDotNet.Actions.RenameParams;

namespace PhotoGallery.Server.Services
{
    public class CloudinaryService
    {
        const int THUMBNAIL_HEIGHT = 128;

        private readonly Cloudinary _cloudinary;
        private readonly string _prefix;

        public string Prefix => _prefix;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(cloudinarySettings.Value.CloudName, cloudinarySettings.Value.ApiKey, cloudinarySettings.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
            _prefix = cloudinarySettings.Value.ResourcePrefix;
        }

        public async Task<List<CloudinaryImage>> ListImagesAsync()
        {
            var resultList = new List<CloudinaryImage>();
            string nextCursor = null;
            do
            {
                var resources = await _cloudinary.ListResourcesByPrefixAsync(_prefix, nextCursor: nextCursor);
                ThrowIfError(resources);
                resultList.AddRange(resources.Resources?.Select(r => GetImage(r)) ?? Array.Empty<CloudinaryImage>());
                nextCursor = resources.NextCursor;
            }
            while (nextCursor != null);
            return resultList;
        }

        public async Task<List<bool>> DeleteImagesAsync(List<string> ids)
        {
            var result = new List<bool>();
            if (ids.Count == 1)
            {
                result.Add(await DeleteImageAsync(ids[0]));
            }
            else if (ids.Count > 1)
            {
                foreach (var chunk in ids.Chunk(100))
                {
                    var current = await _cloudinary.DeleteResourcesAsync(chunk.Select(id => _prefix + id).ToArray());
                    ThrowIfError(current);
                    foreach (var id in chunk)
                    {
                        result.Add(current.Deleted.GetValueOrDefault(id) == "deleted");
                    }
                }
            }
            return result;
        }

        public async Task<bool> DeleteImageAsync(string id)
        {
            var current = await _cloudinary.DestroyAsync(new DeletionParams(_prefix + id));
            ThrowIfError(current);
            return true;
        }

        public async Task<CloudinaryImage> RenameImageAsync(string oldId, string newId)
        {
            var result = await _cloudinary.RenameAsync(new RenameParams(_prefix + oldId, _prefix + newId));
            ThrowIfError(result);
            return GetImage(result);
        }

        public async Task<CloudinaryImage> UploadImageAsync(Stream data, string fileName, bool dispose = false)
        {
            try
            {
                var result = await _cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(fileName, data),
                    PublicId = _prefix + fileName
                });
                ThrowIfError(result);
                return GetImage(result);
            }
            finally
            {
                if (dispose)
                {
                    await data.DisposeAsync();
                }
            }
        }

        // This is bad but well we'll wait until
        // Cloudinary devs know how to use _interfaces_
        private CloudinaryImage GetImage(dynamic result)
        {
            return new CloudinaryImage()
            {
                Url = new Uri(result.SecureUrl.ToString()),
                DownloadUrl = new Uri(_cloudinary.Api.UrlImgUp.Transform(new Transformation().Flags("attachment")).Format(result.Format).Version(result.Version).BuildUrl(result.PublicId)),
                ThumbnailUrl = new Uri(_cloudinary.Api.UrlImgUp.Transform(new Transformation().Named("media_lib_thumb")).Format(result.Format).Version(result.Version).BuildUrl(result.PublicId)),
                Id = result.PublicId.Substring(_prefix.Length),
                Size = result.Bytes,
                Height = result.Height,
                Width = result.Width,
                Format = result.Format,
                DateCreated = (result.CreatedAt is DateTime) ? result.CreatedAt : DateTime.ParseExact(result.CreatedAt, "MM/dd/yyyy HH:mm:ss", null),
                GenerateDownloadUrl = (str) => new Uri(_cloudinary.Api.UrlImgUp.Transform(new Transformation().Flags($"attachment:{Uri.EscapeDataString(str).Replace(".", "%252E")}")).Format(result.Format).Version(result.Version).BuildUrl(result.PublicId))
            };
        }

        private void ThrowIfError(BaseResult result)
        {
            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }
        }
    }
}
