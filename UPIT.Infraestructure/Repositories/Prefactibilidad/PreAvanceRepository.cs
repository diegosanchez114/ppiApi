using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Models.Prefactibilidad;
using UPIT.Domain.Interfaces.Prefactibilidad;
using UPIT.Domain.DTOs;

namespace UPIT.Infraestructure.Repositories
{
    public class PreAvanceRepository: IPreAvanceRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PreAvanceRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PreAvance model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Pre_Avances (
                  idProyecto,
                  idTipoAvance,
                  numAvance,
                  observaciones,
                  fechaAvance,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idAvance
                VALUES (
                  @IdProyecto,
                  @IdTipoAvance,
                  @numAvance,
                  @Observaciones,
                  @FechaAvance,
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
                        throw new Exception($"PreAvance not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<PreAvance>> GetByFilterPaginatedAsync(string id, int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Pre_Avances 
                            WHERE idProyecto = @Id AND Observaciones LIKE '%' + @Filter + '%'
                            ORDER BY numAvance, fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Pre_Avances 
                            WHERE idProyecto = @Id
                            ORDER BY numAvance, fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Id} {@Page}, {@Limit}, {@Filter}", sql, id, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<PreAvance> list = await connection.QueryAsync<PreAvance>(sql, new { Id = id, Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"PreAvances not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Pre_Avances";
                    int total = connection.ExecuteScalar<int>(queryCount);

                    var obj = new DataPaginatedDTO<PreAvance>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PreAvance model)
        {
            if (model.IdAvance != guid)
                throw new Exception($"PreAvance not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Pre_Avances
                SET 
                  idProyecto = @IdProyecto,
                  idTipoAvance = @IdTipoAvance,
                  observaciones = @Observaciones,
                  fechaAvance = @FechaAvance,                  
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
                        throw new Exception($"PreAvance not was updated");
                    return (Guid)model.IdAvance;
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
