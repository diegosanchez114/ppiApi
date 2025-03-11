using Dapper;
using Microsoft.Extensions.Logging;
using UPIT.Infraestructure.Logging;
using UPIT.Domain.DTOs;
using System.Data.SqlClient;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Infraestructure.Repositories.ProyectosInternos
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public ProyectoRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Proyecto model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO Proyectos (
              idSubdireccion,
              nombreProyecto,
              objetoProyecto,
              fechaInicioContrato,
              fechaFinalContratoContractual,
              fechaFinalRealContrato,
              fechaSuscripcionContrato,             
              observaciones,
              valorContrastistaInicial,
              valorInterventoriaInicial,
              fechaCreacion,
              fechaActualizacion
            )
            OUTPUT INSERTED.idProyecto
            VALUES (
              @IdSubdireccion,
              @NombreProyecto,
              @ObjetoProyecto,
              @FechaInicioContrato,
              @FechaFinalContratoContractual,
              @FechaFinalRealContrato,
              @FechaSuscripcionContrato,              
              @Observaciones,
              @ValorContrastistaInicial,
              @ValorInterventoriaInicial,
              @FechaCreacion,
              @FechaActualizacion
            )";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    Guid idInserted = await connection.ExecuteScalarAsync<Guid>(sql, model);

                    if (idInserted == Guid.Empty)
                        throw new Exception($"Proyecto not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<Proyecto>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string sql = "";

            if (filter != null)
                sql = @"SELECT * FROM Proyectos 
                            WHERE nombreProyecto LIKE '%' + @Filter + '%' OR objetoProyecto LIKE '%' + @Filter + '%' 
                            ORDER BY nombreProyecto OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            else
                sql = @"SELECT * FROM Proyectos 
                            ORDER BY nombreProyecto OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;
                    IEnumerable<Proyecto> list = await connection.QueryAsync<Proyecto>(sql, new { Filter = filter, Offset = offset, Limit = limit });

                    if (list == null)
                        throw new Exception($"Proyecto not was found");

                    //Get total
                    string queryCount = "SELECT COUNT(*) FROM Proyectos";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount);

                    var obj = new DataPaginatedDTO<Proyecto>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Proyecto> GetByIdAsync(Guid guid)
        {
            string sql = "SELECT * FROM Proyecto WHERE IdProyecto = @Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<Proyecto>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"Proyecto {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, Proyecto model)
        {
            if (model.IdProyecto != guid)
                throw new Exception($"Proyecto not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"UPDATE Proyectos
                SET 
                  idSubdireccion = @IdSubdireccion,
                  nombreProyecto = @NombreProyecto,
                  objetoProyecto = @ObjetoProyecto,
                  fechaInicioContrato = @FechaInicioContrato,
                  fechaFinalContratoContractual = @FechaFinalContratoContractual,
                  fechaFinalRealContrato = @FechaFinalRealContrato,
                  fechaSuscripcionContrato = @FechaSuscripcionContrato,                  
                  observaciones = @Observaciones,
                  valorContrastistaInicial = @ValorContrastistaInicial,
                  valorInterventoriaInicial = @ValorInterventoriaInicial,
                  fechaCreacion = @FechaCreacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idProyecto = @IdProyecto";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"Proyecto not was updated");
                    return (Guid)model.IdProyecto;
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
