using System.Net.Sockets;
using System.Text;
using SimpleHttpExample.Server.Models;

namespace SimpleHttpExample.Server.Handlers;

public static class ResponseHandler
{
    public static async Task SendResponse(HttpResponse response, NetworkStream stream)
    {
        Console.WriteLine($"Response: {response.StatusCode}");
        var bytes = Encoding.UTF8.GetBytes(response.ToString());
        await stream.WriteAsync(bytes);
    }
}