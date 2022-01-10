using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Report;

public class Report4Month
{

    [Required]
    [Display(Name = "รหัสอาจารย์")]

    public string Id { get; set; }

    [Required]
    [Display(Name = "ชื่ออาจารย์")]
    public string name { get; set; }

    [Required]
    [Display(Name = "link4reportTeacher")]
    public string link { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "รายได้")]
    public decimal income { get; set; }

    public string color { get; set; }


}
