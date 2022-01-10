using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Report;

public class Report4TeacherReport
{

    [Required]
    [Display(Name = "รหัสวิชา")]
    public string sku { get; set; }

    [Required]
    [Display(Name = "ชื่อวิชา")]
    public string name { get; set; }

    [Required]
    [Display(Name = "จำนวนสั่งซื้อ")]
    public int quantity { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "รายได้")]
    public decimal income { get; set; }

    public string color { get; set; }

}
