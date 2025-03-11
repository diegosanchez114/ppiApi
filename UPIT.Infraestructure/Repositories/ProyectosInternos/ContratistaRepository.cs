using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Domain.DTOs;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class ContratistaRepository : IContratistaRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public ContratistaRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<DataPaginatedDTO<Contratista>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Contratistas 
                            WHERE nombreContratista LIKE '%' + @Filter + '%' OR nombreAccionistaContratista LIKE '%' + @Filter + '%'
                           ";
            else
                sql = @"SELECT * FROM Contratistas 
                            ";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Contratista> list = await connection.QueryAsync<Contratista>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Contratista not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Contratistas";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount);

                    var obj = new DataPaginatedDTO<Contratista>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Contratista> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Contratistas WHERE idContratista  = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Contratista>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Contratista {guid} not was found");

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
