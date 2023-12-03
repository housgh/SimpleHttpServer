using System.Net.Sockets;
using System.Text;
using SimpleHttpExample.Server.Helpers;

namespace SimpleHttpExample.Server.Handlers;

public static class HttpRequestHandler
{
    public static async Task HandleHttpRequestAsync(TcpClient tcpClient)
    {
        NetworkStream? stream = null;
        try
        {
            stream = tcpClient.GetStream();
            var buffer = new byte[1_024];
            _ = await stream.ReadAsync(buffer, 0, buffer.Length);

            var request = Encoding.UTF8.GetString(buffer);
            var httpRequest = RequestHelper.Parse(request);

            Console.WriteLine($"{httpRequest.RequestMethod} /{httpRequest.Route}");

            await BaseRequestHandler.HandleRequestAsync(httpRequest, stream);

        }
        catch (Exception ex)
        {
            if (stream is null) throw;
            await ExceptionHandler.HandleExceptionAsync(ex, stream);
        }
        finally
        {
            if (stream is not null)
            {
                await stream.FlushAsync();
                stream.Close();
                await stream.DisposeAsync();
            }
        }
    }
}