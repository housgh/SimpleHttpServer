using SimpleHttpExample.Server.Handlers;
using SimpleHttpExample.Server.Helpers;
using System.Net.Sockets;
using System.Net;

namespace SimpleHttpExample.Server;

public static class Runner
{
    public static void RunServer(IPAddress host, int port)
    {
        Console.WriteLine($"Listening to port {port}");
        ControllerHelper.ReadControllers();

        var listener = new TcpListener(host, port);
        listener.Start(10);

        while (true)
        {
            var tcpClient = listener.AcceptTcpClient();
            #pragma warning disable CS4014
            HttpRequestHandler.HandleHttpRequestAsync(tcpClient);
            #pragma warning restore CS4014
        }
    }
}