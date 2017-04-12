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
using System.Diagnostics;

namespace MegapolisClientSimulate
{
    public partial class Form1 : Form
    {
        MyTableLayoutPanel TLPmain,TLPbtn;
        MyLabel LBLstatus;
        MyTextBox TXBlog;
        List<MyButton> BTNS = new List<MyButton>();
        public Form1()
        {
            this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            InitializeControls();
            NetworkCommunicator.LogChanged += delegate (string log)
            {
                Do(() =>
                {
                    //LBLstatus.Text = log;
                    TXBlog.AppendText(log + "\r\n");
                });
            };
            NetworkCommunicator.StatusChanged += delegate (string status)
            {
                Do(() => { LBLstatus.Text = status; });
            };
            NetworkCommunicator.Start();
        }
        void InitializeControls()
        {
            this.Size = new Size(600, 600);
            {
                TLPmain = new MyTableLayoutPanel(3, 1, "PAA", "P");
                {
                    TXBlog = new MyTextBox(true);
                    TLPmain.AddControl(TXBlog, 0, 0);
                }
                {
                    LBLstatus = new MyLabel("");
                    TLPmain.AddControl(LBLstatus, 1, 0);
                }
                {
                    BTNS.Add(new MyButton("Click Start Button"));
                    BTNS.Add(new MyButton("Click Stop Button"));
                    BTNS.Add(new MyButton("Get TXBlog.Text"));
                    BTNS.Add(new MyButton("Is BlueStacks Started"));
                    BTNS.Add(new MyButton("Close BlueStacks"));
                    BTNS.Add(new MyButton("Get TXBlog.Text Updates"));
                    BTNS.Add(new MyButton("Show Next Run Time"));
                    Debug.Assert(BTNS.Count == 7);
                    TLPbtn = new MyTableLayoutPanel(2, 4, "PP", "PPPP");
                    for(int i=0;i<BTNS.Count;i++)
                    {
                        BTNS[i].Font = new Font("Consolas", 7, FontStyle.Regular);
                        BTNS[i].Click += Form1_Click;
                        TLPbtn.AddControl(BTNS[i], i / 4, i % 4);
                    }
                    TLPmain.AddControl(TLPbtn, 2, 0);
                }
                this.Controls.Add(TLPmain);
            }
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            NetworkCommunicator.SendMessage((sender as MyButton).Text);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        private void Do(Action a)
        {
            if (this.InvokeRequired) this.Invoke(a);
            else a.Invoke();
        }
    }
}
