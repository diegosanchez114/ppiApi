using UPIT.Domain.DTOs;
using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using UPIT.Infraestructure.Logging;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Data.SqlClient;

namespace UPIT.Infraestructure.Repositories
{
    public class ScopeRepository: IScopeRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public ScopeRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Scope model)
        {
            model.ScopeCreated = DateTime.Now;
            model.ScopeUpdated = DateTime.Now;

            string sql = @"INSERT INTO Scopes (
                    ScopeName, 
                    ScopePath,
                    ScopeMethod,
                    ScopeDescription,             
                    ScopeCreated, 
                    ScopeUpdated)
                OUTPUT INSERTED.ScopeId
                VALUES (           
                    @ScopeName,
                    @ScopePath,
                    @ScopeMethod,
                    @ScopeDescription, 
                    @ScopeCreated, 
                    @ScopeUpdated)";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.QuerySingleAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"Scope not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }            
        }

        public async Task<DataPaginatedDTO<Scope>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Scopes 
                            WHERE 
                                ScopeName LIKE '%' + @Filter + '%'                                
                            ORDER BY ScopeName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Scopes 
                            ORDER BY ScopeName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Scope> list = await connection.QueryAsync<Scope>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Scopes not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Scopes";
                    int total = connection.ExecuteScalar<int>(queryCount);

                    var obj = new DataPaginatedDTO<Scope>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Scope> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Scopes WHERE ScopeId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Scope>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Scope {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Scope model)
        {
            if (model.ScopeId != guid)
                throw new Exception($"Scope not was updated");

            model.ScopeUpdated = DateTime.Now;      

            string sql = @"UPDATE Scopes
                SET ScopeName = @ScopeName,
                    ScopePath = @ScopePath,
                    ScopeMethod = @ScopeMethod,
                    ScopeDescription = @ScopeDescription,
                    ScopeUpdated = @ScopeUpdated                  
                WHERE ScopeId = @ScopeId";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Scope not was updated");
                    return model.ScopeId;
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
