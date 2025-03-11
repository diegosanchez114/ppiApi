using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;

namespace UPIT.Infraestructure.Repositories
{
    public class RoleScopeRepository: IRoleScopeRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public RoleScopeRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(RoleScope model)
        {
            model.RoleScopeCreated = DateTime.Now;
            model.RoleScopeUpdated = DateTime.Now;

            string sql = @"INSERT INTO RoleScopes (
                    RoleId, 
                    ScopeId,             
                    RoleScopeCreated, 
                    RoleScopeUpdated)
                OUTPUT INSERTED.RoleScopeId
                VALUES (           
                    @RoleId, 
                    @ScopeId, 
                    @RoleScopeCreated, 
                    @RoleScopeUpdated)";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.QuerySingleAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"RoleScope not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }            
        }

        public async Task<Guid> DeleteAsync(Guid guid)
        {
            string sql = "DELETE FROM RoleScopes WHERE RoleScopeId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    
                    await connection.ExecuteAsync(sql, new { Id = guid });
                    return guid;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }    

        public async Task<RoleScope> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM RoleScopes WHERE RoleScopeId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    
                    var model = await connection.QueryFirstOrDefaultAsync<RoleScope>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"RoleScope {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RoleScope>> GetScopesByRolAsync(Guid guid)
        {
            /*string sql = "SELECT s.* " +
                "FROM RoleScopes rs " +
                "JOIN Scopes s ON rs.ScopeId = s.ScopeId " +
                "WHERE rs.RoleId = @Id";*/
            string sql = "SELECT * FROM RoleScopes WHERE RoleId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);

                    IEnumerable<RoleScope> list = await connection.QueryAsync<RoleScope>(sql, new { Id = guid });
                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, RoleScope model)
        {
            if (model.RoleScopeId != guid)
                throw new Exception($"RoleScope not was updated");

            model.RoleScopeUpdated = DateTime.Now;

            string sql = @"UPDATE Scopes
                SET RoleId = @RoleId,
                    ScopeId = @ScopeId,
                    RoleScopeUpdated = @RoleScopeUpdated                  
                WHERE RoleScopeId = @RoleScopeId";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"RoleScope not was updated");
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
