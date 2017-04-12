using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hackpad_Typer_2
{
    public partial class Form1 : Form
    {
        /*HotKey hotkey1, hotkey2, hotkey3, hotkey4, hotkey5;
        private void button1_Click(object sender, EventArgs e)
        {
            hotkey1 = new HotKey(this.Handle, Keys.F2, Keys.None); //註冊F2為熱鍵, 如果不要組合鍵請傳Keys.None當參數
            hotkey1.OnHotkey += new HotKey.HotkeyEventHandler(hotkey1to4_OnHotkey); //hotkey1~4共用事件

            hotkey2 = new HotKey(this.Handle, Keys.F2, Keys.Control); //註冊Control+F2為熱鍵
            hotkey2.OnHotkey += new HotKey.HotkeyEventHandler(hotkey1to4_OnHotkey); //hotkey1~4共用事件

            hotkey3 = new HotKey(this.Handle, Keys.F2, Keys.Shift); //註冊Shift+F2為熱鍵
            hotkey3.OnHotkey += new HotKey.HotkeyEventHandler(hotkey1to4_OnHotkey); //hotkey1~4共用事件

            hotkey4 = new HotKey(this.Handle, Keys.F2, Keys.Alt); //註冊Alt+F2為熱鍵
            hotkey4.OnHotkey += new HotKey.HotkeyEventHandler(hotkey1to4_OnHotkey); //hotkey1~4共用事件

            hotkey5 = new HotKey(this.Handle, Keys.F2, Keys.LWin); //註冊Win+F2為熱鍵
            hotkey5.OnHotkey += new HotKey.HotkeyEventHandler(hotkey5_OnHotkey); //獨立事件
        }
        private void hotkey1to4_OnHotkey(object sender, HotKeyEventArgs e)
        {
            MessageBox.Show("熱鍵" + e.ComboKey.ToString() + "+" + e.HotKey.ToString() + "被觸發了!", "共用事件");
        }
        private void hotkey5_OnHotkey(object sender, HotKeyEventArgs e)
        {
            MessageBox.Show("熱鍵" + e.ComboKey.ToString() + "+" + e.HotKey.ToString() + "被觸發了!", "獨立事件");
        }*/
        TableLayoutPanel TLP_MAIN = new TableLayoutPanel();
        Button BTN_SETTING = new Button();
        TextBox TXB_INPUT = new TextBox();
        public Form1()
        {
            this.Icon = Properties.Resources.Icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Hackpad Typer 2.11 (by Motivation)";
            Environment.MainForm = this;
            this.Height = 88; this.Width = 750;
            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TLP_MAIN.Dock = DockStyle.Fill;
            TLP_MAIN.AutoSize = true;
            TLP_MAIN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TLP_MAIN.ColumnCount = 2;
            TLP_MAIN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            TLP_MAIN.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
            {
                {
                    TXB_INPUT.Dock = DockStyle.Fill;
                    TXB_INPUT.KeyUp += TXB_INPUT_KeyUp;
                    TXB_INPUT.KeyDown += TXB_INPUT_KeyDown;
                    TXB_INPUT.ScrollBars = ScrollBars.Vertical;
                    TXB_INPUT.WordWrap = false;
                    TXB_INPUT.Multiline = true;
                    TXB_INPUT.Font = new Font("微軟正黑體", 20, FontStyle.Regular);
                    TXB_INPUT.TextChanged += TXB_INPUT_TextChanged;
                }
                TLP_MAIN.Controls.Add(TXB_INPUT); TLP_MAIN.SetCellPosition(TXB_INPUT, new TableLayoutPanelCellPosition(0, 0));
                {
                    BTN_SETTING.Text = "Settings";
                    BTN_SETTING.Dock = DockStyle.Fill;
                    BTN_SETTING.AutoSize = true;
                    BTN_SETTING.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    BTN_SETTING.Click += BTN_SETTING_Click;
                }
                TLP_MAIN.Controls.Add(BTN_SETTING); TLP_MAIN.SetCellPosition(BTN_SETTING, new TableLayoutPanelCellPosition(1, 0));
            }
            this.Controls.Add(TLP_MAIN);
            Environment.HotKeyActivated += Environment_HotKeyActivated;
            //button1_Click(null,null);
            this.Resize += Form1_Move;
            this.Move += Form1_Move;
            this.FormClosing += Form1_FormClosing;
            //Environment.HOT_KEIES.Add(new HotKey(this.Handle, Keys.Q, Keys.Control));
            Environment.ReadFile();
            //MessageBox.Show(this.Height.ToString());
        }
        void TXB_INPUT_TextChanged(object sender, EventArgs e)
        {
            string s = (sender as TextBox).Text;
            int cntr = 0, cntn = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\r') ++cntr;
                if (s[i] == '\n') ++cntn;
            }
            this.Height =(88-34)+ 34 * (Math.Max(cntr, cntn) + 1);
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.SaveFile();
        }
        void TXB_INPUT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A) TXB_INPUT.SelectAll();
        }
        string ProcessInputString(string original_string)
        {
            string ans = "";
            HashSet<char>special_charactors=new HashSet<char>{'+','^','%','~','(',')','[',']','{','}'};
            for(int i=0;i<original_string.Length;i++)
            {
                if (i + 1 < original_string.Length && original_string[i] == 'r' && original_string[i + 1] == '\n')
                {
                    ans += "{ENTER}";
                    i++;
                }
                else if(original_string[i]=='\r'||original_string[i]=='\n')
                {
                    ans += "{ENTER}";
                }
                else
                {
                    char c = original_string[i];
                    if (special_charactors.Contains(c)) ans += "{" + c + "}";
                    else ans += c;
                }
            }
            if (ans.Length >= 7 && ans.Substring(ans.Length - 7) == "{ENTER}") ans = ans.Remove(ans.Length - 7);
            return ans;
        }
        void TXB_INPUT_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Environment.SendWithShiftEnter&& e.KeyCode == Keys.Enter && e.Modifiers == Keys.Shift)
                || (!Environment.SendWithShiftEnter && e.KeyCode == Keys.Enter && e.Modifiers != Keys.Shift))
            {
                this.Hide();
                SendKeys.SendWait("   " + ProcessInputString(TXB_INPUT.Text));
                if (!Environment.HideFormInsteadOfMinimize)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Show();
                }
            }

        }
        void Form1_Move(object sender, EventArgs e)
        {
            /*Size sz = TXB_INPUT.MaximumSize;
            sz.Width = Screen.FromControl(this).WorkingArea.Width - this.Left;
            TXB_INPUT.MaximumSize = sz;*/
        }
        void Environment_HotKeyActivated()
        {
            TXB_INPUT.Clear();
            TXB_INPUT.Focus();
            if(!this.Visible||this.WindowState==FormWindowState.Minimized)
            {
                if (!this.Visible) this.Visible = true;
                if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            }
            else
            {
                if (Environment.HideFormInsteadOfMinimize) this.Hide();
                else this.WindowState = FormWindowState.Minimized;
            }
        }
        void BTN_SETTING_Click(object sender, EventArgs e)
        {
            foreach(var a in Environment.HOT_KEYS)
            {
                if (a.disposed) MessageBox.Show("BTN_SETTING_Click error");
            }
            new SettingForm().Show();
        }
    }
}
