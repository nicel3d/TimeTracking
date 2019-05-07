using System;
using System.ServiceProcess;
using System.Threading;
using TimeTrackingClient.Services;

namespace TimeTrackingClient
{
    partial class Service : ServiceBase
    {
        public SocketService _socketService = new SocketService();

        public Service()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Thread connectSoket = new Thread(_socketService.LoopConnectServerAndSendMessage);
            //Thread connectSoket = new Thread(_socketService.LoopConnectServerAndSendMessage);
            //Thread listeningStore = new Thread(_socketService.StartListeningTemporaryStorageForWrite);
            //Thread listeningGet = new Thread(_socketService.StartListeningGetTemporaryStorage);
            connectSoket.Start();
            //listeningStore.Start();
            //listeningGet.Start();
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
