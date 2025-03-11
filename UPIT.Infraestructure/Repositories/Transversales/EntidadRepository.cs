using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.Transversales;
using UPIT.Domain.Models.Transversales;

namespace UPIT.Infraestructure.Repositories.Transversales
{
    public class EntidadRepository : IEntidadRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public EntidadRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Entidad model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Entidades (
                  nombre,
                  descripcion,
                  direccion,
                  telefono,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idEntidad
                VALUES (
                  @Nombre,
                  @Descripcion,
                  @Direccion,
                  @Telefono,
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
                        throw new Exception($"Entidad not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Entidad>> GetAllAsync()
        {
            string sql = "SELECT * FROM Entidades ORDER BY nombre";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);
                    IEnumerable<Entidad> list = await connection.QueryAsync<Entidad>(sql);

                    if (list == null)
                        throw new Exception($"Entidades not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Entidad> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Entidades WHERE IdTipoAvance = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Entidad>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Entidad {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Entidad model)
        {
            if (model.IdEntidad != guid)
                throw new Exception($"Entidad not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Entidades
                SET 
                  nombre = @Nombre,
                  descripcion = @Descripcion,
                  direccion = @Direccion,
                  telefono = @Telefono,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idEntidad = @IdEntidad;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Entidad not was updated");
                    return (Guid)model.IdEntidad;
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
