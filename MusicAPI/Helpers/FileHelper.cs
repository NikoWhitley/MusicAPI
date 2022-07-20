using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MusicAPI.Models;
using System.IO;
using System.Threading.Tasks;

namespace MusicAPI.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file) 
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=nikomusicstorageaccount;AccountKey=W4ue7JVmiVn/3oMx3Q2TPEK5u3J8FlIIQwTUd4cnEdOdWcJJZV2UCyrDMxHc5jdHE6ewY4nWUxew+AStgK+fcA==;EndpointSuffix=core.windows.net";
            string containerName = "songscover";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
        public static async Task<string> UploadFile(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=nikomusicstorageaccount;AccountKey=W4ue7JVmiVn/3oMx3Q2TPEK5u3J8FlIIQwTUd4cnEdOdWcJJZV2UCyrDMxHc5jdHE6ewY4nWUxew+AStgK+fcA==;EndpointSuffix=core.windows.net";
            string containerName = "audiofiles";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
