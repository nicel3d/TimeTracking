using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTrackingServer.Services;
using TimeTrackingServer.Services.Impl;

namespace TimeTrackingServer.Middlewares
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024 * 512; // 0.5Mb
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class AsyncListenerSocketMiddleware
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public const int port = 8005;
        public IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        private readonly RequestDelegate _next;
        private static StreamingDataService _streamingDataService;
        public static BlockingCollection<StreamingDataRequest> bc = new BlockingCollection<StreamingDataRequest>();


        public AsyncListenerSocketMiddleware(RequestDelegate next, StreamingDataService streamingDataService)
        {
            _next = next;
            _streamingDataService = streamingDataService;

            (new Thread(delegate () {
                Task.Run(() => StartListening());
            })).Start();

            (new Thread(delegate () {
                Task.Run(StartListeningQueues);
            })).Start();
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }

        public static async Task StartListeningQueues()
        {
            while (true)
            {
                await _streamingDataService.AddActivity(bc.Take());
                await Task.Delay(100);
            }
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            //byte[] bytes = new Byte[1024 * 2];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            //IPEndPoint remoteIpEndPoint = handler.RemoteEndPoint as IPEndPoint;
            //Console.WriteLine();
            //Console.WriteLine(remoteIpEndPoint.Address + ":" + remoteIpEndPoint.Port);

            try
            {
                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    builder.Append(Encoding.Unicode.GetString(state.buffer, 0, bytesRead));

                    StreamingDataRequest editActivityStaffRequest = JsonConvert.DeserializeObject<StreamingDataRequest>(builder.ToString());

                    bc.Add(editActivityStaffRequest);

                    //Console.WriteLine(content);
                    //if (content.IndexOf("<EOF>") > -1)
                    //{
                    //    // All the data has been read from the 
                    //    // client. Display it on the console.
                    //    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    //        content.Length, content);
                    //    // Echo the data back to the client.
                    //    Send(handler, content);
                    //}
                    //else
                    //{
                    //    // Not all data received. Get more.
                    //}
                }

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.


            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
