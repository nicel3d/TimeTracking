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
using System.Collections.Concurrent;

namespace TimeTrackingClient.Services
{
    class SocketService
    {
        private const string _address = "127.0.0.1";
        private const int _port = 8005;
        private static IPEndPoint _ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
        private static IdleTimeFinderService _idleTimeFinder = new IdleTimeFinderService();
        private static DataBaseService _dataBaseService = new DataBaseService();
        private static StreamingData _streamingData;
        private static BlockingCollection<StreamingData> bc;
        private static Socket _socket;

        private static TimeSpan _waitingBeforeShipping = TimeSpan.FromSeconds(5);
        private static TimeSpan _waitingBeforeReconnect = TimeSpan.FromSeconds(1);
        private static TimeSpan _waitingBeforeGetTemporaryStorage = TimeSpan.FromSeconds(10);
        private static TimeSpan _waitingBeforeDeleteTemporaryStorage = TimeSpan.FromMilliseconds(100);

        public SocketService()
        {
            (new Thread(delegate ()
            {
                Task.Run(StartListeningTemporaryStorageForWrite);
            })).Start();


            (new Thread(delegate ()
            {
                Task.Run(StartListeningGetTemporaryStorage);
            })).Start();
        }


        private Task StartListeningGetTemporaryStorage()
        {
            while (true)
            {
                List<StreamingData> list = _dataBaseService.GetTemporaryStorageList();

                if (list.Count > 0)
                {
                    bc = new BlockingCollection<StreamingData>();

                    foreach (StreamingData item in list)
                    {
                        bc.Add(item);
                    }

                    while (bc.Count > 1)
                    {
                        try
                        {
                            if (_socket != null && _socket.Connected)
                            {
                                var activityStaffToStreamingData = bc.Take();
                                _socket.Send(Encoding.Unicode.GetBytes(new JavaScriptSerializer().Serialize(activityStaffToStreamingData)));
                                _dataBaseService.DeleteTemporaryStorageByActivityTime(activityStaffToStreamingData.ActivityTime);
                                Thread.Sleep(_waitingBeforeDeleteTemporaryStorage);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                Thread.Sleep(_waitingBeforeGetTemporaryStorage);
            }
        }

        private Task StartListeningTemporaryStorageForWrite()
        {
            int expect = 0;

            while (true)
            {
                if (_socket == null || !_socket.Connected)
                {
                    expect++;
                    Thread.Sleep(1);

                    if (expect >= _waitingBeforeShipping.TotalMilliseconds)
                    {
                        _dataBaseService.AddStreamingDataToTemporaryStorage(GetActivityStaffToStreamingData());
                        expect = 0;
                    }
                }
                else
                {
                    expect = 0;
                }
            }
        }

        public void LoopConnectServer()
        {
            int attempts = 0;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (!_socket.Connected)
            {
                if (!_idleTimeFinder.GetActiveUser())
                {
                    continue;
                }

                try
                {
                    attempts++;
                    _socket.Connect(_ipPoint);
                }
                catch (SocketException)
                {
                    //Console.Clear();
                    //Console.WriteLine("Connection attempts: {0}", attempts.ToString());
                    Thread.Sleep(_waitingBeforeReconnect);
                }
            }
            //Console.WriteLine("Connected!");
        }

        private static StreamingData GetActivityStaffToStreamingData()
        {
            ApplicationStreamingData applicationStreamingData;

            applicationStreamingData = new WinApiService().GetActiveWindow();
            return _streamingData = new StreamingData()
            {
                ApplicationAlias = applicationStreamingData.ApplicationAlias,
                ApplicationTitle = applicationStreamingData.ApplicationTitle,
                ApplicationImage = applicationStreamingData.ApplicationImage,
                StaffAlias = Environment.UserName,
                ActivityTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
            };
        }

        public void LoopConnectServerAndSendMessage()
        {
            LoopConnectServer();

            try
            {
                do
                {
                    if (!_idleTimeFinder.GetActiveUser())
                    {
                        continue;
                    }

                    _socket.Send(Encoding.Unicode.GetBytes(new JavaScriptSerializer().Serialize(GetActivityStaffToStreamingData())));
                    Thread.Sleep(_waitingBeforeShipping);
                } while (true);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                LoopConnectServerAndSendMessage();
            }
        }
    }
}
