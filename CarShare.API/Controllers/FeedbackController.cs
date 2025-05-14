using CarShare.API.Hubs;
using CarShare.BLL.Dtos;
using CarShare.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using CarShare.API.Hubs;
using CarShare.BLL.Dtos;
namespace CarShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly IHubContext<FeedbackHub> _hub;

        public FeedbackController(FeedbackService feedbackService, IHubContext<FeedbackHub> hub)
        {
            _feedbackService = feedbackService;
            _hub = hub;
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackCreateDto dto)
        {
            var renterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var feedback = await _feedbackService.AddFeedbackAsync(renterId, dto);

            await _hub.Clients.All.SendAsync("ReceiveFeedback", new
            {
                CarPostId = feedback.CarPostId,
                Comment = feedback.Comment,
                Rating = feedback.Rating,
                RenterId = feedback.RenterId
            });

            return Ok("Feedback submitted successfully.");
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetFeedbacksForPost(int postId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksForPost(postId);
            return Ok(feedbacks);
        }
    }

}
