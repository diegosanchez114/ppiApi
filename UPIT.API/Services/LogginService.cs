using UPIT.Infraestructure.Logging;

namespace UPIT.API.Services
{
    public class LoggingService
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogsRepository _repository;

        public LoggingService(
            ITokenService tokenService, 
            IHttpContextAccessor httpContextAccessor,
            ILogsRepository repository
        )
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public async void CreateLog(string message, string source, string exception = "", string stackTrace = "", string correlationId = "", string additionalData = "")
        {
            var context = _httpContextAccessor.HttpContext;
            var userId = _tokenService.GetClaimValue("oid");
            var email = _tokenService.GetClaimValue("email");

            var logEntry = new LogEntry
            {
                LogLevel = "Info",
                Message = message,
                Source = source,
                ExceptionInfo = exception,
                StackTraceInfo = stackTrace,
                CorrelationId = correlationId,
                UserId = userId,
                Username = email,
                IpAddress = context?.Connection?.RemoteIpAddress?.ToString()!,
                RequestUrl = context?.Request?.Path!,
                HttpMethodRequest = context?.Request?.Method!,
                UserAgent = context?.Request?.Headers["User-Agent"].FirstOrDefault()!,
                AdditionalData = additionalData
            };

            try
            {
                await _repository.CreateAsync(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }
    }
}
