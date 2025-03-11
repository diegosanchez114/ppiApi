using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class AvanceRepository : IAvanceRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public AvanceRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Avance model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Avances (
                  idProyecto,
                  porcentajeAvanceTotalPlaneado,
                  fechaInicio,
                  fechaFinal,
                  fechaFinalEfectiva,
                  observaciones,
                  fechaCreacion,
                  fechaActualizacion
                )
                VALUES (
                  @IdProyecto,
                  @PorcentajeAvanceTotalPlaneado,
                  @FechaInicio,
                  @FechaFinal,
                  @FechaFinalEfectiva,
                  @Observaciones,
                  @FechaCreacion,
                  @FechaActualizacion
                )";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.QuerySingleAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"Avance not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<Avance>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Avances 
                            WHERE observaciones LIKE '%' + @Filter + '%' OR fechaInicio LIKE '%' + @Filter + '%' OR fechaFinal LIKE '%' + @Filter + '%'
                            ORDER BY fechaInicio OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Avances 
                            ORDER BY fechaInicio OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Avance> list = await connection.QueryAsync<Avance>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Avance not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Avances";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount);

                    var obj = new DataPaginatedDTO<Avance>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Avance> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Avances WHERE IdAvance = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Avance>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Avance {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Avance model)
        {
            if (model.IdAvance != guid)
                throw new Exception($"Avance not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Avances
                SET 
                  idProyecto = @IdProyecto,
                  porcentajeAvanceTotalPlaneado = @PorcentajeAvanceTotalPlaneado,
                  fechaInicio = @FechaInicio,
                  fechaFinal = @FechaFinal,
                  fechaFinalEfectiva = @FechaFinalEfectiva,
                  observaciones = @Observaciones,
                  fechaCreacion = @FechaCreacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idAvance = @IdAvance";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Avance not was updated");
                    return model.IdAvance;
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
