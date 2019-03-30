using TimeTrackingClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TimeTrackingClient.Services
{
    class SocketService
    {
        private static string _address = "127.0.0.1";
        private static int _port = 8005;
        private static IPEndPoint _ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
        private static IdleTimeFinderService _idleTimeFinder = new IdleTimeFinderService();
        private static WinApiService _window = new WinApiService();

        private static ApplicationStreamingData _applicationStreamingData;
        private static StreamingData _streamingData;
        private static byte[] _streamingDataBytes;

        public SocketService()
        {
        }

        public Socket LoopConnectServer()
        {
            int attempts = 0;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IdleTimeFinderService idleTimeFinder = new IdleTimeFinderService();

            while (!socket.Connected)
            {
                if (!idleTimeFinder.GetActiveUser())
                {
                    continue;
                }

                try
                {
                    attempts++;
                    socket.Connect(_ipPoint);
                    Thread.Sleep(1500);
                }
                catch (SocketException)
                {
                    //Console.Clear();
                    Console.WriteLine("Connection attempts: {0}", attempts.ToString());
                }
            }
            Console.WriteLine("Connected!");

            return socket;
        }

        public void LoopConnectServerAndSendMessage()
        {
            Socket _socket = LoopConnectServer();
            int expect = 0;

            try
            {
                do
                {
                    expect++;
                    Console.WriteLine(expect);
                    Thread.Sleep(1);

                    if (expect == 2000)
                    {
                        expect = 0;
                    }

                    if (!_idleTimeFinder.GetActiveUser())
                    {
                        continue;
                    }

                    _applicationStreamingData = _window.GetActiveWindow();
                    _streamingData = new StreamingData()
                    {
                        ApplicationAlias = _applicationStreamingData.ApplicationAlias,
                        ApplicationTitle = _applicationStreamingData.ApplicationTitle,
                        ApplicationImage = _applicationStreamingData.ApplicationImage,
                        StaffAlias = Environment.UserName,
                        ActivityTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                    };
                    _streamingDataBytes = Encoding.Unicode.GetBytes(new JavaScriptSerializer().Serialize(_streamingData));

                    _socket.Send(_streamingDataBytes);
                    Thread.Sleep(2000);
                    expect = 0;
                } while (true);

                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LoopConnectServerAndSendMessage();
            }
        }
    }
}
