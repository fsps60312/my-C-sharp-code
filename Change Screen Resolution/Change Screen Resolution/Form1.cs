using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Change_Screen_Resolution
{
    public partial class Form1 : Form
    {
        TableLayoutPanel TLP_MAIN;
        TableLayoutPanel TLP_SETS;
        Button SAVE;
        Label[] LABS;
        TextBox[] TXBS;
        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            TLP_MAIN = new TableLayoutPanel();
            TLP_SETS = new TableLayoutPanel();
            SAVE = new Button();
            LABS = new Label[4];
            TXBS = new TextBox[3];
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //this.MaximizeBox = false;
            {
                TLP_MAIN.Dock = DockStyle.Fill;
                TLP_MAIN.AutoSize = true;
                TLP_MAIN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                TLP_MAIN.ColumnCount = 1;
                TLP_MAIN.RowCount = 2;
                TLP_MAIN.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP_MAIN.SetCellPosition(TLP_SETS, new TableLayoutPanelCellPosition(0, 0));
                {
                    TLP_SETS.Dock = DockStyle.Fill;
                    TLP_SETS.ColumnCount = 7;
                    TLP_SETS.RowCount = 1;
                    for (int i = 0; i < TLP_SETS.ColumnCount; i++) TLP_SETS.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                    TLP_SETS.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                    string[] lab_texts = new string[] { " Set Resolution : ", " * ", " Frequency : ", " Hz " };
                    for (int i = 0; i < LABS.Length;i++ )
                    {
                        var l = LABS[i] = new Label();
                        l.Dock = DockStyle.Fill;
                        l.AutoSize = true;
                        l.Text = lab_texts[i];
                    }
                    for (int i = 0; i < TXBS.Length;i++ )
                    {
                        var t = TXBS[i] = new TextBox();
                        t.Dock = DockStyle.Fill;
                    }
                    int idx=0;
                    int lab_idx = 0;
                    int txb_idx = 0;
                    TLP_SETS.SetCellPosition(LABS[lab_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(LABS[lab_idx]); lab_idx++;
                    TLP_SETS.SetCellPosition(TXBS[txb_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(TXBS[txb_idx]); txb_idx++;
                    TLP_SETS.SetCellPosition(LABS[lab_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(LABS[lab_idx]); lab_idx++;
                    TLP_SETS.SetCellPosition(TXBS[txb_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(TXBS[txb_idx]); txb_idx++;
                    TLP_SETS.SetCellPosition(LABS[lab_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(LABS[lab_idx]); lab_idx++;
                    TLP_SETS.SetCellPosition(TXBS[txb_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(TXBS[txb_idx]); txb_idx++;
                    TLP_SETS.SetCellPosition(LABS[lab_idx], new TableLayoutPanelCellPosition(idx++, 0)); TLP_SETS.Controls.Add(LABS[lab_idx]); lab_idx++;
                } TLP_MAIN.Controls.Add(TLP_SETS);
                TLP_MAIN.SetCellPosition(SAVE, new TableLayoutPanelCellPosition(0, 1));
                {
                    SAVE.Dock = DockStyle.Fill;
                    SAVE.Text = "Save";
                    SAVE.AutoSize = true;
                    SAVE.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    SAVE.Click += SAVE_Click;
                } TLP_MAIN.Controls.Add(SAVE);
            } this.Controls.Add(TLP_MAIN);
            ShowScreenInfo();
        }
        void SAVE_Click(object sender, EventArgs e)
        {
            int w, h, f;
            if (!int.TryParse(TXBS[0].Text, out w)) { MessageBox.Show("X Resolution Format Error"); return; }
            if (!int.TryParse(TXBS[1].Text, out h)) { MessageBox.Show("Y Resolution Format Error"); return; }
            if (!int.TryParse(TXBS[2].Text, out f)) { MessageBox.Show("Screen Update Frequency Format Error"); return; }
            ApplyScreenSettings(w, h, f);
            ShowScreenInfo();
        }
        void ShowScreenInfo()
        {
            TXBS[0].Text = Screen.PrimaryScreen.Bounds.Width.ToString();
            TXBS[1].Text = Screen.PrimaryScreen.Bounds.Height.ToString();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct DEVMODE
        {
            public const int DM_DISPLAYFREQUENCY = 0x400000;
            public const int DM_PELSWIDTH = 0x80000;
            public const int DM_PELSHEIGHT = 0x100000;
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public DMDO dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int ChangeDisplaySettings([In] ref DEVMODE lpDevMode, int dwFlags);
        public enum DMDO
        {
            DEFAULT = 0,
            D90 = 1,
            D180 = 2,
            D270 = 3
        }
        private void ApplyScreenSettings(int intWidth, int intHeight, int intFrequency)
        {
            long RetVal = 0;
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            dm.dmPelsWidth = intWidth;
            dm.dmPelsHeight = intHeight;
            dm.dmDisplayFrequency = intFrequency;
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;
            RetVal = ChangeDisplaySettings(ref dm, 0);
        }
    }
}