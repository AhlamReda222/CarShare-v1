// CarPostService.cs
using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using CarShare.DAL.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShare.BLL.Services
{
    public class CarPostService
    {
        private readonly IGenericRepository<CarPost> _carPostRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<RentalRequest> _rentalRepo;

        public CarPostService(
            IGenericRepository<CarPost> carPostRepo,
            IGenericRepository<User> userRepo,
            IGenericRepository<RentalRequest> rentalRepo)
        {
            _carPostRepo = carPostRepo;
            _userRepo = userRepo;
            _rentalRepo = rentalRepo;
        }

        public async Task<CarPostResponseDto> CreatePostAsync(CarPostCreateDto dto, int ownerId)
        {
            // Save image to wwwroot/Images
            var imageUrl = await SaveImage(dto.CarImage);

            var carPost = new CarPost
            {
                Title = dto.Title,
                Description = dto.Description,
                CarType = dto.CarType,
                Brand = dto.Brand,
                Model = dto.Model,
                Transmission = dto.Transmission,
                Location = dto.Location,
                AvailableFrom = dto.AvailableFrom,
                AvailableTo = dto.AvailableTo,
                RentalPrice = dto.RentalPrice,
                ImageUrl = imageUrl,
                RentalStatus = RentalStatus.Available,
                IsApproved = false,
                OwnerId = ownerId
            };

            await _carPostRepo.AddAsync(carPost);
            await _carPostRepo.SaveChangesAsync();

            return MapToDto(carPost);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/Images/{fileName}";
        }

        private CarPostResponseDto MapToDto(CarPost carPost)
        {
            return new CarPostResponseDto
            {
                Id = carPost.Id,
                Title = carPost.Title,
                Description = carPost.Description,
                CarType = carPost.CarType,
                Brand = carPost.Brand,
                Model = carPost.Model,
                Transmission = carPost.Transmission,
                Location = carPost.Location,
                RentalPrice = carPost.RentalPrice,
                AvailableFrom = carPost.AvailableFrom,
                AvailableTo = carPost.AvailableTo,
                ImageUrl = carPost.ImageUrl,
                RentalStatus = carPost.RentalStatus,
                IsApproved = carPost.IsApproved,
                OwnerId = carPost.OwnerId,
                OwnerName = carPost.Owner?.Username
            };
        }

        public async Task<string> UpdateAsync(int id, CarPostCreateDto dto, string imageUrl)
        {
            var post = await _carPostRepo.GetByIdAsync(id);
            if (post == null || post.RentalStatus == RentalStatus.Rented)
                throw new Exception("Post not found or currently rented.");

            post.Title = dto.Title;
            post.Description = dto.Description;
            post.CarType = dto.CarType;
            post.Brand = dto.Brand;
            post.Model = dto.Model;
            post.Transmission = dto.Transmission;
            post.Location = dto.Location;
            post.RentalPrice = dto.RentalPrice;
            post.AvailableFrom = dto.AvailableFrom;
            post.AvailableTo = dto.AvailableTo;

            // تحديث الصورة فقط لو فيه صورة جديدة اتبعتت
            if (!string.IsNullOrEmpty(imageUrl))
            {
                post.ImageUrl = imageUrl;
            }

            _carPostRepo.Update(post);
            await _carPostRepo.SaveChangesAsync();

            return "Post updated successfully.";
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _carPostRepo.GetByIdAsync(id);
            if (post == null || post.RentalStatus == RentalStatus.Rented)
                throw new Exception("Cannot delete rented or non-existent post.");

            _carPostRepo.Delete(post);
            return await _carPostRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<CarPostResponseDto>> GetPostsForOwnerAsync(int ownerId)
        {
            var posts = await _carPostRepo.GetAllAsync(p => p.OwnerId == ownerId);

            return posts.Select(p => new CarPostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CarType = p.CarType,
                Brand = p.Brand,
                Model = p.Model,
                Transmission = p.Transmission,
                Location = p.Location,
                RentalPrice = p.RentalPrice,
                AvailableFrom = p.AvailableFrom,
                AvailableTo = p.AvailableTo,
                ImageUrl = p.ImageUrl,
                RentalStatus = p.RentalStatus,
                IsApproved = p.IsApproved,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner?.Username
            });
        }

        public async Task<string> MarkAsReturnedAsync(int postId)
        {
            var post = await _carPostRepo.GetByIdAsync(postId);
            if (post == null) throw new Exception("Post not found.");

            post.RentalStatus = RentalStatus.Available;
            _carPostRepo.Update(post);
            await _carPostRepo.SaveChangesAsync();

            return "Car marked as returned.";
        }

        public async Task<string> HandleProposalAsync(int requestId, bool accept)
        {
            var request = await _rentalRepo.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found.");

            var car = await _carPostRepo.GetByIdAsync(request.CarPostId);
            if (car == null) throw new Exception("Car post not found.");

            if (accept)
            {
                request.Status = RequestStatus.Approved;
                car.RentalStatus = RentalStatus.Rented;

                // رفض باقي الطلبات
                var others = await _rentalRepo.GetAllAsync(r => r.CarPostId == car.Id && r.Id != requestId);
                foreach (var r in others) r.Status = RequestStatus.Rejected;
            }
            else
            {
                request.Status = RequestStatus.Rejected;
            }

            _rentalRepo.Update(request);
            _carPostRepo.Update(car);
            await _rentalRepo.SaveChangesAsync();
            await _carPostRepo.SaveChangesAsync();

            return "Proposal handled.";
        }

        public async Task<IEnumerable<CarPostResponseDto>> GetAllApprovedPostsAsync()
        {
            var posts = await _carPostRepo.GetAllAsync(p =>
                p.IsApproved == true
            );

            return posts.Select(p => new CarPostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CarType = p.CarType,
                Brand = p.Brand,
                Model = p.Model,
                Transmission = p.Transmission,
                Location = p.Location,
                RentalPrice = p.RentalPrice,
                AvailableFrom = p.AvailableFrom,
                AvailableTo = p.AvailableTo,
                ImageUrl = p.ImageUrl,
                RentalStatus = p.RentalStatus,
                IsApproved = p.IsApproved,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner?.Username
            });
        }


        // 3. للبحث المتقدم بناءً على النوع والسعر الأقصى
        public async Task<IEnumerable<CarPostResponseDto>> SearchAsync(string carType, decimal maxPrice)
        {
            var posts = await _carPostRepo.GetAllAsync(p =>
                p.IsApproved &&
                p.RentalStatus == RentalStatus.Available &&
                p.CarType.ToLower() == carType.ToLower() &&
                p.RentalPrice <= maxPrice
            );

            return posts.Select(p => new CarPostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CarType = p.CarType,
                Brand = p.Brand,
                Model = p.Model,
                Transmission = p.Transmission,
                Location = p.Location,
                RentalPrice = p.RentalPrice,
                AvailableFrom = p.AvailableFrom,
                AvailableTo = p.AvailableTo,
                ImageUrl = p.ImageUrl,
                RentalStatus = p.RentalStatus,
                IsApproved = p.IsApproved,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner?.Username
            });
        }
        public async Task<CarPost> GetByIdAsync(int id)
        {
            return await _carPostRepo.GetByIdAsync(id); // استخدام GetByIdAsync من GenericRepository
        }
        public async Task UpdateAsync(CarPost carPost)
        {
            _carPostRepo.Update(carPost);
            await _carPostRepo.SaveChangesAsync();
        }
    }
}