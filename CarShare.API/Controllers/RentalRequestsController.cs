using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CarShare.BLL.Dtos;
using CarShare.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarShare.API.Controllers
{

    // Controller
    [ApiController]
    [Route("api/[controller]")]

    public class RentalRequestsController : ControllerBase
    {
        private readonly RentalRequestService _rentalRequestService;

        public RentalRequestsController(RentalRequestService rentalRequestService)
        {
            _rentalRequestService = rentalRequestService;
        }

        [Authorize(Roles = "Renter")]
        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromForm] RentalRequestDto dto)
        {
            try
            {
                var renterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                dto.RenterId = renterId; // نضبط الـ DTO من التوكن

                var result = await _rentalRequestService.SubmitRequestAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("my-requests")]
        public async Task<ActionResult<List<RentalRequestResponseDto>>> GetMyRequests()
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var rentalRequests = await _rentalRequestService.GetRequestsForOwnerAsync(ownerId);

            if (rentalRequests == null || !rentalRequests.Any())
            {
                return NotFound("No rental requests found.");
            }

            return Ok(rentalRequests);
        }


    }
}