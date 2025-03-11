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
    public class SubdireccionController : Controller
    {
        private readonly ILogger<SubdireccionController> _logger;
        private readonly ISubdireccionRepository _repository;
        private readonly ResponseAPIManager _rm;

        public SubdireccionController(
           ILogger<SubdireccionController> logger,
           ISubdireccionRepository repository,
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
                var response = new ApiResponseDTO<Subdireccion>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.GetAllAsync();
                var response = new ApiResponseDTO<DataPaginatedDTO<Subdireccion>>(true, _rm.GetSuccessMessage(), data);
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
