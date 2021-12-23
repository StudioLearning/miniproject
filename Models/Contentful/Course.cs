using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class Course
{
    public SystemProperties? Sys { get; set; }

    public string? courseName { get; set; }

    public Asset? coursevideo { get; set; }

    public Document? coursecontent { get; set; }

}
