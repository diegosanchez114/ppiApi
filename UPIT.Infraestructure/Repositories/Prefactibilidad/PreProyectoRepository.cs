using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Models.Prefactibilidad;
using UPIT.Domain.Interfaces.Prefactibilidad;

namespace UPIT.Infraestructure.Repositories
{
    public class PreProyectoRepository: IPreProyectoRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PreProyectoRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PreProyecto model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Pre_Proyectos (
                  nombre,
                  observaciones,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idProyecto
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
                        throw new Exception($"PreProyecto not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<PreProyecto>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Pre_Proyectos 
                            WHERE nombre LIKE '%' + @Filter + '%'
                            ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Pre_Proyectos 
                            ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<PreProyecto> list = await connection.QueryAsync<PreProyecto>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"PreProyectos not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Pre_Proyectos";
                    int total = connection.ExecuteScalar<int>(queryCount);

                    var obj = new DataPaginatedDTO<PreProyecto>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PreProyecto> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Pre_Proyectos WHERE IdProyecto = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<PreProyecto>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PreProyecto {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PreProyecto model)
        {
            if (model.IdProyecto != guid)
                throw new Exception($"PreProyecto not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Pre_Proyectos
                SET 
                  nombre = @Nombre,
                  observaciones = @Observaciones,
                  fechaCreacion = @FechaCreacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idProyecto = @IdProyecto;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PreProyecto not was updated");
                    return model.IdProyecto;
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
