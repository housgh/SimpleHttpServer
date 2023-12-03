using System.Reflection;
using SimpleHttpExample.Server.Attributes;

namespace SimpleHttpExample.Server.Helpers;

public static class ControllerHelper
{
    public static void ReadControllers()
    {
        Console.WriteLine("Initializing Controllers...");
        var controllers = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(x => x.GetCustomAttributes()
                .Select(y => y.GetType()).Contains(typeof(ControllerAttribute)));

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods();

            foreach (var methodInfo in methods
                         .Where(x => x.GetCustomAttributes()
                             .Select(y => y.GetType()).Contains(typeof(GetRequestAttribute))))
            {
                var attribute = methodInfo.GetCustomAttributes()
                    .First(x => x.GetType() == typeof(GetRequestAttribute));
                var route = ((GetRequestAttribute)attribute).Route;
                HttpRoutes.GetRoutes.Add(route, methodInfo);
            }

            foreach (var methodInfo in methods
                         .Where(x => x.GetCustomAttributes()
                             .Select(y => y.GetType()).Contains(typeof(PostRequestAttribute))))
            {
                var attribute = methodInfo.GetCustomAttributes()
                    .FirstOrDefault(x => x.GetType() == typeof(PostRequestAttribute));
                if (attribute is null) continue;
                var route = ((PostRequestAttribute)attribute).Route;
                HttpRoutes.PostRoutes.Add(route, methodInfo);
            }

            foreach (var methodInfo in methods
                         .Where(x => x.GetCustomAttributes()
                             .Select(y => y.GetType()).Contains(typeof(PutRequestAttribute))))
            {
                var attribute = methodInfo.GetCustomAttributes()
                    .FirstOrDefault(x => x.GetType() == typeof(PutRequestAttribute));
                if (attribute is null) continue;
                var route = ((PutRequestAttribute)attribute).Route;
                HttpRoutes.PutRoutes.Add(route, methodInfo);
            }

            foreach (var methodInfo in methods
                         .Where(x => x.GetCustomAttributes()
                             .Select(y => y.GetType()).Contains(typeof(DeleteRequestAttribute))))
            {
                var attribute = methodInfo.GetCustomAttributes()
                    .FirstOrDefault(x => x.GetType() == typeof(DeleteRequestAttribute));
                if (attribute is null) continue;
                var route = ((DeleteRequestAttribute)attribute).Route;
                HttpRoutes.DeleteRoutes.Add(route, methodInfo);
            }
            Console.WriteLine($"Initialized Controller: {controller.Name}");
        }
    }
}