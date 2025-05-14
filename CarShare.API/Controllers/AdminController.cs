// AdminController.cs
using CarShare.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarShare.BLL.Services;
using Microsoft.AspNetCore.Authorization;
namespace CarShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize(Roles = "Admin")]


    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("pending-owners")]
        public async Task<IActionResult> GetPendingOwners()
        {
            var result = await _adminService.GetPendingOwnersAsync();
            return Ok(result);
        }

        [HttpPost("approve-owner/{ownerId}")]
        public async Task<IActionResult> ApproveOwner(int ownerId)
        {
            var success = await _adminService.ApproveOwnerAsync(ownerId);
            return success ? Ok("Owner approved.") : BadRequest("Failed to approve owner.");
        }

        [HttpPost("reject-owner/{ownerId}")]
        public async Task<IActionResult> RejectOwner(int ownerId)
        {
            var success = await _adminService.RejectOwnerAsync(ownerId);
            return success ? Ok("Owner rejected.") : BadRequest("Failed to reject owner.");
        }

        [HttpGet("pending-posts")]
        public async Task<IActionResult> GetPendingPosts()
        {
            var result = await _adminService.GetPendingPostsAsync();
            return Ok(result);
        }

        [HttpPost("approve-post/{postId}")]
        public async Task<IActionResult> ApprovePost(int postId)
        {
            var success = await _adminService.ApprovePostAsync(postId);
            return success ? Ok("Post approved.") : BadRequest("Failed to approve post.");
        }

        [HttpPost("reject-post/{postId}")]
        public async Task<IActionResult> RejectPost(int postId)
        {
            var success = await _adminService.RejectPostAsync(postId);
            return success ? Ok("Post rejected.") : BadRequest("Failed to reject post.");
        }
    }
}
