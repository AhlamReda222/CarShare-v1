using CarShare.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{

    public class RentalRequestDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarPostId { get; set; }
        public int RenterId { get; set; }
        public IFormFile License { get; set; }
        public IFormFile ProposalDocument { get; set; }


    }
}