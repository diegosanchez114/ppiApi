using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace UPIT.Infraestructure.Logging
{

    /*
        Id: Identificador único del log.
        Timestamp: Marca de tiempo del evento.
        LogLevel: Nivel del log (por ejemplo, Info, Warning, Error).
        Message: Mensaje descriptivo del evento o error.
        Exception: Información de la excepción si hubo un error.
        UserId: Identificador del usuario que causó el evento (si es aplicable).
        Username: Nombre de usuario que causó el evento (si es aplicable).
        EventId: Identificador del evento específico.
        Source: Fuente del evento (por ejemplo, el nombre de la clase o método).
        StackTraceInfo: Traza de la pila en caso de una excepción.
        CorrelationId: Identificador para correlacionar múltiples eventos relacionados.
        IpAddress: Dirección IP del usuario que causó el evento.
        RequestUrl: URL de la solicitud que causó el evento.
        HttpMethod: Método HTTP de la solicitud.
        UserAgent: Información del agente de usuario del cliente que hizo la solicitud.
        AdditionalData: Cualquier dato adicional que pueda ser relevante.
     */

    public class LogEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Timestamp { get; set; }
        public string LogLevel { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ExceptionInfo { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int? EventId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string StackTraceInfo { get; set; } = string.Empty;
        public string? CorrelationId { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string RequestUrl { get; set; } = string.Empty;
        public string HttpMethodRequest { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string AdditionalData { get; set; } = string.Empty;
    }


    public interface ILogsRepository
    {
        Task<Guid> CreateAsync(LogEntry model);
    }

    public class LogsRepository : ILogsRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public LogsRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(LogEntry model)
        {
            string sql = @"INSERT INTO Logs (
                  id, 
                  logLevel, 
                  message, 
                  exceptionInfo, 
                  userId, 
                  username, 
                  eventId, 
                  source, 
                  stackTraceInfo, 
                  correlationId, 
                  ipAddress, 
                  requestUrl, 
                  httpMethodRequest, 
                  userAgent, 
                  additionalData
                )
                OUTPUT INSERTED.id
                VALUES (
                  @Id, 
                  @LogLevel, 
                  @Message, 
                  @ExceptionInfo, 
                  @UserId, 
                  @Username, 
                  @EventId, 
                  @Source, 
                  @StackTraceInfo, 
                  @CorrelationId, 
                  @IpAddress, 
                  @RequestUrl, 
                  @HttpMethodRequest, 
                  @UserAgent, 
                  @AdditionalData
                )";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.ExecuteScalarAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"Log not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
