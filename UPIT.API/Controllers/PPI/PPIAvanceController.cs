using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Application.Services.PPI;
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
    public class PPIAvanceController : Controller
    {        
        private readonly ILogger<PPIAvanceController> _logger;
        private readonly LoggingService _loggingService;
        private readonly IPPIAvanceRepository _repository;
        private readonly IPPIContratoRepository _repositoryContrato;
        private readonly ResponseAPIManager _rm;

        public PPIAvanceController(
           ILogger<PPIAvanceController> logger,
           LoggingService loggingService,
           IPPIAvanceRepository repository,
           IPPIContratoRepository repositoryContrato,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _loggingService = loggingService;
            _repository = repository;
            _repositoryContrato = repositoryContrato;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PPIAvance model)
        {
            _logger.LogInformation("Start Post");

            try
            {
                var appPPIAvance = new PPIAvanceService(_repositoryContrato);
                model = await appPPIAvance.ValidateAvanceBeforeCreate(model);
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
                var response = new ApiResponseDTO<PPIAvance>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("byContrato/{id}")]
        public async Task<IActionResult> GetAllByIdContract(Guid id)
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetAllByIdContractAsync(id);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] PPIAvance model)
        {
            _logger.LogInformation("Start Put");

            try
            {
                var data = await _repository.UpdateAsync(id, model);
                var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);                
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
