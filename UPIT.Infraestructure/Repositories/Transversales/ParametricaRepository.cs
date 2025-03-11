using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.Repositories;
using UPIT.Domain.Models.Models;
using UPIT.Infraestructure.Logging;


namespace UPIT.Infraestructure.Repositories.Repositories
{
    public class ParametricaRepository : IParametricaRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public ParametricaRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Parametrica model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Parametricas (
                  Nombre,
                  Valor,
                  Descripcion,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.id
                VALUES (
                  @Nombre,
                  @Valor,
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
                        throw new Exception($"Parametrica not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }   

        public async Task<Parametrica> GetByIdAsync(Guid id)
        {         
            string sql = "SELECT * FROM Parametricas WHERE Id = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, id);
                    
                    var model = await connection.QueryFirstOrDefaultAsync<Parametrica>(sql, new { Id = id });

                    if (model == null)
                        throw new Exception($"Parametricas {id} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }            
        }   

        public async Task<List<Parametrica>> GetParametricasByName(string Name)
        {
            string sql = "SELECT * FROM Parametricas WHERE Nombre = @IdName";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);
                    IEnumerable<Parametrica> list = await connection.QueryAsync<Parametrica>(sql, new { IdName = Name });

                    if (list == null)
                        throw new Exception($"Parametrica not was found");

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<Parametrica>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string condition = "";

            if (filter != null)
                condition = @"WHERE UPPER(nombre) LIKE UPPER('%' + @Filter + '%') 
                                OR UPPER(valor) LIKE UPPER('%' + @Filter + '%') 
                                OR UPPER(descripcion) LIKE UPPER('%' + @Filter + '%')";
            
            string sql = $"SELECT * FROM Parametricas {condition} ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Parametrica> list = await connection.QueryAsync<Parametrica>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Parametrica not was found");

                    //Get total
                    string queryCount = $"SELECT COUNT(*) FROM Parametricas {condition}";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount, new { Filter = filter });

                    var obj = new DataPaginatedDTO<Parametrica>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Parametrica model)
        {
            if (model.Id != guid)
                throw new Exception($"Parametrica not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Parametricas
                SET 
                  Nombre = @Nombre,
                  Valor = @Valor,
                  Descripcion = @Descripcion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  Id = @Id;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Parametrica not was updated");
                    return (Guid)model.Id;
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
