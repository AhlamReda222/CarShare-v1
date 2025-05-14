using CarShare.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace CarShare.BLL.Dtos
{
    public class CarPostCreateDto
    {
      
            [Required]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string CarType { get; set; }

            [Required]
            public string Brand { get; set; }

            [Required]
            public string Model { get; set; }

            [Required]
            public TransmissionType Transmission { get; set; }

            [Required]
            public string Location { get; set; }

            [Required]
            public DateTime AvailableFrom { get; set; }

            [Required]
            public DateTime AvailableTo { get; set; }

            [Required]
            [Range(1, 10000)]
            public decimal RentalPrice { get; set; }

            [Required]
            public IFormFile CarImage { get; set; }
        
    }
}