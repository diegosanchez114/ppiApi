using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.API.PPI.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/ppi/[controller]")]
    [ApiController]
    public class PPIContratoController : Controller
    {
        private readonly ILogger<PPIContratoController> _logger;
        private readonly LoggingService _loggingService;
        private readonly IPPIContratoRepository _repository;
        private readonly ResponseAPIManager _rm;

        public PPIContratoController(
           ILogger<PPIContratoController> logger,
           LoggingService loggingService,
           IPPIContratoRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _loggingService = loggingService;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PPIContrato model)
        {
            _logger.LogInformation("Start Post");

            try
            {                
                var data = await _repository.CreateAsync(model);
                var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);
                _loggingService.CreateLog(_rm.GetSuccessMessage(), this.GetType().Name, correlationId: response.Data!.ToString()!);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Start Get by Id");
            
            try
            {
                var data = await _repository.GetByIdAsync(id);
                var response = new ApiResponseDTO<PPIContrato>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("byProyecto/{id}")]
        public async Task<IActionResult> GetAllByIdProject(Guid id)
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetAllByIdProjectAsync(id);
                var response = new ApiResponseDTO<DataPaginatedDTO<PPIContrato>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("byProyecto/{idProject}/byEntidad/{idEntity}")]
        public async Task<IActionResult> GetAllByIdProjectAndIdEntity(Guid idProject, Guid idEntity)
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetAllByIdProjectAndIdEntityAsync(idProject, idEntity);
                var response = new ApiResponseDTO<DataPaginatedDTO<PPIContrato>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid idProject, Guid id, [FromBody] PPIContrato model)
        {
            _logger.LogInformation("Start Put");

            try
            {
                var data = await _repository.UpdateAsync(id, model);
                var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);
                _loggingService.CreateLog(_rm.GetSuccessMessage(), this.GetType().Name, correlationId: response.Data!.ToString()!);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }
    }
}
