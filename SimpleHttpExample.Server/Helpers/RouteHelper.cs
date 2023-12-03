using System.Reflection;
using SimpleHttpExample.Server.Exceptions;

namespace SimpleHttpExample.Server.Helpers;

public static class HttpRoutes
{
    public static readonly Dictionary<string, MethodInfo> GetRoutes = new();
    public static readonly Dictionary<string, MethodInfo> PostRoutes = new();
    public static readonly Dictionary<string, MethodInfo> PutRoutes = new();
    public static readonly Dictionary<string, MethodInfo> DeleteRoutes = new();
}

public enum HttpMethod
{
    Get,
    Post,
    Put,
    Delete
}

public static class RouteHelper
{
    public static (string RouteKey, Dictionary<string, string> Parameters) ParseRequest(string currentRoute, HttpMethod method)
    {
        var routes = method switch
        {
            HttpMethod.Get => HttpRoutes.GetRoutes.Keys,
            HttpMethod.Post => HttpRoutes.PostRoutes.Keys,
            HttpMethod.Put => HttpRoutes.PutRoutes.Keys,
            _ => HttpRoutes.DeleteRoutes.Keys,
        };
        foreach (var route in routes)
        {
            var parameters = new Dictionary<string, string>();
            var routeValues = route.Split('/');
            var currentRouteValues = currentRoute.Split('/');

            if (routeValues.Length != currentRouteValues.Length) continue;

            var rootRouteValues = routeValues.Where(x => !x.StartsWith('{') && !x.EndsWith('}'));
            if (!rootRouteValues.All(currentRoute.Contains)) continue;

            var parameterValues = routeValues.Where(x => x.StartsWith('{') && x.EndsWith('}')).ToList();
            foreach (var parameterValue in parameterValues)
            {
                var index = routeValues.ToList().IndexOf(parameterValue);
                var name = parameterValue[1..^1];
                var value = currentRouteValues.ElementAt(index);
                parameters.Add(name, value);
            }
            return (route, parameters);
        }

        throw new InvalidRouteException();
    }
}