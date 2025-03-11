using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Models.Models;

namespace UPIT.Infraestructure.Services
{
    public class Util
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public Util(ILogger logger, string connectionString)
        {
            _connectionString = connectionString;
            _logger = (ILogger)logger;
        }

        public async Task<Parametrica> GetParametricaById(int id)
        {
            _logger.LogInformation($"Executing GetParametricasById {id} from Services");

            string sql = "SELECT * FROM Parametricas WHERE Id = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, id);

                    var model = await connection.QueryFirstOrDefaultAsync<Parametrica>(sql, new { Id = id });

                    if (model == null)
                        throw new Exception($"Parametricas {id} not was found");

                    return model;
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
