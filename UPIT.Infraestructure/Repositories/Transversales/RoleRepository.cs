using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;

namespace UPIT.Infraestructure.Repositories
{
    public class RoleRepository: IRoleRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public RoleRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Role model)
        {
            model.RoleCreated = DateTime.Now;
            model.RoleUpdated = DateTime.Now;

            string sql = @"INSERT INTO Roles (
                    RoleName, 
                    RoleDescription,             
                    RoleCreated, 
                    RoleUpdated)
                OUTPUT INSERTED.RoleId
                VALUES (           
                    @RoleName, 
                    @RoleDescription, 
                    @RoleCreated, 
                    @RoleUpdated)";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.QuerySingleAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"Roles not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Role>> GetAllAsync()
        {
            string sql = "SELECT * FROM Roles ORDER BY RoleName";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);                    

                    IEnumerable<Role> list = await connection.QueryAsync<Role>(sql);

                    if (list == null)
                        throw new Exception($"Roles not was found");                    

                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Role> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Roles WHERE RoleId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Role {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Role model)
        {
            if (model.RoleId != guid)
                throw new Exception($"Role not was updated");

            model.RoleUpdated = DateTime.Now;

            string sql = @"UPDATE Roles
                SET RoleName = @RoleName,
                    RoleDescription = @RoleDescription,
                    RoleUpdated = @RoleUpdated                  
                WHERE RoleId = @RoleId";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Role not was updated");
                    return model.RoleId;
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
