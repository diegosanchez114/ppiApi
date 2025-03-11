using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using UPIT.Domain.Models;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.Prefactibilidad;
using UPIT.Domain.Models.Prefactibilidad;

namespace UPIT.Infraestructure.Repositories
{
    public class PreTipoAvanceRepository : IPreTipoAvanceRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PreTipoAvanceRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PreTipoAvance model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Pre_TiposAvance (                  
                  nombre,
                  observaciones,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.IdTipoAvance
                VALUES (                  
                  @Nombre,
                  @Observaciones,
                  @FechaCreacion,
                  @FechaActualizacion
                );";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.ExecuteScalarAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"PreTipoAvance not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PreTipoAvance>> GetAllAsync()
        {
            string sql = "SELECT * FROM Pre_TiposAvance ORDER BY nombre";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);
                    IEnumerable<PreTipoAvance> list = await connection.QueryAsync<PreTipoAvance>(sql);

                    if (list == null)
                        throw new Exception($"PreTipoAvance not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PreTipoAvance> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Pre_TiposAvance WHERE IdTipoAvance = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<PreTipoAvance>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PreTipoAvance {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PreTipoAvance model)
        {
            if (model.IdTipoAvance != guid)
                throw new Exception($"PreTiposAvance not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Pre_TiposAvance
                SET 
                  nombre = @Nombre,
                  observaciones = @Observaciones,
                  fechaCreacion = @FechaCreacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idTipoAvance = @IdTipoAvance;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PreTipoAvance not was updated");
                    return model.IdTipoAvance;
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
