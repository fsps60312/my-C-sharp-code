using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motivation;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;


namespace Client2
{
    public partial class Form1 : Form
    {
        const int port = 5;
        void SendMessage(string msg,int portNumber)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(GetMyIpAddress(), portNumber));
            byte[] data = Encoding.UTF8.GetBytes(msg);
            socket.Send(data);
            socket.Shutdown(SocketShutdown.Send);
            socket.Close();
        }
        IPAddress GetMyIpAddress()
        {
            //return IPAddress.Parse("192.168.43.19").MapToIPv4();
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    //MessageBox.Show(ipAddress.Address.ToString());
                    return ipAddress;
                }
            }
            throw new Exception("Can't detect your IP address");
        }
        private void Txb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                this.Text = "Sending messages...";
                SendMessage(txb.Text, port);
                this.Text = "Message sent!";
                txb.Clear();
            }
        }
        MyTextBox txb;
        public Form1()
        {
            this.Controls.Add(new Func<MyTextBox>(() =>
            {
                txb = new MyTextBox(false);
                txb.Multiline = false;
                txb.KeyDown += Txb_KeyDown;
                return txb;
            })());
        }
    }
}
