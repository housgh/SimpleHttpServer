namespace SimpleHttpExample.Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DeleteRequestAttribute : Attribute
{
    public DeleteRequestAttribute(string route = "")
    {
        Route = route;
    }

    public string Route { get; set; }
}