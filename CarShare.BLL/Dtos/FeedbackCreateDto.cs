using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Dtos
{
    public class FeedbackCreateDto
    {
        public int CarPostId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
