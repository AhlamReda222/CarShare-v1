using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.DAL.Models
{
    public class User
    {
            public int Id { get; set; }
            public string Username { get; set; }
          public string Email { get; set; }
           public string PasswordHash { get; set; }
            public UserType UserType { get; set; }
            public bool IsApproved { get; set; }

            public ICollection<CarPost> CarPosts { get; set; }
            public ICollection<RentalRequest> RentalRequests { get; set; }
            public ICollection<Feedback> Feedbacks { get; set; }
        }

    }


