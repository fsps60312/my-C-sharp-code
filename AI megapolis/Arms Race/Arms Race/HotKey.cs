using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arms_Race
{
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
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            public static extern UInt32 GlobalAddAtom(String lpString);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);
            IntPtr _hWnd = IntPtr.Zero;
            UInt32 _hotKeyID;
            Keys _hotKey = Keys.None;
            public Keys GetHotKey { get { return _hotKey; } }
            Keys _comboKey = Keys.None;
            public Keys GetComboKey { get { return _comboKey; } }
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
            public bool disposed = false;
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
        class GlobalKeysDetector
        {
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            public static extern UInt32 GlobalAddAtom(String lpString);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);
            List<HotKey> HOTKEIES = new List<HotKey>();
            public GlobalKeysDetector(IntPtr formHandle)
            {
                HashSet<Keys> combo_key = new HashSet<Keys> { Keys.Alt, Keys.Control, Keys.Shift, Keys.LWin, Keys.None };
                List<Keys> hot_key = new List<Keys>();
                HashSet<Keys> not_hot_key = new HashSet<Keys> { Keys.ControlKey, Keys.Menu, Keys.ShiftKey/*, Keys.KeyCode, Keys.Modifiers, Keys.LButton, Keys.RButton, Keys.Cancel,Keys.MButton,Keys.XButton1,Keys.XButton2
            ,Keys.Back,Keys.Tab*/};
                foreach (System.Reflection.FieldInfo p in typeof(Keys).GetFields())
                {
                    //MessageBox.Show(p.Name);
                    if (p.IsStatic)
                    {
                        //MessageBox.Show(p.GetValue(null).GetType().ToString() + "\r\n" + typeof(Keys).ToString());
                        if (p.GetValue(null).GetType() == typeof(Keys))
                        {
                            Keys k = (Keys)p.GetValue(null);
                            if (!combo_key.Contains(k) && !not_hot_key.Contains(k))
                            {
                                hot_key.Add(k);
                            }
                        }
                    }
                }
                //MessageBox.Show(hot_key.Count.ToString());
                foreach (Keys hk in hot_key)
                {
                    foreach (Keys ck in combo_key)
                    {
                        HotKey v = new HotKey(formHandle, hk, ck);
                        v.OnHotkey += new HotKey.HotkeyEventHandler(KeyDetected);
                        HOTKEIES.Add(v);
                    }
                }
            }
            private void KeyDetected(object sender, HotKeyEventArgs e)
            {
                //MessageBox.Show(e.HotKey.ToString());
                if (OnKeyDetected != null)
                {
                    //MessageBox.Show("b");
                    OnKeyDetected(this, e);
                }
            }
            public event HotKey.HotkeyEventHandler OnKeyDetected;
            private bool disposed = false;
            public void Dispose()
            {
                if (!disposed)
                {
                    foreach (HotKey k in HOTKEIES) k.Dispose();
                    HOTKEIES.Clear();
                    OnKeyDetected = null;
                    disposed = true;
                }
            }
            ~GlobalKeysDetector()
            {
                Dispose();
            }
        }
}
