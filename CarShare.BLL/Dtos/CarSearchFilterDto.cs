using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{
    public class CarSearchFilterDto
    {
        public string? CarType { get; set; }
        public decimal? MaxPrice { get; set; }
    }

}
