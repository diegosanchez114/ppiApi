using System.Text.Json;

namespace UPIT.API.Services
{
    public class ResponseAPI
    {
        public string Success { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string NotFound {  get; set; } = string.Empty;
        public string MissingAuthorizationHeader {  get; set; } = string.Empty;
        public string InvalidFormatPassword {  get; set; } = string.Empty;
    }

    public class ResponseAPIManager
    {
        public readonly ResponseAPI _responseAPI;

        public ResponseAPIManager(string pathResource) {
            string jsonString = File.ReadAllText(pathResource);
            _responseAPI = JsonSerializer.Deserialize<ResponseAPI>(jsonString)!;
        }        

        public string GetSuccessMessage()
        {
            return _responseAPI.Success;
        }

        public string GetErrorMessage()
        {
            return _responseAPI.Error;
        }

        public string GeNotFoundMessage()
        {
            return _responseAPI.NotFound;
        }

        public string GetMissingAuthorizationHeader()
        {
            return _responseAPI.MissingAuthorizationHeader;
        }
        public string GetInvalidFormatPassword()
        {
            return _responseAPI.InvalidFormatPassword;
        }
    }
}
