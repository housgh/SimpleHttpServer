namespace SimpleHttpExample.Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class GetRequestAttribute : Attribute
{
    public GetRequestAttribute(string route = "")
    {
        Route = route;
    }

    public string Route { get; set; }
}