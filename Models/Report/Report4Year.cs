using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Report;

public class Report4Year
{

    [Required]
    [Display(Name = "เดือน")]
    public string name { get; set; }

    [Required]
    public int indexName { get; set; }

    [Required]
    [Display(Name = "link4reportMonth")]
    public string link { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "รายได้")]
    public decimal income { get; set; }

}

public class Report4Years
{
    public int year { get; set; }

    public List<Report4Year> report { get; set; }
    
}
