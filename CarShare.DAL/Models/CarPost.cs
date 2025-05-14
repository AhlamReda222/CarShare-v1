using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.DAL.Models
{
    public class CarPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CarType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public TransmissionType Transmission { get; set; }
        public string Location { get; set; }
        public RentalStatus RentalStatus { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public decimal RentalPrice { get; set; }
        public bool IsApproved { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<RentalRequest> RentalRequests { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }

}