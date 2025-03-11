using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UPIT.Domain.Interfaces.Transversales;

namespace UPIT.Infraestructure.Repositories.Transversales
{
    public class AzureBlobStorageRepository : IBlobStorageRepository
    {
        private readonly CloudBlobClient _blobClient;
        private readonly string _container;

        public AzureBlobStorageRepository(string connectionString, string container)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Azure Storage connection string cannot be null or empty.", nameof(connectionString));

            // Parse the connection string and create a reference to the storage account.
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            _blobClient = storageAccount.CreateCloudBlobClient();
            _container = container;
        }

        public async Task<bool> UploadBlobAsync(string blobName, Stream stream, string? containerName = null)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name cannot be null or empty.", nameof(blobName));
            if (stream == null || stream.Length == 0)
                throw new ArgumentException("Stream cannot be null or empty.", nameof(stream));

            try
            {
                // Get a reference to a container.
                var container = _blobClient.GetContainerReference(containerName != null ? containerName : _container);

                // Create the container if it does not exist.
                await container.CreateIfNotExistsAsync();

                // Get a reference to a blob.
                var blob = container.GetBlockBlobReference(blobName);

                // Upload the stream to the blob.
                await blob.UploadFromStreamAsync(stream);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
