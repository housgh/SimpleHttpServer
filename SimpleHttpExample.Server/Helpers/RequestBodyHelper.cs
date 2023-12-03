using System.Reflection;
using System.Text.Json;

namespace SimpleHttpExample.Server.Helpers;

public static class RequestBodyHelper
{
    public static object? ParseBody(string body, MethodInfo methodInfo)
    {
        var bodyParameter = methodInfo.GetParameters()[^1];
        return JsonSerializer.Deserialize(body, bodyParameter.ParameterType);
    }
}