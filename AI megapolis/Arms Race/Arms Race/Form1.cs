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

namespace Arms_Race
{
    public partial class Form1 : Form
    {
        private MyTextBox TXBoutput;
        private void AppendMsg(string msg)
        {
            Do(() =>
            {
                this.Text = msg;
                TXBoutput.AppendText(msg + "\r\n");
            });
        }
        private void Do(Action a)
        {
            if (this.InvokeRequired) this.Invoke(a);
            else a.Invoke();
        }
        static IntPtr thisHandle;
        public static IntPtr getHandle()
        {
            return thisHandle;
        }
        Circuit circuit;
        public Form1()
        {
            this.Size = new Size(500, 500);
            this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            thisHandle = this.Handle;
            {
                TXBoutput = new MyTextBox(true);
                TXBoutput.WordWrap = true;
                this.Controls.Add(TXBoutput);
            }
            circuit = new Circuit();
            circuit.MessageAppended += new Circuit.MessageAppendedHandler((msg) => AppendMsg(msg));
            circuit.start();
        }
    }
}
