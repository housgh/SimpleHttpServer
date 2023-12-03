namespace SimpleHttpExample.Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PutRequestAttribute : Attribute
{
    public PutRequestAttribute(string route = "")
    {
        Route = route;
    }

    public string Route { get; set; }
}