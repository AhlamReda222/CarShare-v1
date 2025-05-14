// AdminService.cs
using CarShare.DAL.Models;
using CarShare.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShare.BLL.Services
{
    public class AdminService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<CarPost> _carPostRepo;
        private readonly IAuthService _authService;

        public AdminService(
            IGenericRepository<User> userRepo,
            IGenericRepository<CarPost> carPostRepo,
                    IAuthService authService)

        {
            _userRepo = userRepo;
            _carPostRepo = carPostRepo;
            _authService = authService;

        }

        public async Task<IEnumerable<User>> GetPendingOwnersAsync()
        {
            return await _userRepo.GetAllAsync(u => u.UserType == UserType.Owner && !u.IsApproved);
        }

        public async Task<bool> ApproveOwnerAsync(int ownerId)
        {
            var owner = await _userRepo.GetByIdAsync(ownerId);
            if (owner == null || owner.UserType != UserType.Owner)
                throw new Exception("Owner not found.");

            owner.IsApproved = true;
            _userRepo.Update(owner);
            return await _userRepo.SaveChangesAsync();
        }
        public async Task<bool> RejectOwnerAsync(int ownerId)
        {
            var owner = await _userRepo.GetByIdAsync(ownerId);
            if (owner == null || owner.UserType != UserType.Owner)
                throw new Exception("Owner not found.");

            _userRepo.Delete(owner);
            return await _userRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<CarPost>> GetPendingPostsAsync()
        {
            return await _carPostRepo.GetAllAsync(p => !p.IsApproved);
        }

        public async Task<bool> ApprovePostAsync(int postId)
        {
            var post = await _carPostRepo.GetByIdAsync(postId);
            if (post == null) throw new Exception("Post not found.");

            post.IsApproved = true;
            _carPostRepo.Update(post);
            return await _carPostRepo.SaveChangesAsync();
        }

        public async Task<bool> RejectPostAsync(int postId)
        {
            var post = await _carPostRepo.GetByIdAsync(postId);
            if (post == null) throw new Exception("Post not found.");

            _carPostRepo.Delete(post);
            return await _carPostRepo.SaveChangesAsync();
        }
    }
}