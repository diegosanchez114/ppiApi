namespace UPIT.Domain.Interfaces.Transversales
{
    public interface IBlobStorageRepository
    {
        Task<bool> UploadBlobAsync(string blobName, Stream stream, string containerName);
        //Task<Stream> DownloadBlobAsync(string blobName);
        //Task<bool> DeleteBlobAsync(string blobName);
        //Task<IEnumerable<string>> ListBlobsAsync();
    }
}
