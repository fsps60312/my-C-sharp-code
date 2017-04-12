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

namespace Get_my_IP_address
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.MaximizeBox = false;
            this.MinimumSize = this.MaximumSize = new Size(480, 270);
            this.Controls.Add(new Func<MyTextBox>(() =>
            {
                MyTextBox txb = new MyTextBox(true);
                foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        txb.AppendText("Your IP address is: " + ipAddress.ToString() + "\r\n");
                    }
                }
                return txb;
            })());
        }
    }
}
