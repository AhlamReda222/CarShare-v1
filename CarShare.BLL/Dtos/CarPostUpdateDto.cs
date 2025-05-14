using CarShare.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{
    public class CarPostUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CarType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public TransmissionType Transmission { get; set; }
        public string Location { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public decimal RentalPrice { get; set; }

        public IFormFile CarImage { get; set; }  // صورة جديدة
    }

}


