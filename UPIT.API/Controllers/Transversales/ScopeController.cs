using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using UPIT.Domain.DTOs;
using Microsoft.Identity.Web.Resource;


namespace UPIT.API.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/transversales/[controller]")]
    [ApiController]
    public class ScopeController : Controller
    {
        private readonly ILogger<ScopeController> _logger;
        private readonly IScopeRepository _repository;
        private readonly ResponseAPIManager _rm;

        public ScopeController(
           ILogger<ScopeController> logger,
           IScopeRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Scope model)
        {
            _logger.LogInformation("Start Post");

            try 
            { 
                var data = await _repository.CreateAsync(model);
                var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
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
                var response = new ApiResponseDTO<Scope>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int limit, [FromQuery] string? filter)
        {
            _logger.LogInformation("Start Get Results Paginated");

            try
            {
                var data = await _repository.GetByFilterPaginatedAsync(page, limit, filter);
                var response = new ApiResponseDTO<DataPaginatedDTO<Scope>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Scope model)
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
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }
    }
}
