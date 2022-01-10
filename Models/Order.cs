using System;
// using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "รหัสใบสั่งซื้อ")]
        [DisplayFormat(DataFormatString = "{0:000000}")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "รหัสคอร์ส")]
        public string Sku { get; set; }

        [Required]
        [Display(Name = "รหัสคอร์ส4ContentfulID")]
        public string ContentfulID { get; set; }
        
        [Required]
        [Display(Name = "รหัสผู้ใช้")]
        public string UserId { get; set; }

        [Display(Name = "วันที่สั่งซื้อ")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "ราคา")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "สถานะการชำระเงิน")]
        public Orders PaymentState { get; set; }

        [Display(Name = "การชำระเงิน")]
        public string Payment {
            get { return PaymentState.GetType().GetMember(PaymentState.ToString()).FirstOrDefault().GetCustomAttribute<DisplayAttribute>().GetName() ;}
        }

        [Display(Name = "วันที่ชำระเงิน")]
        [DataType(DataType.DateTime)]
        public DateTime PaidDate { get; set; }
    }
}