using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Digging_Game_Problems_Demonstrator
{
    public partial class Form1 : Form
    {
        const int N = 2;
        static Button[] BTNS = new Button[N];
        static TableLayoutPanel TLP = new TableLayoutPanel();
        static string[] TEXT = new string[N]
        {
            "PictureBox Performance Stability",
            "Self Make Image Processing Performance"
        };
        Form[] FORM = new Form[N]
        {
            new PictureBox_Performance_Stability(),
            new Self_Make_Image_Processing_Performance()
        };
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < N; i++) FORM[i].FormClosing += Form1_FormClosing;
            this.Controls.Add(TLP);
            {
                TLP.Dock = DockStyle.Fill;
                TLP.RowCount = N;
                for (int i = 0; i < N; i++)
                {
                    BTNS[i] = new Button();
                    BTNS[i].Dock = DockStyle.Fill;
                    BTNS[i].Text = TEXT[i];
                    BTNS[i].Click += Form1_Click;
                    TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                    TLP.Controls.Add(BTNS[i]); TLP.SetCellPosition(BTNS[i], new TableLayoutPanelCellPosition(0, i));
                }
            }
            this.FormClosed += Form1_FormClosed;
        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form f = sender as Form;
            e.Cancel = true;
            f.Hide();
            this.Show();
        }
        void Form1_Click(object sender, EventArgs e)
        {
            Button b=sender as Button;
            for (int i = 0; i < N; i++) if (BTNS[i] == b) { FORM[i].Show(); this.Hide(); break; }
        }
    }
}
