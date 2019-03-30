using System;
using System.Threading.Tasks;

namespace TimeTrackingServer.Services
{
    public interface IAsyncListenerSocketService
    {
        void StartListening();
        void AcceptCallback(IAsyncResult ar);
        void ReadCallback(IAsyncResult ar);
    }
}
