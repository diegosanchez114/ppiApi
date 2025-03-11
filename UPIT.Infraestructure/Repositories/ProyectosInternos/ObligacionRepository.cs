using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class ObligacionRepository : IObligacionRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public ObligacionRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Obligacion model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Obligaciones (
              idProyecto,
              idContratista,
              idInterventoria,
              tipoObligacion,
              tiempo,
              porcentajeAvancePlaneado,
              porcentajeAvanceEjecutado,
              observaciones,
              fechaCreacion,
              fechaActualizacion
            )
            VALUES (
              @IdProyecto,
              @IdContratista,
              @IdInterventoria,
              @TipoObligacion,
              @Tiempo,
              @PorcentajeAvancePlaneado,
              @PorcentajeAvanceEjecutado,
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
                        throw new Exception($"Obligacion not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<Obligacion>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Obligaciones 
                            WHERE 
                                tipoObligacion LIKE '%' + @Filter + '%' OR
                                observaciones  LIKE '%' + @Filter + '%' OR 
                            ORDER BY fechaCreacion  OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Obligaciones 
                            ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Obligacion> list = await connection.QueryAsync<Obligacion>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Obligacion not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Obligaciones";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount);

                    var obj = new DataPaginatedDTO<Obligacion>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Obligacion> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Obligaciones WHERE IdObligacion = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Obligacion>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Obligacion {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Obligacion model)
        {
            if (model.IdObligacion != guid)
                throw new Exception($"Obligacion not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Obligaciones
                SET 
                  idProyecto = @IdProyecto,
                  idContratista = @IdContratista,
                  idInterventoria = @IdInterventoria,
                  tipoObligacion = @TipoObligacion,
                  tiempo = @Tiempo,
                  porcentajeAvancePlaneado = @PorcentajeAvancePlaneado,
                  porcentajeAvanceEjecutado = @PorcentajeAvanceEjecutado,
                  observaciones = @Observaciones,
                  fechaCreacion = @FechaCreacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idObligacion = @IdObligacion";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Obligacion not was updated");
                    return model.IdObligacion;
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
