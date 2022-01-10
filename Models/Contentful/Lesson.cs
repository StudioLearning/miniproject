using Contentful.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Contentful;

public class Lesson
{
    public SystemProperties? Sys { get; set; }

    [Display(Name = "รหัสคอร์ส")]
    public string? sku { get; set; }

    [Display(Name = "ชื่อคอร์ส")]
    public string? name { get; set; }

    [Display(Name = "บทนำ")]
    public string? description { get; set; }

    [Display(Name = "ราคา")]
    public int price { get; set; }


    public Document? content { get; set; }

    public string? linkyoutube { get; set; }

    public Asset? image { get; set; }

    [Display(Name = "ผู้สอน")]
    public Teacher teacher { get; set; }

    // public List<Course>? course { get; set; }
    
    public IEnumerable<Course>? course { get; set; }

    public IEnumerable<CourseType>? courses { get; set; }


}
