using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using UPIT.Domain.DTO.PPI;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;
using UPIT.Infraestructure.Logging;

namespace UPIT.Infraestructure.Repositories.PPI
{
    public class PPIProyectoRepository: IPPIProyectoRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger _logger;

        public PPIProyectoRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(PPIProyecto model)
        {
            model.FechaCreacion = DateTime.Now;
            model.FechaActualizacion = DateTime.Now;

            string sql = @"INSERT INTO PPI_Proyectos (
                  idEntidadResponsable,
                  codigo,
                  nombreLargo,
                  nombreCorto,
                  categoria,
                  modo,
                  estaRadicado,
                  codigoCorredor,
                  tieneEstudiosYDisenio,
                  programa,
                  priorizacionInvias,
                  bpinPng,                  
                  observaciones,
                  tieneContratos,
                  tieneContratosObservacion,
                  fechaCreacion,
                  fechaActualizacion
                )
                OUTPUT INSERTED.idProyecto
                VALUES (
                  @IdEntidadResponsable,
                  @Codigo,
                  @NombreLargo,
                  @NombreCorto,
                  @Categoria,
                  @Modo,
                  @EstaRadicado,
                  @CodigoCorredor,
                  @TieneEstudiosYDisenio,
                  @Programa,
                  @PriorizacionInvias,
                  @BpinPng,                  
                  @Observaciones,
                  @TieneContratos,
                  @TieneContratosObservacion,
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
                        throw new Exception($"PPIProyecto not was created");
                    return idInserted;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<PPIProyectoDTO>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            string condition = "";

            if (filter != null)
                condition = @"WHERE UPPER(nombreLargo) LIKE UPPER('%' + @Filter + '%') 
                                OR UPPER(nombreCorto) LIKE UPPER('%' + @Filter + '%')";

            string sql = $"SELECT p.*, e.nombre AS NombreEntidad, " +
                $"CASE WHEN p.categoria IS NULL OR p.categoria = '' THEN '' " + 
                $"ELSE (SELECT pa.valor FROM Parametricas pa WHERE p.categoria = pa.Id) " +
                $"END AS NombreCategoria, " +
                $"CASE WHEN p.modo IS NULL OR p.modo = '' THEN '' " + 
                $"ELSE (SELECT pa.valor FROM Parametricas pa WHERE p.modo = pa.Id) " +
                $"END AS NombreModo, " + 
                $"CASE WHEN p.programa IS NULL OR p.programa = '' THEN '' " +
                $"ELSE (SELECT pa.valor FROM Parametricas pa WHERE p.programa = pa.Id)" +
                $"END AS NombrePrograma, " +
                $"CASE WHEN p.priorizacionInvias IS NULL OR p.priorizacionInvias = '' THEN '' " +
                $"ELSE (SELECT pa.valor FROM Parametricas pa WHERE p.priorizacionInvias = pa.Id) " +
                $"END AS NombrePriorizacionInvias," + 
                $"(SELECT SUM(c.valorContrato) FROM PPI_Contratos c JOIN PPI_Proyectos pr ON c.idProyecto = pr.idProyecto " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalValorProyecto, " +
                $"(SELECT SUM(a.porcentajeEjecutado) FROM PPI_Avances a JOIN PPI_Contratos c ON a.idContrato = c.idContrato " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalPorcentajeEjecutado, " +
                $"(SELECT SUM(a.valorEjecutado) FROM PPI_Avances a JOIN PPI_Contratos c ON a.idContrato = c.idContrato " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalValorEjecutado " +
                $"FROM PPI_Proyectos p " +
                $"LEFT JOIN Entidades e ON p.idEntidadResponsable = e.idEntidad {condition} " +            
                $"ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Page}, {@Limit}, {@Filter}", sql, page, limit, filter);

                    // Calcular el offset
                    int offset = (int)((page - 1) * limit)!;

                    var parameters = new DynamicParameters();
                    parameters.Add("@Offset", offset);
                    parameters.Add("@Limit", limit);

                    if (filter != null)                    
                        parameters.Add("@Filter", filter);                    

                    // Calculate offset
                    IEnumerable<PPIProyectoDTO> list = await connection.QueryAsync<PPIProyectoDTO>(sql, parameters);

                    if (list == null)
                        throw new Exception($"PPIProyecto not was found");

