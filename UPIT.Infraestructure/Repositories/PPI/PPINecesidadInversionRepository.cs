using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.Infraestructure.Repositories
{
    public class PPINecesidadInversionRepository : IPPINecesidadInversionRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPINecesidadInversionRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PPINecesidadInversion model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO PPI_NecesidadInversion 
                (idContrato, valorInversion, tipoObra, descripcion, fechaCreacion) 
                OUTPUT INSERTED.idNecesidad
                VALUES 
                (@IdContrato, @ValorInversion, @TipoObra, @Descripcion, @FechaCreacion);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.ExecuteScalarAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"PPINecesidadInversion not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PPINecesidadInversion>> GetAllByIdContractAsync(Guid guid)
        {
            string sql = $"SELECT * FROM PPI_NecesidadInversion WHERE idContrato = @Id " +
                $"ORDER BY fechaCreacion DESC;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    IEnumerable<PPINecesidadInversion> list = await connection.QueryAsync<PPINecesidadInversion>(sql, new { Id = guid });

                    if (list == null)
                        throw new Exception($"PPINecesidadInversion not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PPINecesidadInversion> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_NecesidadInversion WHERE idNecesidad = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    
                    var model = await connection.QueryFirstOrDefaultAsync<PPINecesidadInversion>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PPINecesidadInversion {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PPINecesidadInversion model)
        {
            if (model.IdNecesidad != guid)
                throw new Exception($"PPINecesidadInversion not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE PPI_NecesidadInversion 
                SET 
                    valorInversion = @ValorInversion,
                    tipoObra = @TipoObra,
                    descripcion = @Descripcion,
                    fechaActualizacion = @FechaActualizacion
                WHERE idNecesidad = @IdNecesidad;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PPINecesidadInversion not was updated");
                    return (Guid)model.IdContrato!;
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
