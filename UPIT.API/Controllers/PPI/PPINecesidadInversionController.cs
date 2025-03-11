using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.API.PPI.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/ppi/[controller]")]
    [ApiController]
    public class PPINecesidadInversionController : Controller
    {
        private readonly ILogger<PPINecesidadInversionController> _logger;
        private readonly LoggingService _loggingService;
        private readonly IPPINecesidadInversionRepository _repository;
        private readonly ResponseAPIManager _rm;

        public PPINecesidadInversionController(
           ILogger<PPINecesidadInversionController> logger,
           LoggingService loggingService,
           IPPINecesidadInversionRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _loggingService = loggingService;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PPINecesidadInversion model)
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
                var response = new ApiResponseDTO<PPINecesidadInversion>(true, _rm.GetSuccessMessage(), data);
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
                var response = new ApiResponseDTO<PPINecesidadInversion>(true, _rm.GetSuccessMessage(), data);
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
        public async Task<IActionResult> Put(Guid idProject, Guid id, [FromBody] PPINecesidadInversion model)
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