                    //Get total                    
                    string queryCount = $"SELECT COUNT(*) FROM PPI_Proyectos {condition}";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount, parameters);

                    var obj = new DataPaginatedDTO<PPIProyectoDTO>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataPaginatedDTO<PPIProyectoDTO>> GetByEntityFilterPaginatedAsync(Guid id, int? page, int? limit, string? filter)
        {
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;
           
            // Condicion que filtar por la entidad a la cual pertenece el usuario logeado.
            string condition = "WHERE p.idEntidadResponsable = @Id ";

            if (filter != null)                
                condition += @"AND (UPPER(p.nombreLargo) LIKE UPPER('%' + @Filter + '%') 
                            OR UPPER(p.nombreCorto) LIKE UPPER('%' + @Filter + '%'))";

            string sql = $"SELECT p.*, e.nombre AS NombreEntidad, " +
                $"(SELECT COUNT(*) FROM PPI_Contratos c WHERE c.idProyecto = p.idProyecto) as CantContratos, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE TRY_CONVERT(UNIQUEIDENTIFIER, p.categoria) = pa.Id) AS NombreCategoria, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE TRY_CONVERT(UNIQUEIDENTIFIER, p.modo) = pa.Id) AS NombreModo, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE TRY_CONVERT(UNIQUEIDENTIFIER, p.programa) = pa.Id) AS NombrePrograma, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE TRY_CONVERT(UNIQUEIDENTIFIER, p.priorizacionInvias) = pa.Id) AS NombrePriorizacionInvias, " +
                $"(SELECT SUM(c.valorContrato) FROM PPI_Contratos c JOIN PPI_Proyectos pr ON c.idProyecto = pr.idProyecto " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalValorProyecto, " +
                $"(SELECT SUM(a.porcentajeEjecutado) FROM PPI_Avances a JOIN PPI_Contratos c ON a.idContrato = c.idContrato " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalPorcentajeEjecutado, " +
                $"(SELECT SUM(a.valorEjecutado) FROM PPI_Avances a JOIN PPI_Contratos c ON a.idContrato = c.idContrato " +
                $"WHERE c.idProyecto = p.idProyecto) as TotalValorEjecutado " +
                $"FROM PPI_Proyectos p " +
                $"LEFT JOIN Entidades e ON p.idEntidadResponsable = e.idEntidad {condition} " +
                $"ORDER BY fechaCreacion OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Id}, {@Page}, {@Limit}, {@Filter}", sql, id, page, limit, filter);
                    //connection.Open();

                    // Calculate offset
                    int offset = (int)((page - 1) * limit)!;

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id);
                    parameters.Add("@Offset", offset);
                    parameters.Add("@Limit", limit);

                    if (filter != null)
                        parameters.Add("@Filter", filter);

                    IEnumerable<PPIProyectoDTO> list = await connection.QueryAsync<PPIProyectoDTO>(sql, parameters);

                    if (list == null)
                        throw new Exception($"PPIProyecto not was found");

                    //Get total                    
                    string queryCount = $"SELECT COUNT(*) FROM PPI_Proyectos p {condition}";
                    int total = await connection.ExecuteScalarAsync<int>(queryCount, parameters);

                    var obj = new DataPaginatedDTO<PPIProyectoDTO>(total, list.ToList());
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PPIProyectoDTO> GetByIdAsync(Guid guid)
        {
            string sql = $"SELECT p.*, e.nombre AS NombreEntidad, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE p.categoria = pa.Id) as NombreCategoria, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE p.modo = pa.Id) as NombreModo, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE p.programa = pa.Id) as NombrePrograma, " +
                $"(SELECT pa.valor FROM Parametricas pa WHERE p.priorizacionInvias = pa.Id) as NombrePriorizacionInvias " +
                $"FROM PPI_Proyectos p " +
                $"LEFT JOIN Entidades e ON p.idEntidadResponsable = e.idEntidad " +
                $"WHERE IdProyecto = @Id;";            

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Guid}", sql, guid);
                    //connection.Open();

                    var model = await connection.QueryFirstOrDefaultAsync<PPIProyectoDTO>(sql, new { Id = guid });

                    if (model == null)
                        throw new Exception($"PPIProyecto {guid} not was found");

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> UpdateAsync(Guid guid, PPIProyecto model)
        {
            if (model.IdProyecto != guid)
                throw new Exception($"PPIProyecto not was updated");

            model.FechaActualizacion = DateTime.Now;

            string sql = @"
                UPDATE PPI_Proyectos
                SET 
                  idEntidadResponsable = @IdEntidadResponsable,
                  codigo = @Codigo,
                  nombreLargo = @NombreLargo,
                  nombreCorto = @NombreCorto,
                  categoria = @Categoria,
                  modo = @Modo,
                  estaRadicado = @EstaRadicado,
                  codigoCorredor = @CodigoCorredor,
                  tieneEstudiosYDisenio = @TieneEstudiosYDisenio,
                  programa = @Programa,
                  priorizacionInvias = @PriorizacionInvias,
                  bpinPng = @BpinPng,                  
                  observaciones = @Observaciones,
                  tieneContratos = @TieneContratos,
                  tieneContratosObservacion = @TieneContratosObservacion,
                  fechaActualizacion = @FechaActualizacion
                WHERE 
                  idProyecto = @IdProyecto;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query log
                    _logger.LogInformation("Executing SQL: {Sql}, \nParameters: {@Parameters}", sql, DapperLoggingParameters.GetParametersValue(model));

                    int rowsAffected = await connection.ExecuteAsync(sql, model);

                    if (rowsAffected == 0)
                        throw new Exception($"PPIProyecto not was updated");
                    return (Guid)model.IdProyecto!;
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
