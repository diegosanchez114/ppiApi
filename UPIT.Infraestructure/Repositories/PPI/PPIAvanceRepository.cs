using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;
using UPIT.Domain.DTO.PPI;

namespace UPIT.Infraestructure.Repositories
{
    public class PPIAvanceRepository : IPPIAvanceRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPIAvanceRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PPIAvance model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            //Get fecha fin contrato
            string sqlContrato = "SELECT fechaTerminacionContrato FROM PPI_Contratos WHERE idContrato = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sqlContrato, model.IdContrato);                   

                    var modelContrato = await connection.QueryFirstOrDefaultAsync<PPIAvance>(sqlContrato, new { Id = model.IdContrato });

                    if (model == null)
                      throw new Exception($"Contrato {modelContrato!.IdContrato} para PPIAvance not was found");

                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            string sql = @"INSERT INTO PPI_Avances (
                  idContrato,
                  fecha,
                  porcentajeProgramado,
                  porcentajeEjecutado,
                  valorEjecutado,
                  observaciones,
                  tieneAlerta,
                  alertaResuelta,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idAvance
                VALUES (
                  @IdContrato,
                  @Fecha,
                  @PorcentajeProgramado,
                  @PorcentajeEjecutado,
                  @ValorEjecutado,
                  @Observaciones,
                  @TieneAlerta,
                  @AlertaResuelta,
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
                        throw new Exception($"PPIAvance not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PPIAvanceDTO>> GetAllByIdContractAsync(Guid guid)
        {
            string sql = $"SELECT a.*, " +
                $"(SELECT COUNT(*) FROM PPI_Novedades n JOIN PPI_Avances av ON n.idAvance = av.idAvance WHERE av.idAvance = a.idAvance) as CantNovedades " +
                $"FROM PPI_Avances a " +
                $"WHERE idContrato = @Id " +
                $"ORDER BY fechaCreacion DESC;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    IEnumerable<PPIAvanceDTO> list = await connection.QueryAsync<PPIAvanceDTO>(sql, new { Id = guid });

                    if (list == null)
                        throw new Exception($"PPIAvance not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PPIAvance> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM PPI_Avances WHERE idAvance = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<PPIAvance>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PPIAvance {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PPIAvance model)
        {
            if (model.IdAvance != guid)
                throw new Exception($"PPIAvance not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE PPI_Avances
                SET 
                  fecha = @Fecha,
                  porcentajeProgramado = @PorcentajeProgramado,
                  porcentajeEjecutado = @PorcentajeEjecutado,
                  valorEjecutado = @ValorEjecutado,
                  observaciones = @Observaciones,
                  tieneAlerta = @TieneAlerta,
                  alertaResuelta = @AlertaResuelta,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idAvance = @IdAvance;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PPIAvance not was updated");
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
