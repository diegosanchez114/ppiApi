using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Models.PPI;
using UPIT.Domain.Interfaces.PPI;

namespace UPIT.Infraestructure.Repositories
{
    public class PPIContratoRepository : IPPIContratoRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPIContratoRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PPIContrato model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO PPI_Contratos (
                  idProyecto,
                  idEntidadResponsable,
                  codigo,
                  objeto,
                  fechaInicioContrato,
                  fechaTerminacionContrato,
                  valorContrato,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idContrato
                VALUES (
                  @IdProyecto,
                  @IdEntidadResponsable,
                  @Codigo,
                  @Objeto,
                  @FechaInicioContrato,
                  @FechaTerminacionContrato,
                  @ValorContrato,
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
                        throw new Exception($"PPIContrato not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PPIContrato>> GetAllByIdProjectAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_Contratos WHERE idProyecto = @Id ORDER BY fechaCreacion DESC";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    IEnumerable<PPIContrato> list = await connection.QueryAsync<PPIContrato>(sql, new { Id = guid });

                    if (list == null)
                        throw new Exception($"PPIContrato not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PPIContrato>> GetAllByIdProjectAndIdEntityAsync(Guid guidProject, Guid guidEntity)
        {
            string sql = "SELECT * FROM PPI_Contratos WHERE idProyecto = @IdProject AND idEntidadResponsable = @IdEntity ORDER BY fechaCreacion DESC";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: Project: {@Guid}, Entity: {@Guid}", sql, guidProject, guidEntity);
                    IEnumerable<PPIContrato> list = await connection.QueryAsync<PPIContrato>(sql, new { IdProject = guidProject, IdEntity = guidEntity });

                    if (list == null)
                        throw new Exception($"PPIContrato not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PPIContrato> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_Contratos WHERE idContrato = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    
                    var model = await connection.QueryFirstOrDefaultAsync<PPIContrato>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PPIContrato {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PPIContrato model)
        {
            if (model.IdContrato != guid)
                throw new Exception($"PPIContrato not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE PPI_Contratos
                SET 
                  idEntidadResponsable = @IdEntidadResponsable,
                  codigo = @Codigo,
                  objeto = @Objeto,
                  fechaInicioContrato = @FechaInicioContrato,
                  fechaTerminacionContrato = @FechaTerminacionContrato,
                  valorContrato = @ValorContrato,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idContrato = @IdContrato;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PPIContrato not was updated");
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
