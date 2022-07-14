using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarSalesPortal.Extensions;
using static CarSalesPortal.Extensions.AnnotationExtensions;

namespace CarSalesPortal.Models
{
    public class Car
    {
        public int Id { get; set; }

        public Make Make { get; set; }
        [Required]
        [RegularExpression("^[1-9]*$", ErrorMessage ="Select Make")]
        public int MakeID { get; set; }


        public Model Model { get; set; }
        [Required]
        [RegularExpression("^[1-9]*$", ErrorMessage = "Select Model")]
        public int ModelID { get; set; }

        [Required(ErrorMessage ="Provide Year")]
        [YearRangeTillDate(1990, ErrorMessage ="Invalid Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Provide Colour")]
        public string Colour { get; set; }

        [Required(ErrorMessage = "Provide New or Secondhand")]
        public string N_S { get; set; }
        
        public string Extra_Features { get; set; }

        [Required(ErrorMessage = "Provide Price")]
        public int Price { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Select Currency")]
        public string Currency { get; set; }

        public string ImagePath { get; set; }
    }

}
