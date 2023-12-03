using SimpleHttpExample.Server.Helpers;
using SimpleHttpExample.Server.Models;
using System.Net.Sockets;
using HttpMethod = SimpleHttpExample.Server.Helpers.HttpMethod;

namespace SimpleHttpExample.Server.Handlers;

public static class GetRequestHandler
{
    public static async Task HandleGetRequestAsync(SimpleHttpRequest request, NetworkStream stream)
    {
        var parsedRequest = RouteHelper.ParseRequest(request.Route, HttpMethod.Get);
        var methodInfo = HttpRoutes.GetRoutes[parsedRequest.RouteKey];
        var parameters = ParameterHelper.ParseParameters(parsedRequest.Parameters, methodInfo);

        var controllerType = methodInfo.DeclaringType;
        var controller = (BaseController) Activator.CreateInstance(controllerType!)!;

        controller.Request = request;

        var result = methodInfo.Invoke(controller, parameters);
        var response = new HttpResponse(content: result);

        await ResponseHandler.SendResponse(response, stream);
    }
}