using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using CarShare.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShare.BLL.Services
{

    public class FeedbackService
    {
        private readonly IGenericRepository<Feedback> _feedbackRepo;
        private readonly IGenericRepository<RentalRequest> _rentalRepo;

        public FeedbackService(
            IGenericRepository<Feedback> feedbackRepo,
            IGenericRepository<RentalRequest> rentalRepo
)
        {
            _feedbackRepo = feedbackRepo;
            _rentalRepo = rentalRepo;
        }

        public async Task<Feedback> AddFeedbackAsync(int renterId, FeedbackCreateDto dto)
        {
            var rented = await _rentalRepo.GetAllAsync(r =>
                r.RenterId == renterId &&
                r.CarPostId == dto.CarPostId &&
                r.Status == RequestStatus.Approved);

            if (!rented.Any())
                throw new Exception("Only renters who rented this car can leave feedback.");

            var feedback = new Feedback
            {
                CarPostId = dto.CarPostId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                RenterId = renterId
            };

            await _feedbackRepo.AddAsync(feedback);
            await _feedbackRepo.SaveChangesAsync();

            return feedback; // رجّعي الفيدباك بدل الرسالة
        }

        public async Task<List<FeedbackResponseDto>> GetFeedbacksForPost(int postId)
        {
            var feedbacks = await _feedbackRepo.GetAllAsync(f => f.CarPostId == postId, includes: new[] { "Renter" });

            return feedbacks.Select(f => new FeedbackResponseDto
            {
                Id = f.Id,
                Rating = f.Rating,
                Comment = f.Comment,
                RenterName = f.Renter?.Username
            }).ToList();
        }
    }

}