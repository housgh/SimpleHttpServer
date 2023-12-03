using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using SimpleHttpExample.Server.Exceptions;
using SimpleHttpExample.Server.Helpers;
using SimpleHttpExample.Server.Models;
using HttpMethod = SimpleHttpExample.Server.Helpers.HttpMethod;

namespace SimpleHttpExample.Server.Handlers;

public static class BaseRequestHandler
{
    public static async Task HandleRequestAsync(SimpleHttpRequest request, NetworkStream stream)
    {
        var parsedRequest = RouteHelper.ParseRequest(request.Route, request.RequestMethod);
        var methodInfo = request.RequestMethod switch
        {
            HttpMethod.Post => HttpRoutes.PostRoutes[parsedRequest.RouteKey],
            HttpMethod.Put => HttpRoutes.PutRoutes[parsedRequest.RouteKey],
            HttpMethod.Delete => HttpRoutes.DeleteRoutes[parsedRequest.RouteKey],
            HttpMethod.Get => HttpRoutes.GetRoutes[parsedRequest.RouteKey],
            _ => throw new InvalidMethodException()
        };
        var parameters = ParameterHelper.ParseParameters(parsedRequest.Parameters, methodInfo);
        var allParameters = new List<object?>();
        allParameters.AddRange(parameters);

        var controllerType = methodInfo.DeclaringType;
        var controller = (BaseController)Activator.CreateInstance(controllerType!)!;

        controller.Request = request;

        if (request.RequestMethod != HttpMethod.Get && request.StringContent is not null)
        {
            var body = RequestBodyHelper.ParseBody(request.StringContent, methodInfo);
            allParameters.Add(body);

        }
        while (methodInfo.GetParameters().Length > allParameters.Count)
        {
            allParameters.Add(null);
        }
        var result = methodInfo.Invoke(controller, allParameters.ToArray());
        var response = new HttpResponse(content: result);
        await ResponseHandler.SendResponse(response, stream);
    }
}