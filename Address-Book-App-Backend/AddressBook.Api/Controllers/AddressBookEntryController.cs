using AddressBook.Application.DTOs;
using AddressBook.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookEntryController : ControllerBase
    {
        private readonly IAddressBookEntryService _entryService;
        private readonly ILogger<JobTitleController> _logger;

        public AddressBookEntryController(IAddressBookEntryService entryService, ILogger<JobTitleController> logger)
        {
            _entryService = entryService;
            _logger = logger;
        }

        [HttpPost]
        [RequestSizeLimit(5_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateAddressBookEntryDto dto)
        {
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(dto.Photo.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type. Only JPG, JPEG, and PNG are allowed." });
                }
            }

            
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            var id = await _entryService.CreateAsync(dto);
            var createdEntry = await _entryService.GetByIdAsync(id);

            return Ok(new { data = createdEntry });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _entryService.GetAllAsync();
            return Ok(entries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateAddressBookEntryDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var success = await _entryService.UpdateAsync(dto);
            if (!success) return NotFound();

            return Ok(new { message = "Updated successfully" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _entryService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Failed to delete AddressBookEntry with ID: {EntryId}. Entry not found.", id);
                    return NotFound(new { message = "AddressBookEntry not found" });
                }
                _logger.LogInformation("Deleted AddressBookEntry with ID: {EntryId}.", id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting AddressBookEntry with ID: {EntryId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting AddressBookEntry.");
            }
        }
    }

}