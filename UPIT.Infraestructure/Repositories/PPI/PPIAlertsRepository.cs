using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.DTO.PPI;

namespace UPIT.Infraestructure.Repositories
{
    public class PPIAlertsRepository : IPPIAlertsRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPIAlertsRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<PPIAlerDTO>> getAlertsAvances()
        {   
            string sql = $"SELECT " +
                $"c.idContrato as IdContrato, " +
                $"c.codigo AS CodigoContrato, " +
                $"c.idEntidadResponsable as IdEntidadResponsable, " +
                $"e.nombre AS NombreEntidad, " +
                $"a.idAvance AS IdAvance, " +
                $"a.fecha AS FechaAvance, " +
                $"c.fechaTerminacionContrato AS FechaFinalizacion, " +
                $"a.tieneAlerta AS TieneAlerta, " +
                $"a.alertaResuelta AS AlertaResuelta " +                 
                $"FROM PPI_Contratos c JOIN PPI_Avances a ON c.idContrato = a.idContrato " +
                $"JOIN Entidades e ON e.idEntidad = c.idEntidadResponsable " + 
                $"WHERE a.fecha > c.fechaTerminacionContrato AND a.tieneAlerta = 1 AND a.alertaResuelta = 0" +
                $"ORDER BY c.fechaTerminacionContrato;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}", sql);
                    IEnumerable<PPIAlerDTO> list = await connection.QueryAsync<PPIAlerDTO>(sql);

                    if (list == null)
                        throw new Exception($"PPIAlerts not was found");

                    return list.ToList();
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
