using System.ComponentModel.DataAnnotations;

namespace miniproject.Models
{
    public class Order
    {
        [Display(Name = "รหัสใบสั่งซื้อ")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "รหัสคอร์ส")]
        public string Sku { get; set; }

        [Display(Name = "วันที่สั่งซื้อ")]
        [DataType(DataType.DateTime)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "ราคา")]
        public decimal Price { get; set; }
    }
}