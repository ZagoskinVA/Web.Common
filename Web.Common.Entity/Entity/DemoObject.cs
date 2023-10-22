using WebUtilities.Model;

namespace Web.Common.Entity.Entity;

public class DemoObject: BaseObject
{
    public string Title { get; set; }

    public double Value { get; set; }
    
    public string? UserId { get; set; }
}