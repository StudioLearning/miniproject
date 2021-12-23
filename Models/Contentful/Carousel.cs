using Contentful.Core.Models;

namespace miniproject.Models.Contentful;

public class Carousel
{
    public SystemProperties? Sys { get; set; }

    public string? name { get; set; }

    public Document? captions { get; set; }

}
