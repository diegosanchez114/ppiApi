using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using UPIT.API.Services;
using UPIT.Domain.DTO;
using UPIT.Domain.DTO.PPI;
using UPIT.Domain.Interfaces.PPI;

namespace UPIT.API.PPI.Controllers
{
    [RequiredScope("UPITApp")]
    [Authorize]
    [Route("api/ppi/[controller]")]
    [ApiController]
    public class PPIAlertsController : Controller
    {        
        private readonly ILogger<PPIAlertsController> _logger;
        private readonly LoggingService _loggingService;
        private readonly IPPIAlertsRepository _repository;
        private readonly ResponseAPIManager _rm;

        public PPIAlertsController(
           ILogger<PPIAlertsController> logger,
           LoggingService loggingService,
           IPPIAlertsRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _loggingService = loggingService;
            _repository = repository;
            _rm = rm;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAllByIdContract()
        {
            _logger.LogInformation("Start Get Results");

            try
            {
                var data = await _repository.getAlertsAvances();
                var response = new ApiResponseDTO<PPIAlerDTO>(true, _rm.GetSuccessMessage(), data);
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
