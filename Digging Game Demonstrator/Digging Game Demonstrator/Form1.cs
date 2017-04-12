using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Digging_Game_Demonstrator
{
    public partial class Form1 : Form
    {
        PictureBox PBX = new PictureBox();
        TableLayoutPanel TLP_MAIN = new TableLayoutPanel();
        TableLayoutPanel TLP_BTN = new TableLayoutPanel();
        Button[] BTN = new Button[10];
        bool FULLSCREEN = false;
        void Form1_Shown(object sender, EventArgs e)
        {
            PBX.Image = BITMAP.FromFile("Copper.png");
        }
        public Form1()
        {
            this.KeyPreview = true;
            PublicVariables.THIS = this;
            PublicVariables.PBX = PBX;
            this.HandleCreated += Form1_HandleCreated;
            this.HandleDestroyed += Form1_HandleDestroyed;
        }
        void Form1_HandleDestroyed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Dispose();
        }
        void Form1_HandleCreated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(TLP_MAIN);
            {
                TLP_MAIN.Dock = DockStyle.Fill;
                TLP_MAIN.ColumnCount = 2;
                TLP_MAIN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                TLP_MAIN.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                TLP_MAIN.Controls.Add(PBX); TLP_MAIN.SetCellPosition(PBX, new TableLayoutPanelCellPosition(0, 0));
                {
                    PBX.Dock = DockStyle.Fill;
                    PBX.SizeMode = PictureBoxSizeMode.StretchImage;
                    PBX.DoubleClick += PBX_DoubleClick;
                }
                TLP_MAIN.Controls.Add(TLP_BTN); TLP_MAIN.SetCellPosition(TLP_BTN, new TableLayoutPanelCellPosition(1, 0));
                {
                    TLP_BTN.Dock = DockStyle.Fill;
                    TLP_BTN.RowCount = BTN.Length;
                    TLP_BTN.AutoSize = true;
                    TLP_BTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    for(int i=0;i<BTN.Length;i++)
                    {
                        BTN[i] = new Button();
                        TLP_BTN.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                        TLP_BTN.Controls.Add(BTN[i]); TLP_BTN.SetCellPosition(BTN[i], new TableLayoutPanelCellPosition(0, i));
                        BTN[i].AutoSize = true;
                        BTN[i].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        BTN[i].Dock = DockStyle.Fill;
                        BTN[i].Text = i.ToString();
                    }
                }
            }
            this.Shown += Form1_Shown;
        }
        void PBX_DoubleClick(object sender, EventArgs e)
        {
            FULLSCREEN ^= true;
            if (FULLSCREEN)
            {
                this.Controls.Add(PBX);
                this.Controls.Remove(TLP_MAIN);
            }
            else
            {
                this.Controls.Add(TLP_MAIN);
                this.Controls.Remove(PBX);
            }
        }
    }
}