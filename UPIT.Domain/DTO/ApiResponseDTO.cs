namespace UPIT.Domain.DTO
{
    public class ApiResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponseDTO(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
