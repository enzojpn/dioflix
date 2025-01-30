using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace dioflix
{
    public static class Function1
    {
        [FunctionName("UploadFile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // ... (código original para upload)

            return new OkObjectResult("Arquivo enviado com sucesso!");
        }

        [FunctionName("ListFiles")]
        public static async Task<IActionResult> ListFiles(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "files/{containerName}")] HttpRequest req,
            string containerName,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            BlobContainerClient containerClient = new BlobClient(connectionString, containerName);

            var blobs = containerClient.GetBlobsAsync();
            List<string> blobNames = new List<string>();
            await foreach (var blob in blobs)
            {
                blobNames.Add(blob.Name);
            }

            return new OkObjectResult(blobNames);
        }

        [FunctionName("DeleteFile")]
        public static async Task<IActionResult> DeleteFile(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "files/{containerName}/{fileName}")] HttpRequest req,
            string containerName,
            string fileName,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            BlobClient blobClient = new BlobClient(connectionString, containerName, fileName);

            await blobClient.DeleteAsync();

            return new OkObjectResult("Arquivo deletado com sucesso!");
        }
    }
}
