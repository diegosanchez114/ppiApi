using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTOs;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.API.Controllers.ProyectosInternos
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContratistaController : Controller
    {
        private readonly ILogger<ContratistaController> _logger;
        private readonly IContratistaRepository _repository;
        private readonly ResponseAPIManager _rm;

        public ContratistaController(
           ILogger<ContratistaController> logger,
           IContratistaRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _repository = repository;
            _rm = rm;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Start Get by Id");

            try
            {
                var data = await _repository.GetByIdAsync(id);
                var response = new ApiResponseDTO<Contratista>(true, _rm.GetSuccessMessage(), data);
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
                var response = new ApiResponseDTO<DataPaginatedDTO<Contratista>>(true, _rm.GetSuccessMessage(), data);
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
