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
using System.IO;
using Motivation;

namespace generals.io
{
    public partial class Form1 : Form
    {
        MyTextBox TXB;
        MyButton BTN;
        string msg = "";
        int msgLen = 0;
        public Form1()
        {
            TXB = new MyTextBox(true);
            BTN = new MyButton($"Send");
            this.Controls.Add(BTN);
            BTN.Click += BTN_Click;
            //this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
        }

        private void BTN_Click(object sender, EventArgs e)
        {
            Form1_Shown(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            TXB.AppendText("Connecting...\r\n");
            socket.Connect(GetMyIP(AddressFamily.InterNetworkV6), 8877);
            TXB.AppendText("Connected\r\n");
            StreamWriter writer = new StreamWriter(new NetworkStream(socket));
            for(int i=0;i<msgLen;i++) writer.Write('a');
            writer.Flush();
            socket.Close();
            msgLen += msgLen==0?1:msgLen;
            TXB.AppendText("Closed\r\n");
            socket.Dispose();
            //this.Close();
        }

        IPAddress GetMyIP(AddressFamily addressFamily)
        {
            //return IPAddress.Parse("192.168.43.19").MapToIPv4();
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily == addressFamily)
                {
                    //MessageBox.Show(ipAddress.Address.ToString());
                    return ipAddress;
                }
            }
            throw new Exception("Can't detect your IP address");
        }
    }
}
