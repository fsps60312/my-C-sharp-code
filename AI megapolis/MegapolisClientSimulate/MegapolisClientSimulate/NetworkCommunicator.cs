using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace MegapolisClientSimulate
{
    static class NetworkCommunicator
    {
        private static bool UseIPv6 = false;
        private static int port { get { return IPEndPoint.MinPort + Hash("Megapolis", IPEndPoint.MaxPort - 1024 + 1); } }
        private static string serverIP { get { return UseIPv6 ? "fe80::70e9:b961:8252:e9e7%11" : "140.112.239.83"; } }
        private static string SendAndReceiveMessage(string msg)
        {
            Socket socket = new Socket(UseIPv6?AddressFamily.InterNetworkV6:AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            status = "Connecting...";
            while (true)
            {
                try
                {
                    socket.Connect(IPAddress.Parse(serverIP), port);
                    break;
                }
                catch (Exception error)
                {
                    var result = MessageBox.Show(error.ToString(), "Error", MessageBoxButtons.RetryCancel);
                    if (result != DialogResult.Retry) return "Error";
                }
            }
            status = $"Sending: {msg}";
            StreamWriter writer = new StreamWriter(new NetworkStream(socket));
            writer.WriteLine(msg);
            writer.Close();
            status = $"Message sent, receiving...";
            StreamReader reader = new StreamReader(new NetworkStream(socket));
            string answer = reader.ReadToEnd();
            reader.Close();
            status = "Received";
            return answer;
        }
        public static void SendMessage(string msg)
        {
            Thread thread = new Thread(() =>
              {
                  string result = SendAndReceiveMessage(msg);
                  log = result;
              });
            thread.IsBackground = true;
            thread.Start();
        }
        public static void Start()
        {
            log = $"MinPort: {IPEndPoint.MinPort}, MaxPort: {IPEndPoint.MaxPort}";
            log = $"IP: {MyIP(UseIPv6? AddressFamily.InterNetworkV6:AddressFamily.InterNetwork)}, Server's IP: {serverIP}, port: {port}";
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
        #region log changed event
        public delegate void LogChangedEventHandler(string log);
        public static LogChangedEventHandler LogChanged;
        private static void OnLogChanged(string log) { LogChanged?.Invoke(/*"Network: " +*/ log); }
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
