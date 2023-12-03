namespace SimpleHttpExample.Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PostRequestAttribute : Attribute
{
    public PostRequestAttribute(string route = "")
    {
        Route = route;
    }

    public string Route { get; set; }
}