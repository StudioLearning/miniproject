using System.ComponentModel.DataAnnotations;

namespace miniproject.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "รหัสใบสั่งซื้อ")]
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
        [Display(Name = "ราคา")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "สถานะการชำระเงิน")]
        public Orders PaymentState { get; set; }
    }
}