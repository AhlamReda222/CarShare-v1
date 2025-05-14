using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShare.DAL.Models;
namespace CarShare.BLL.Dtos
{
    public class UserLoginDto
    {
    
            public string Email { get; set; }
            public string Password { get; set; }
    }

}


