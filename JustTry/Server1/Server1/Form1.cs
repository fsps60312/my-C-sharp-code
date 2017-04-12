using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Motivation;
using System.Threading;
using System.Diagnostics;

namespace Server1
{
    public partial class Form1 : Form
    {
        private void Form1_Shown(object sender, EventArgs e)
        {
            AppendMessage("Your IP is: " + GetMyIpAddress().ToString());
            AppendMessage("Starting server...");
            InitializeSocket();
            AppendMessage("Listening...");
            StartListening();
        }
        void StartListening()
        {
            int msg_count = 0;
            new Thread(() =>
            {
                while (true)
                {
                    Socket client = socket.Accept();
                    AppendMessage(string.Format("Message #{0}:", ++msg_count));
                    IPEndPoint clientip = client.RemoteEndPoint as IPEndPoint;
                    AppendMessage("Client's ip is: " + clientip);
                    new Thread(() =>
                    {
                        while (true)
                        {
                            byte[] data = new byte[1024];
                            int length = client.Receive(data);
                            if (length == 0) break;
                            AppendMessage(Encoding.UTF8.GetString(data, 0, length));
                        }
                        client.Close();
                    }).Start();
                }
            }).Start();
        }
        void InitializeSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 5));
            socket.Listen(10);
        }
        Socket socket;
        IPAddress GetMyIpAddress()
        {
            foreach(IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if(ipAddress.AddressFamily==AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            throw new Exception("Can't detect your IP address");
        }
        void AppendMessage(string msg)
        {
            this.Invoke(new Action(() => { this.Text = msg; }));
            txb.Invoke(new Action(()=> { txb.AppendText(msg + "\r\n"); }));
        }
        MyTextBox txb;
        public Form1()
        {
            this.MaximizeBox = false;
            this.MinimumSize = this.MaximumSize = new Size(640, 360);
            this.Controls.Add(txb = new MyTextBox(true));
            this.Shown += Form1_Shown;
            this.FormClosed += Form1_FormClosed;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
