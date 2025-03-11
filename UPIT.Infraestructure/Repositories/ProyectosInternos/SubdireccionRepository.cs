using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class SubdireccionRepository : ISubdireccionRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public SubdireccionRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Subdireccion> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Subdirecciones WHERE idSubdireccion = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);

                    var model = await connection.QueryFirstOrDefaultAsync<Subdireccion>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Subdireccion {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Subdireccion>> GetAllAsync()
        {
            string sql = "SELECT * FROM Subdirecciones ORDER BY nombreDependencia";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);
                    IEnumerable<Subdireccion> list = await connection.QueryAsync<Subdireccion>(sql);

                    if (list == null)
                        throw new Exception($"Subdireccion not was found");

                    return list.ToList();
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
