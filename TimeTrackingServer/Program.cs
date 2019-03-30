using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TimeTrackingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new AsynchronousSocketListener();
            //Task.Run(() => new AsynchronousSocketListener());

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
