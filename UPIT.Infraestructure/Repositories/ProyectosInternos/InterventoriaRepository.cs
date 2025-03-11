using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class InterventoriaRepository : IInterventoriaRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public InterventoriaRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<DataPaginatedDTO<Interventoria>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Entities 
                            WHERE EntityName LIKE '%' + @Filter + '%'
                            ORDER BY EntityName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Entities 
                            ORDER BY EntityName OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Interventoria> list = await connection.QueryAsync<Interventoria>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Entities not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Entities";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount);

                    var obj = new DataPaginatedDTO<Interventoria>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Interventoria> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Entities WHERE EntityId = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Interventoria>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Entity {guid} not was found");

                    return model;
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
