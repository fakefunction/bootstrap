using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount;

namespace CalculatorLib.ServiceApp
{
    public class AzureBlobBackUp
    {
        public static bool BackUpText(string connectionString, string containerReference, string name, string content)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var BlobClient = storageAccount.CreateCloudBlobClient();

            BlobClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(1), 5);

            var container = BlobClient.GetContainerReference(containerReference);
            if (container == null) throw new Exception("Null ContainerReference");

            var blockBlob = container.GetBlockBlobReference(name);

            blockBlob.UploadText(content);
            return true;
        }
    }
}