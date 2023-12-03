using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleHttpExample.Server.Exceptions;
using SimpleHttpExample.Server.Models;

namespace SimpleHttpExample.Server.Handlers;

public static class ExceptionHandler
{
    public static async Task HandleExceptionAsync(Exception ex, NetworkStream stream)
    {
        HttpResponse response;
        switch (ex)
        {
            case InvalidRouteException:
                response = new HttpResponse(HttpStatusCode.NotFound);
                break;
            case InvalidMethodException:
                response = new HttpResponse(HttpStatusCode.MethodNotAllowed);
                break;
            default:
                response = new HttpResponse(HttpStatusCode.InternalServerError, ex.ToString());
                break;
        }
        var bytes = Encoding.UTF8.GetBytes(response.ToString());
        await stream.WriteAsync(bytes);
    }
}