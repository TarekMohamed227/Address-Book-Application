using AddressBook.Application.DTOs;
using AddressBook.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;
        private readonly ILogger<JobTitleController> _logger;

        public JobTitleController(IJobTitleService jobTitleService, ILogger<JobTitleController> logger)
        {
            _jobTitleService = jobTitleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobTitleDto dto)
        {
            try
            {
                var createdJobTitle = await _jobTitleService.CreateAsync(dto);
                _logger.LogInformation("Successfully created JobTitle with ID: {JobTitleId}", createdJobTitle.Id);

                return Ok(new { data = createdJobTitle });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating JobTitle.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating JobTitle.");
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _jobTitleService.GetAllAsync();
                _logger.LogInformation("Fetched {JobTitleCount} JobTitles.", list.Count);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching JobTitles.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching JobTitles.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _jobTitleService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("JobTitle with ID {Id} not found for deletion.", id);
                    return NotFound(new { message = "JobTitle not found" });
                }

                _logger.LogInformation("Deleted JobTitle with ID: {Id}", id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting JobTitle with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting JobTitle.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobTitleDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            try
            {
                var success = await _jobTitleService.UpdateAsync(dto);
                if (!success)
                    return NotFound($"Job Title with ID {dto.Id} not found.");

                return Ok(new { message = "Job title updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating job title.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating job title.");
            }
        }





    }
}