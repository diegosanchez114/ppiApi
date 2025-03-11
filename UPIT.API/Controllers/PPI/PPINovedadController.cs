using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.API.PPI.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/ppi/[controller]")]
    [ApiController]
    public class PPINovedadController : Controller
    {        
        private readonly ILogger<PPINovedadController> _logger;
        private readonly LoggingService _loggingService;
        private readonly IPPINovedadRepository _repository;
        private readonly ResponseAPIManager _rm;

        public PPINovedadController(
           ILogger<PPINovedadController> logger,
           LoggingService loggingService,
           IPPINovedadRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _loggingService = loggingService;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PPINovedad model)
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
                var response = new ApiResponseDTO<PPINovedad>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("byAvance/{id}")]
        public async Task<IActionResult> GetAllByIdContract(Guid id)
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetAllByIdAvanceAsync(id);
                var response = new ApiResponseDTO<DataPaginatedDTO<Subdireccion>>(true, _rm.GetSuccessMessage(), data);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] PPINovedad model)
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
