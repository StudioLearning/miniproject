using System;
// using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using miniproject.Models.Contentful;

namespace miniproject.Models
{
    public class Cart
    {
        public Lesson lesson { get; set; }  

        [Display(Name = "ราคา")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal price { get; set; }

        [Display(Name = "ภาษี 7%")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal tax { get; set; }

        [Display(Name = "รวม")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal totalPrice { get; set; }

        [Display(Name = "รูปปลากรอบ")]
        public string img { get; set; }
    }
}