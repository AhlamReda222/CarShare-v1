using System;
using CarShare.DAL.Models;

namespace CarShare.DAL.Models
{
    public class RentalRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CarPostId { get; set; }

        public CarPost CarPost { get; set; }

        public int RenterId { get; set; }

        public User Renter { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public string LicensePath { get; set; }

        public string ProposalDocumentPath { get; set; }
    }
}
