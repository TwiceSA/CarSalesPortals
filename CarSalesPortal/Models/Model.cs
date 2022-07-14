using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarSalesPortal.Models
{
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(225)]

        public string Name { get; set; }

        public Make Make { get; set; }

        public int MakeID { get; set; }
    }
}
