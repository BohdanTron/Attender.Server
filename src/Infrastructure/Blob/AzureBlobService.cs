using Attender.Server.Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

using BlobInfo = Attender.Server.Application.Common.Models.BlobInfo;

namespace Attender.Server.Infrastructure.Blob
{
    public class AzureBlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public Task<BlobInfo> UploadAvatar(string contentType, Stream content)
        {
            const string containerName = AzureBlobConstants.Container.Avatars;
            var blobName = Guid.NewGuid().ToString();

            return Upload(containerName, blobName, contentType, content);
        }

        private async Task<BlobInfo> Upload(string containerName, string blobName, string contentType, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });

            return new BlobInfo
            {
                Name = blobClient.Name,
                Location = blobClient.Uri.AbsoluteUri
            };
        }
    }
}
