using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.Repositories;
using UPIT.Domain.Models.Models;
using UPIT.Domain.Models.PPI;

namespace UPIT.API.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/transversales/[controller]")]
    [ApiController]
    public class ParametricaController : ControllerBase
    {
        private readonly ILogger<ParametricaController> _logger;
        private readonly IParametricaRepository _repository;
        private readonly ResponseAPIManager _rm;

        public ParametricaController(
           ILogger<ParametricaController> logger,
           IParametricaRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Parametrica model)
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
                var response = new ApiResponseDTO<PPIAvance>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetAllByName(string name)
        {
            _logger.LogInformation("Start Get all");

            try
            {
                var data = await _repository.GetParametricasByName(name);
                var response = new ApiResponseDTO<DataPaginatedDTO<Parametrica>>(true, _rm.GetSuccessMessage(), data);
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
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetByFilterPaginatedAsync(page, limit, filter);
                var response = new ApiResponseDTO<DataPaginatedDTO<Parametrica>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Parametrica model)
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
