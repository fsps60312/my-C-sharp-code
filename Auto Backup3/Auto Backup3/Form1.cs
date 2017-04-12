using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Threading;

namespace Auto_Backup3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += Form1_FormClosing;
            InitializeComponent();
            this.Controls.Add(Panel1);
        }
        public void SaveSetting()
        {

        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
            Process.GetCurrentProcess().Kill();
        }
        public static TaskTableLayoutPanel Panel1 = new TaskTableLayoutPanel();
        public partial class TaskTableLayoutPanel:TableLayoutPanel
        {
            public partial class TaskCheckBox:CheckBox
            {
                public TaskCheckBox(string name)
                {
                    this.Text = name;
                    this.Dock = DockStyle.Top;
                    this.Checked = true;
                    this.MouseEnter += TaskCheckBox_MouseEnter;
                    this.MouseLeave += TaskCheckBox_MouseLeave;
                }
                public Thread ShineThread = null;
                public Thread ColorThread = null;
                public bool IsShining = false;
                public void StartShine()
                {
                    if (ShineThread != null) ShineThread.Abort();
                    ShineThread = new Thread(() =>
                    {
                        IsShining = true;
                        int shinerate = 1000;
                        while (true)
                        {
                            ChangeBackColor(Color.FromArgb(0, 0, 255));
                            Thread.Sleep(shinerate);
                            ChangeBackColor(Color.FromArgb(255, 0, 0));
                            Thread.Sleep(shinerate);
                            ChangeBackColor(Color.FromArgb(0, 255, 0));
                            Thread.Sleep(shinerate);
                        }
                    });
                    ShineThread.IsBackground = true;
                    //ShineThread.Priority = ThreadPriority.Lowest;
                    ShineThread.Start();
                }
                public void StopShine()
                {
                    if (ShineThread != null) ShineThread.Abort();
                    IsShining = false;
                }
                public void ChangeBackColor(Color a)
                {
                    if (ColorThread != null) ColorThread.Abort();
                    ColorThread = new Thread(() =>
                    {
                        while (true)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                BackColor = ApproachColor(BackColor, a);
                                if(IsLightColor(a))
                                {
                                    ForeColor = ApproachColor(ForeColor, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    ForeColor = ApproachColor(ForeColor, Color.FromArgb(255, 255, 255));
                                }
                            }
                            Thread.Sleep(10);
                        }
                    });
                    ColorThread.IsBackground = true;
                    //ColorThread.Priority = ThreadPriority.Lowest;
                    ColorThread.Start();
                }
                void TaskCheckBox_MouseLeave(object sender, EventArgs e)
                {
                    if(IsShining)
                    {
                        StartShine();
                    }
                    else
                    {
                        ChangeBackColor(DefaultBackColor);
                    }
                }
                void TaskCheckBox_MouseEnter(object sender, EventArgs e)
                {
                    if(IsShining)
                    {
                        ShineThread.Abort();
                    }
                    ChangeBackColor(Color.FromArgb(0, 0, 0));
                }
            }
            TaskCheckBox[] Task = new TaskCheckBox[20];
            public TaskTableLayoutPanel()
            {
                this.Dock = DockStyle.Fill;
                this.RowCount = Task.Length;
                for(int i=0;i<Task.Length;i++)
                {
                    Task[i] = new TaskCheckBox(i.ToString()+" Hello!");
                    this.Controls.Add(Task[i]);
                    this.SetCellPosition(Task[i], new TableLayoutPanelCellPosition(0, i));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                    Task[i].Click += TaskTableLayoutPanel_Click;
                }
                Thread thr = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    for (int i = 0; i < Task.Length; i++)
                    {
                        Thread.Sleep(100);
                        Task[i].StartShine();
                    }
                });
                thr.IsBackground = true;
                thr.Start();
            }
            void TaskTableLayoutPanel_Click(object sender, EventArgs e)
            {
                TaskCheckBox a = sender as TaskCheckBox;
                if (a.IsShining) a.StopShine();
                else a.StartShine();
            }
        }
        public static Color ApproachColor(Color a, Color b)
        {
            return Color.FromArgb(a.R == b.R ? a.R : (a.R < b.R ? a.R + 1 : a.R - 1), a.G == b.G ? a.G : (a.G < b.G ? a.G + 1 : a.G - 1), a.B == b.B ? a.B : (a.B < b.B ? a.B + 1 : a.B - 1));
        }
        public static bool IsLightColor(Color a)
        {
            int b = a.R;
            b += a.G;
            b += a.B;
            return b > (255 * 3) / 2;
        }
    }
}
