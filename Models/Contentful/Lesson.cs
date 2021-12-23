using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class Lesson
{
    public SystemProperties? Sys { get; set; }

    public string? sku { get; set; }

    public string? name { get; set; }

    public string? description { get; set; }

    public int? price { get; set; }

    public Document? content { get; set; }

    public string? linkyoutube { get; set; }

    public Asset? image { get; set; }

    public Teacher? teacher { get; set; }

    // public List<Course>? course { get; set; }
    
    public IEnumerable<Course>? course { get; set; }

    public IEnumerable<CourseType>? courses { get; set; }


}
