using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class Website
{
    public SystemProperties? Sys { get; set; }

    public string? name { get; set; }
    
    public IEnumerable<Carousel>? carousels { get; set; }

    public IEnumerable<CourseType>? courseType { get; set; }

    public IEnumerable<Lesson>? popularCourse { get; set; }


}
