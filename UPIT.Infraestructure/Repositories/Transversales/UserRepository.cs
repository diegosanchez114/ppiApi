using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using UPIT.Infraestructure.Logging;
using System.Data.SqlClient;

namespace UPIT.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public UserRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(User model)
        {
            model.UserState = true;
            model.UserCreated = DateTime.Now;
            model.UserUpdated = DateTime.Now;

            string sql = @"INSERT INTO Users (
                    UserEmail,
                    UserIdentificationNumber, 
                    UserDocumentType, 
                    UserFirstName, 
                    UserLastName, 
                    RoleId,
                    EntityId, 
                    UserPhoneNumber, 
                    UserPosition, 
                    UserState, 
                    UserPassword, 
                    UserCreated, 
                    UserUpdated)
                OUTPUT INSERTED.UserId                    
                VALUES (
                    @UserEmail,
                    @UserIdentificationNumber, 
                    @UserDocumentType, 
                    @UserFirstName, 
                    @UserLastName, 
                    @RoleId,
                    @EntityId, 
                    @UserPhoneNumber, 
                    @UserPosition, 
                    @UserState, 
                    @UserPassword, 
                    @UserCreated, 
                    @UserUpdated)";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.QuerySingleAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"User not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }                      
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            string sql = "SELECT * FROM Users WHERE UserEmail = @Email";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Email}", sql, email);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

                    if (model == null)
                        throw new Exception($"User {email} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<User>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Users 
                            WHERE 
                                UserIdentificationNumber LIKE '%' + @Filter + '%' OR
                                UserFirstName LIKE '%' + @Filter + '%' OR 
                                UserLastName LIKE '%' + @Filter + '%' OR
                                UserEmail LIKE '%' + @Filter + '%' 
                            ORDER BY UserFirstName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Users 
                            ORDER BY UserFirstName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<User> list = await connection.QueryAsync<User>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    foreach (User item in list)
                    {
                        item.UserPassword = "";
                    }

                    if (list == null)
                        throw new Exception($"Users not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Users";
                    int total = connection.ExecuteScalar<int>(queryCount);

                    var obj = new DataPaginatedDTO<User>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Users WHERE UserId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"User {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, User model)
        {
            if (model.UserId != guid)
                throw new Exception($"User not was updated");

            model.UserUpdated = DateTime.Now;

            string sql = @"UPDATE Users
                SET UserIdentificationNumber = @UserIdentificationNumber,
                    UserEmail = @UserEmail,
                    UserDocumentType = @UserDocumentType,
                    UserFirstName = @UserFirstName,
                    UserLastName = @UserLastName,
                    RoleId = @RoleId,
                    EntityId = @EntityId,
                    UserPhoneNumber = @UserPhoneNumber,
                    UserPosition = @UserPosition,
                    UserState = @UserState,
                    UserPassword = @UserPassword,
                    UserUpdated = @UserUpdated                    
                WHERE UserId = @UserId";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"User not was updated");
                    return model.UserId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateStateAsync(Guid guid, bool state)
        {            

            var userUpdated = DateTime.Now;

            string sql = @"UPDATE Users
                SET UserState = @UserState, 
                    UserUpdated = @UserUpdated                    
                WHERE UserId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nState: {@State}", sql, state);

                    int rowsAffected = await connection.ExecuteAsync(sql, new { Id = guid, UserState = state, UserUpdated = userUpdated });

                    if (rowsAffected == 0)
                        throw new Exception($"User not was updated");
                    return guid;
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
