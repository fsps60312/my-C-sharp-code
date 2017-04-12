using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Megapolis
{
    static class NetworkCommunicator
    {
        private static bool UseIPv6 = false;
        private static int port { get { return IPEndPoint.MinPort + Hash("Megapolis", IPEndPoint.MaxPort - 1024 + 1); } }
        //private static TcpListener socket;
        private static Socket socket;
        public static void Start()
        {
            try
            {
                //status = "Network Function not Implemented!";return;
                log = "Staring...";
                socket = new Socket(UseIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //socket = new TcpListener(MyIP(AddressFamily.InterNetworkV6), port);
                log = $"MinPort: {IPEndPoint.MinPort}, MaxPort: {IPEndPoint.MaxPort}";
                log = $"IP: {MyIP(UseIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork)}, port: {port}";
                socket.Bind(new IPEndPoint(UseIPv6 ? IPAddress.IPv6Any : IPAddress.Any, port));
                log = $"Listening...";
                socket.Listen(int.MaxValue);
                //socket.Start();
                Thread thread = new Thread(() =>
                  {
                      while (true)
                      {
                          Socket client = socket.Accept();
                      //Socket client = socket.AcceptSocket();
                      status = $"Receiving from {client.RemoteEndPoint as IPEndPoint}...";
                          StreamReader reader = new StreamReader(new NetworkStream(client));
                          string msg = reader.ReadLine();
                          reader.Close();
                          status = $"Message received";
                          string reply;
                          MessageReceived(msg, out reply);
                          status = $"Sending message with length {reply.Length}" + (reply.Length <= 500 ? ": " + reply : "...");
                          StreamWriter writer = new StreamWriter(new NetworkStream(client));
                          writer.Write(reply);
                          writer.Close();
                          client.Disconnect(false);
                          status = $"Message sent to {client.RemoteEndPoint as IPEndPoint}";
                      }
                  });
                thread.IsBackground = true;
                thread.Start();
            }
            catch(Exception error)
            {
                log = $"{error}";
                log = $"Network function will be unavailable";
            }
        }
        private static IPAddress MyIP(AddressFamily addressFamily)
        {
            foreach(IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily == addressFamily) return ipAddress;
            }
            throw new Exception($"Can't get my IP address of type {addressFamily}");
        }
        private static int Hash(string s, int mod)
        {
            long seed = 20170216;
            seed %= mod;
            foreach (char c in s) seed = (seed * 0xdefaced + (int)c) % mod;
            seed += seed >> 20;
            seed %= mod;
            return (int)seed;
        }
        #region message received event
        public delegate void MessageReceivedEventHandler(string msg, out string reply);
        public static MessageReceivedEventHandler MessageReceived;
        private static void OnMessageReceived(string msg,out string reply) { reply = null; MessageReceived?.Invoke(msg, out reply); }
        #endregion
        #region log changed event
        public delegate void LogChangedEventHandler(string log);
        public static LogChangedEventHandler LogChanged;
        private static void OnLogChanged(string log) { LogChanged?.Invoke("Network: " + log); }
        private static string log { set { OnLogChanged(value); } }
        #endregion
        #region status changed event
        public delegate void StatusChangedEventHandler(string status);
        public static StatusChangedEventHandler StatusChanged;
        private static void OnStatusChanged(string status) { StatusChanged?.Invoke("Network: " + status); }
        private static string status{ set { OnStatusChanged(value); } }
        #endregion
    }
}
