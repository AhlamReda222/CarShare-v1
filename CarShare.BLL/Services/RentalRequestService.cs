// RentalRequestService.cs
using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using CarShare.DAL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
public class RentalRequestService
{
    private readonly IGenericRepository<RentalRequest> _rentalRepo;
    private readonly IGenericRepository<CarPost> _carPostRepo;

    public RentalRequestService(
        IGenericRepository<RentalRequest> rentalRepo,
        IGenericRepository<CarPost> carPostRepo)
    {
        _rentalRepo = rentalRepo;
        _carPostRepo = carPostRepo;
    }

    public async Task<string> SubmitRequestAsync(RentalRequestDto dto)
    {
        var car = await _carPostRepo.GetByIdAsync(dto.CarPostId);
        if (car == null || car.RentalStatus != RentalStatus.Available || !car.IsApproved)
            throw new Exception("Invalid or unavailable car.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string licensePath = null;
        string proposalPath = null;

        if (dto.License != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.License.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.License.CopyToAsync(stream);
            licensePath = $"/uploads/{fileName}";
        }

        if (dto.ProposalDocument != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ProposalDocument.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.ProposalDocument.CopyToAsync(stream);
            proposalPath = $"/uploads/{fileName}";
        }

        var rental = new RentalRequest
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Location = dto.Location,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CarPostId = dto.CarPostId,
            RenterId = dto.RenterId,
            LicensePath = licensePath,
            ProposalDocumentPath = proposalPath,
            Status = RequestStatus.Pending
        };

        await _rentalRepo.AddAsync(rental);
        await _rentalRepo.SaveChangesAsync();

        return "Rental request submitted successfully.";
    }

    public async Task<List<RentalRequestResponseDto>> GetRequestsForOwnerAsync(int ownerId)
    {
        // جلب كل الإعلانات الخاصة بالمالك
        var posts = await _carPostRepo.GetAllAsync(p => p.OwnerId == ownerId);

        // استخراج المعرفات الخاصة بالإعلانات
        var postIds = posts.Select(p => p.Id).ToList();

        // جلب كل طلبات التأجير المرتبطة بتلك الإعلانات
        var requests = await _rentalRepo.GetAllAsync(r => postIds.Contains(r.CarPostId));

        // تحويل طلبات التأجير إلى RentalRequestResponseDto
        var rentalRequestDtos = requests.Select(r => new RentalRequestResponseDto
        {
            Id = r.Id,
            FullName = r.FullName,
            Email = r.Email,
            Location = r.Location,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            LicensePath = r.LicensePath,
            ProposalDocumentPath = r.ProposalDocumentPath,
            Status = r.Status,  // Assuming `Status` is part of `RentalRequest`
            CarPostId = r.CarPostId,
            RenterId = r.RenterId // Assuming this property exists on `RentalRequest`
        }).ToList();

        return rentalRequestDtos;
    }



}