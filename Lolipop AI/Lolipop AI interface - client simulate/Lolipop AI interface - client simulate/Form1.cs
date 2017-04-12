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
using System.Threading;
using Motivation;

namespace Lolipop_AI_interface___client_simulate
{
    public partial class Form1 : Form
    {
        int port = 5;
        void SendMessage(string msg)
        {
            writer.WriteLine(msg);
            writer.Flush();
        }
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        private void Txb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.R || e.KeyCode == Keys.D0 || e.KeyCode == Keys.D1)
            {
                this.Text = "Sending messages...";
                SendMessage(e.KeyCode==Keys.R?"R":(e.KeyCode==Keys.D0?"0":"1"));
                //this.Text = "Message sent!";
                e.Handled = false;
                TXBinput.Clear();
                if (e.KeyCode == Keys.R) DataSaver.WriteLine("RESTART");
                string str = reader.ReadLine();
                System.Diagnostics.Debug.Assert(str.Length > 0);
                Do(() => { this.Text = str;TXBfeedBack.Text = str; });
                string[] s = str.Split(' ');
                StringBuilder sb = new StringBuilder();
                DataSaver.WriteLine(s[1] + " " + s[2] + " " + s[4] +" "+ s[5] + " " + s[6] + " " + s[7] + " " + s[0] + " " + (int.Parse(s[0])^1).ToString() + " " + (int.Parse(s[0]) ^ 1).ToString() + " " + (e.KeyCode == Keys.R ? "R" : (e.KeyCode == Keys.D0 ? "0" : "1")));
            }
            if (e.KeyCode == Keys.Enter)
            {
                this.Text = "Sending messages...";
                SendMessage(TXBinput.Text);
                //this.Text = "Message sent!";
                TXBinput.Clear();
            }
        }
        private void Do(Action a)
        {
            if (this.InvokeRequired) this.Invoke(a);
            else a.Invoke();
        }
        MyTextBox TXBinput,TXBauto,TXBautoType,TXBfeedBack;
        MyTableLayoutPanel TLPmain;
        double autoPlayRate = 0;
        int autoKey = 0;
        public Form1()
        {
            this.Size = new Size(750, 500);
            this.FormClosing += Form1_FormClosing;
            {
                TLPmain = new MyTableLayoutPanel(4, 2, "AAAP", "PP");
                {
                    TXBinput = new MyTextBox(false);
                    TXBinput.Multiline = false;
                    TXBinput.KeyDown += Txb_KeyDown;
                    TLPmain.AddControl(TXBinput, 0, 0);
                }
                {
                    TLPmain.AddControl(new MyLabel("auto play rate:"),1,0);
                }
                {
                    TLPmain.AddControl(new MyLabel("auto key"), 1, 1);
                }
                {
                    TXBauto = new MyTextBox(false, "0");
                    TXBauto.Multiline = false;
                    TXBauto.TextChanged += TXBauto_TextChanged;
                    TLPmain.AddControl(TXBauto, 2, 0);
                }
                {
                    TXBautoType = new MyTextBox(false, "0");
                    TXBautoType.Multiline = false;
                    TXBautoType.TextChanged += TXBautoType_TextChanged;
                    TLPmain.AddControl(TXBautoType, 2, 1);
                }
                {
                    TXBfeedBack = new MyTextBox(true);
                    TLPmain.AddControl(TXBfeedBack, 3, 0);
                }
                this.Controls.Add(TLPmain);
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(GetMyIpAddress(), port));
            stream = new NetworkStream(socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            new Thread(() =>
            {
                while (true)
                {
                    if (autoPlayRate == 0) Thread.Sleep(500);
                    else
                    {
                        Thread.Sleep((int)(1000.0 / autoPlayRate));
                        Do(() => { Txb_KeyDown(null, new KeyEventArgs(autoKey == 0 ? Keys.D0 : Keys.D1)); });
                    }
                }
            }).Start();
        }

        private void TXBautoType_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(TXBautoType.Text, out autoKey) || (autoKey != 0 && autoKey != 1)) MessageBox.Show("格式不正確");
        }

        private void TXBauto_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(TXBauto.Text, out autoPlayRate)) MessageBox.Show("格式不正確");
        }

        private void TXB_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataSaver.Close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
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
    }
    /*class ChatBox
    {
        int port = 20;

        public static void Main(String[] args)
        {
            ChatBox chatBox = new ChatBox();
            if (args.Length == 0)
                chatBox.ServerMain();
            else
                chatBox.ClientMain(args[0]);
        }

        public void ServerMain()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(ipep);
            newsock.Listen(10);
            Socket client = newsock.Accept();
            new TcpListener(client); // create a new thread and then receive message.
            newsock.Close();
        }

        public void ClientMain(String ip)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ipep);
            new TcpListener(server);
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

    }

    public class TcpListener
    {
        Socket socket;
        Thread inThread, outThread;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;

        public TcpListener(Socket s)
        {
            socket = s;
            stream = new NetworkStream(s);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            inThread = new Thread(new ThreadStart(inLoop));
            inThread.Start();
            outThread = new Thread(new ThreadStart(outLoop));
            outThread.Start();
            inThread.Join(); // 等待 inThread 執行續完成，才離開此函數。 
                             // (注意、按照 inLoop 的邏輯，這個函數永遠不會跳出，因為 inLoop 是個無窮迴圈)。
        }

        public void inLoop()
        {
            while (true)
            {
                String line = reader.ReadLine();
                Console.WriteLine("收到：" + line);
            }
        }

        public void outLoop()
        {
            while (true)
            {
                String line = Console.ReadLine();
                writer.WriteLine(line);
                writer.Flush();
            }
        }
    }*/
}
