using AddressBook.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AddressBook.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            try
            {
                var createdDepartment = await _departmentService.CreateAsync(dto);
                _logger.LogInformation("Successfully created Department with ID: {DepartmentId}", createdDepartment.Id);

                return Ok(new { data = createdDepartment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Department.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating Department.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _departmentService.GetAllAsync();
                _logger.LogInformation("Fetched {DepartmentCount} Departments.", list.Count);

                return Ok(new { data = list });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Departments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching Departments.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.DeleteAsync(id);
                _logger.LogInformation("Deleted Department with ID: {DepartmentId}", id);
                return NoContent(); 
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Department with ID: {DepartmentId} not found", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Department with ID: {DepartmentId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Department.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Department ID mismatch.");

            try
            {
                var result = await _departmentService.UpdateAsync(dto);
                if (!result)
                    return NotFound($"Department with ID {dto.Id} not found.");

                return Ok(new { message = "Department updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating department.");
            }
        }






    }
}