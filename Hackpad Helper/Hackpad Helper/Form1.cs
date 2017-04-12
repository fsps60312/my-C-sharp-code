using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hackpad_Helper
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);
        public class HotKeyEventArgs : EventArgs
        {
            private Keys _hotKey;
            public Keys HotKey //熱鍵
            {
                get { return _hotKey; }
                private set { }
            }

            private Keys _comboKey;
            public Keys ComboKey //組合鍵
            {
                get { return _comboKey; }
                private set { }
            }

            public HotKeyEventArgs(Keys hotKey, Keys comboKey)
            {
                _hotKey = hotKey;
                _comboKey = comboKey;
            }
        }
        class HotKey : IMessageFilter
        {
            IntPtr _hWnd = IntPtr.Zero;
            UInt32 _hotKeyID;
            Keys _hotKey = Keys.None;
            Keys _comboKey = Keys.None;

            public HotKey(IntPtr formHandle, Keys hotKey, Keys comboKey)
            {
                _hWnd = formHandle; //Form Handle, 註冊系統熱鍵需要用到這個
                _hotKey = hotKey; //熱鍵
                _comboKey = comboKey; //組合鍵, 必須設定Keys.Control, Keys.Alt, Keys.Shift, Keys.None以及Keys.LWin等值才有作用

                UInt32 uint_comboKey; //由於API對於組合鍵碼的定義不一樣, 所以我們這邊做個轉換
                switch (comboKey)
                {
                    case Keys.Alt:
                        uint_comboKey = 0x1;
                        break;
                    case Keys.Control:
                        uint_comboKey = 0x2;
                        break;
                    case Keys.Shift:
                        uint_comboKey = 0x4;
                        break;
                    case Keys.LWin:
                        uint_comboKey = 0x8;
                        break;
                    default: //沒有組合鍵
                        uint_comboKey = 0x0;
                        break;
                }

                _hotKeyID = GlobalAddAtom(Guid.NewGuid().ToString()); //向系統取得一組id
                RegisterHotKey((IntPtr)_hWnd, _hotKeyID, uint_comboKey, (UInt32)hotKey); //使用Form Handle與id註冊系統熱鍵
                Application.AddMessageFilter(this); //使用HotKey類別來監視訊息
            }
            public delegate void HotkeyEventHandler(object sender, HotKeyEventArgs e); //HotKeyEventArgs是自訂事件參數
            public event HotkeyEventHandler OnHotkey; //自訂事件

            const int WM_GLOBALHOTKEYDOWN = 0x312; //當按下系統熱鍵時, 系統會發送的訊息

            public bool PreFilterMessage(ref Message m)
            {
                if (OnHotkey != null && m.Msg == WM_GLOBALHOTKEYDOWN && (UInt32)m.WParam == _hotKeyID) //如果接收到系統熱鍵訊息且id相符時
                {
                    OnHotkey(this, new HotKeyEventArgs(_hotKey, _comboKey)); //呼叫自訂事件, 傳遞自訂參數
                    return true; //並攔截這個訊息, Form將不再接收到這個訊息
                }

                return false;
            }
            private bool disposed = false;
            public void Dispose()
            {
                if (!disposed)
                {
                    UnregisterHotKey(_hWnd, _hotKeyID); //取消熱鍵
                    GlobalDeleteAtom(_hotKeyID); //刪除id
                    OnHotkey = null; //取消所有關聯的事件
                    Application.RemoveMessageFilter(this); //不再使用HotKey類別監視訊息

                    GC.SuppressFinalize(this);
                    disposed = true;
                }
            }
            ~HotKey()
            {
                Dispose();
            }
        }
        HotKey hotkey1, hotkey2, hotkey3, hotkey4, hotkey5;

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
        private void button2_Click(object sender, EventArgs e)
        {
            hotkey1.Dispose();
            hotkey2.Dispose();
            hotkey3.Dispose();
            hotkey4.Dispose();
            hotkey5.Dispose();
        }

        private void hotkey1to4_OnHotkey(object sender, HotKeyEventArgs e)
        {
            MessageBox.Show("熱鍵" + e.ComboKey.ToString() + "+" + e.HotKey.ToString() + "被觸發了!", "共用事件");
        }

        private void hotkey5_OnHotkey(object sender, HotKeyEventArgs e)
        {
            MessageBox.Show("熱鍵" + e.ComboKey.ToString() + "+" + e.HotKey.ToString() + "被觸發了!", "獨立事件");
        }

        public Form1()
        {
            InitializeComponent();
            Button button1 = new Button(),button2=new Button();
            button1.Text = "Register"; button2.Text = "Cancel";
            this.Controls.Add(button1); button1.Dock = DockStyle.Top;
            this.Controls.Add(button2); button2.Dock = DockStyle.Top;
            button1.Click+=button1_Click;
            button2.Click+=button2_Click;
        }
    }
}
