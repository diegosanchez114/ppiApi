using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.Infraestructure.Repositories
{
    public class PPINovedadRepository : IPPINovedadRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPINovedadRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PPINovedad model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO PPI_Novedades (
                  idAvance,
                  fecha,                 
                  descripcion,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idNovedad
                VALUES (
                  @IdAvance,
                  @Fecha,
                  @Descripcion,
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
                        throw new Exception($"PPINovedad not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PPINovedad>> GetAllByIdAvanceAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_Novedades WHERE idAvance = @Id ORDER BY fechaCreacion DESC";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    IEnumerable<PPINovedad> list = await connection.QueryAsync<PPINovedad>(sql, new { Id = guid });

                    if (list == null)
                        throw new Exception($"PPINovedad not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PPINovedad> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_Novedades WHERE idNovedad = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<PPINovedad>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PPINovedad {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PPINovedad model)
        {
            if (model.IdAvance != guid)
                throw new Exception($"PPINovedad not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE PPI_Novedades
                SET 
                  fecha = @Fecha,                  
                  descripcion = @Descripcion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idNovedad = @IdNovedad;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PPINovedad not was updated");
                    return (Guid)model.IdNovedad!;
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
