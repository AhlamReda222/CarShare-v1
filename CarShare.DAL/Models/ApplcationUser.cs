using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.DAL.Models
{
    public class ApplcationUser : IdentityUser
    {
         public String Username { get; set; }
    }
}
