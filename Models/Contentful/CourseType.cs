using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class CourseType
{
    public SystemProperties? Sys { get; set; }

    public string? name { get; set; }

    public string? caption { get; set; }

    public string? fontIcon { get; set; }

    public string? fontIconTag { get; set; }


}
