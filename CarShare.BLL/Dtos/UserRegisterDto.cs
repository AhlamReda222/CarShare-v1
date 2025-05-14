using CarShare.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public UserType UserType { get; set; }
    }

}
