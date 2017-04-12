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
using System.IO;

namespace Server
{
    public partial class Form1 : Form
    {
        MyTextBox TXB;
        public Form1()
        {
            this.Size = new Size(750, 500);
            TXB = new MyTextBox(true);
            this.Controls.Add(TXB);
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.IPv6Any, 8877));
            TXB.AppendText("Listening...\r\n");
            socket.Listen(int.MaxValue);
            while (true)
            {
                Socket client = socket.Accept();
                TXB.AppendText($"Receiving from {client.RemoteEndPoint as IPEndPoint}...\r\n");
                StreamReader reader = new StreamReader(new NetworkStream(client));
                List<byte> msg = new List<byte>();
                byte[] buffer = new byte[1025];
                DateTime startTime = DateTime.Now;
                while (client.Receive(buffer, 1024, SocketFlags.None) > 0)
                {
                    for (int i = 0; buffer[i] != '\0'; i++) msg.Add(buffer[i]);
                }
                TXB.AppendText($"time used: {(DateTime.Now-startTime).TotalMilliseconds} ms, received size={msg.Count}\r\n"/*Encoding.UTF8.GetString(msg.ToArray()) + "\r\n"*/);
            }
            //socket.Close();
            //socket.Dispose();
            //TXB.AppendText("Disposed\r\n");
        }
    }
}
