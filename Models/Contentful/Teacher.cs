using Contentful.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniproject.Models.Contentful;

public class Teacher
{
    public SystemProperties? Sys { get; set; }

    [Display(Name = "รหัสผู้สอน")]
    public string? teacherId { get; set; }

    [Display(Name = "ชื่อผู้สอน")]
    public string? teacherName { get; set; }

    [Display(Name = "ตำแหน่ง")]
    public string? position { get; set; }

    [Display(Name = "รูปผู้สอน")]
    public Asset? image { get; set; }

}
