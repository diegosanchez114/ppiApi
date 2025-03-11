﻿using Microsoft.AspNetCore.Authorization;
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
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleRepository _repository;
        private readonly ResponseAPIManager _rm;

        public RoleController(
           ILogger<RoleController> logger,
           IRoleRepository repository,
           ResponseAPIManager rm
        )
        {
            _logger = logger;
            _repository = repository;
            _rm = rm;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role model)
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
                var response = new ApiResponseDTO<Role>(true, _rm.GetSuccessMessage(), data);
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
            _logger.LogInformation("Start Get Results Paginated");

            try
            {
                var data = await _repository.GetAllAsync();
                var response = new ApiResponseDTO<DataPaginatedDTO<Role>>(true, _rm.GetSuccessMessage(), data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponseDTO<string>(false, _rm.GetErrorMessage(), ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Role model)
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
