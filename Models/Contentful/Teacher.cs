using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class Teacher
{
    public SystemProperties? Sys { get; set; }

    public string? teacherId { get; set; }

    public string? teacherName { get; set; }

    public string? position { get; set; }

    public Asset? image { get; set; }

}
