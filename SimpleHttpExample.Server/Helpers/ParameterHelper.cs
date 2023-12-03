using System.Reflection;

namespace SimpleHttpExample.Server.Helpers;

public static class ParameterHelper
{
    public static object[] ParseParameters(Dictionary<string, string> parameters, MethodInfo methodInfo)
    {
        var methodParameters = methodInfo.GetParameters();
        var parsedParameters = new List<object>();
        foreach (var methodParameter in methodParameters)
        {
            if(methodParameter.Name is null) continue;
            var exists = parameters.TryGetValue(methodParameter.Name, out var parameter);
            if(!exists || parameter is null) continue;
            parsedParameters.Add(Convert.ChangeType(parameter, methodParameter.ParameterType));
        }
        return parsedParameters.ToArray();
    }
}