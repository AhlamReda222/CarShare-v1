using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.DAL.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public int CarPostId { get; set; }
        public CarPost CarPost { get; set; }

        public int RenterId { get; set; }
        public User Renter { get; set; }
    }

}
