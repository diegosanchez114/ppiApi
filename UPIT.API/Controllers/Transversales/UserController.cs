using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPIT.API.Services;
using UPIT.Application.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTOs;
using UPIT.Domain.Models;
using UPIT.Domain.Interfaces;
using Microsoft.Identity.Web.Resource;


namespace UPIT.API.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/transversales/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _repository;
        private readonly ResponseAPIManager _rm;

        public UserController(
           ILogger<UserController> logger,
           IUserRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User model)
        {
            _logger.LogInformation("Start Post");

            try 
            {
                if (!PasswordHasher.IsBase64String(model.UserPassword))
                {
                    var response = new ApiResponseDTO<string>(false, _rm.GetInvalidFormatPassword(), "");
                    return BadRequest(response);
                }
                else
                {

                    model.UserPassword = PasswordHasher.HashPassword(model.UserPassword);
                    var data = await _repository.CreateAsync(model);
                    var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);
                    return Ok(response);
                }
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
                var response = new ApiResponseDTO<User>(true, _rm.GetSuccessMessage(), data);
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
                var response = new ApiResponseDTO<DataPaginatedDTO<User>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User model)
        {
            _logger.LogInformation("Start Put");

            if (model.UserPassword != null && !PasswordHasher.IsBase64String(model.UserPassword))
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetInvalidFormatPassword(), "");
                return BadRequest(response);
            }
            else
            {
                if (model.UserPassword != null)
                    model.UserPassword = PasswordHasher.HashPassword(model.UserPassword);
                var data = await _repository.UpdateAsync(id, model);
                var response = new ApiResponseDTO<Guid>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }            
        }

        [HttpPut("state/{id}")]
        public async Task<IActionResult> PutState(Guid id, [FromBody] bool state)
        {
            _logger.LogInformation("Start Put");

            try
            {
                var data = await _repository.UpdateStateAsync(id, state);
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
