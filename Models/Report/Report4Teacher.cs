using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Report;

public class Report4Teacher
{
    [Required]
    public IEnumerable<Report4TeacherReport> report { get; set; }

    [Required]
    public miniproject.Models.Contentful.Teacher teacher { get; set; }

    [Required]
    [Display(Name = "รายรับทั้งหมด")]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    public decimal revenueBeforeTax { get; set; }

    [Required]
    [Display(Name = "ภาษี 7%")]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    public decimal tax { get; set; }

    [Required]
    [Display(Name = "รายรับหลังหักภาษี")]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    public decimal revenue { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "รายได้")]
    public decimal incomeTotal { get; set; }

}
