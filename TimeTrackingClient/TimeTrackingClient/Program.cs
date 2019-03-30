using System;
using TimeTrackingClient.Services;

namespace TimeTrackingClient
{
    class Program
    {
        private static SocketService _socketService = new SocketService();

        static void Main(string[] args)
        {
            //_socketService.LoopConnectServerAndSendMessage();
            Console.Read();
        }
    }
}
