using System.ServiceProcess;

namespace TimeTrackingClient
{
    class Program
    {

        static void Main(string[] args)
        {

#if DEBUG
            Service service = new Service();
            service.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
