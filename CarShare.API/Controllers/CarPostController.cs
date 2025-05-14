using CarShare.BLL.Dtos;
using CarShare.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarShare.DAL.Repository;
using CarShare.DAL.Pepository;
using System.Security.Claims;

namespace CarShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize(Roles = "Owner")]

    public class CarPostController : ControllerBase
    {
        private readonly CarPostService _carPostService;

        public CarPostController(CarPostService carPostService)
        {
            _carPostService = carPostService;
        }


        [HttpPost]
        [Authorize] // تأكد من وجود هذه السمة
        public async Task<IActionResult> Create([FromForm] CarPostCreateDto dto)
        {
            try
            {
                // تحقق من وجود User أولاً
                if (User?.Identity?.IsAuthenticated != true)
                {
                    return Unauthorized(new { Message = "User not authenticated" });
                }

                // تحقق من وجود الـ Claim
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest(new { Message = "User ID claim not found" });
                }

                // تحقق من أن القيمة ليست فارغة
                if (string.IsNullOrEmpty(userIdClaim.Value))
                {
                    return BadRequest(new { Message = "User ID value is empty" });
                }

                if (!int.TryParse(userIdClaim.Value, out var ownerId))
                {
                    return BadRequest(new { Message = "Invalid User ID format" });
                }

                var result = await _carPostService.CreatePostAsync(dto, ownerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
            

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CarPostUpdateDto dto)
        {
            // استخدم الخدمة لتحصل على الـ carPost
            var carPost = await _carPostService.GetByIdAsync(id);   
            if (carPost == null)
                return NotFound("Car post not found.");

            // تحديث البيانات
            carPost.Title = dto.Title;
            carPost.Description = dto.Description;
            carPost.CarType = dto.CarType;
            carPost.Brand = dto.Brand;
            carPost.Model = dto.Model;
            carPost.Transmission = dto.Transmission;
            carPost.Location = dto.Location;
            carPost.AvailableFrom = dto.AvailableFrom;
            carPost.AvailableTo = dto.AvailableTo;
            carPost.RentalPrice = dto.RentalPrice;

            // تحديث الصورة إذا كانت موجودة
            if (dto.CarImage != null && dto.CarImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.CarImage.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                // حفظ الصورة الجديدة
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.CarImage.CopyToAsync(stream);
                }

                carPost.ImageUrl = "/Images/" + fileName;  // تحديث الـ URL للصورة
            }

            // حفظ التحديثات في قاعدة البيانات باستخدام الخدمة
            await _carPostService.UpdateAsync(carPost);

            return Ok(new { Message = "Car post updated successfully", ImageUrl = carPost.ImageUrl });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var success = await _carPostService.DeleteAsync(id);
                return success ? Ok("Post deleted successfully.") : BadRequest("Cannot delete post.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("owner-posts/{ownerId}")]
        public async Task<IActionResult> GetOwnerPosts(int ownerId)
        {
            var posts = await _carPostService.GetPostsForOwnerAsync(ownerId);
            return Ok(posts);
        }

        [HttpPost("mark-as-returned/{postId}")]
        public async Task<IActionResult> MarkAsReturned(int postId)
        {
            var result = await _carPostService.MarkAsReturnedAsync(postId);
            return Ok(result);
        }

        [HttpPost("handle-proposal/{requestId}")]
        public async Task<IActionResult> HandleProposal(int requestId, [FromQuery] bool accept)
        {
            var result = await _carPostService.HandleProposalAsync(requestId, accept);
            return Ok(result);
        }

        [AllowAnonymous] // أو [Authorize] بس بدون تحديد الدور

        [HttpGet("approved")]
        public async Task<IActionResult> GetAllApprovedPosts()
        {
            var posts = await _carPostService.GetAllApprovedPostsAsync();
            return Ok(posts);
        }


        [AllowAnonymous] // أو [Authorize] بس بدون تحديد الدور

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string carType, [FromQuery] decimal maxPrice)
        {
            var results = await _carPostService.SearchAsync(carType, maxPrice);
            return Ok(results);
        }



    }
}