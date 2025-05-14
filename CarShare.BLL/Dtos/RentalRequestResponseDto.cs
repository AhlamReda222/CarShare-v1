using CarShare.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{
    public  class RentalRequestResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LicensePath { get; set; }
        public string ProposalDocumentPath { get; set; }
        public RequestStatus Status { get; set; }
        public int CarPostId { get; set; }
        public int RenterId { get; set; }
    }
}
