using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Hackpad_Typer_2
{
    static class Environment
    {
        public static bool SendWithShiftEnter = true;
        public static bool HideFormInsteadOfMinimize = false;
        public static Form1 MainForm;
        public static List<HotKey> HOT_KEYS = new List<HotKey>();
        //public static event HotKey.HotkeyEventHandler OnHotKeyPressed;
        public delegate void HotKeyActivatedEventHandler();
        public static event HotKeyActivatedEventHandler HotKeyActivated;
        public static void OnHotKeyActivated()
        {
            if(HotKeyActivated!=null)HotKeyActivated();
        }
        public static void RegisterHotKeys()
        {
            foreach(HotKey hk in HOT_KEYS)
            {
                hk.OnHotkey += new HotKey.HotkeyEventHandler((object sender, HotKeyEventArgs e) => { Environment.OnHotKeyActivated(); });
            }
        }
        static string SAVE_FILE_NAME =System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)+ "\\HackpadTyperSettings.ini";
        public static void SaveFile()
        {
            StreamWriter writer = new StreamWriter(SAVE_FILE_NAME, false);
            writer.WriteLine(Environment.SendWithShiftEnter.ToString());
            writer.WriteLine(Environment.HideFormInsteadOfMinimize.ToString());
            foreach(HotKey hk in HOT_KEYS)
            {
                writer.WriteLine(hk.GetHotKey.ToString() + " " + hk.GetComboKey.ToString());
            }
            writer.Close();
        }
        public static void ReadFile()
        {
            FileInfo f = new FileInfo(SAVE_FILE_NAME);
            if (f.Exists)
            {
                StreamReader reader = new StreamReader(f.FullName);
                Dictionary<string, Keys> dict = new Dictionary<string, Keys>();
                foreach (FieldInfo fi in typeof(Keys).GetFields())
                {
                    if (fi.IsStatic && fi.GetValue(null) != null)
                    {
                        if (fi.GetValue(null).GetType() == typeof(Keys))
                        {
                            dict[fi.GetValue(null).ToString()] = (Keys)fi.GetValue(null);
                        }
                    }
                }
                if (!bool.TryParse(reader.ReadLine(), out Environment.SendWithShiftEnter)) goto SaveFileDamaged_Index;
                if (!bool.TryParse(reader.ReadLine(), out Environment.HideFormInsteadOfMinimize)) goto SaveFileDamaged_Index;
                for (string line = ""; (line = reader.ReadLine()) != null; )
                {
                    string[] s = line.Split(' ');
                    if (s.Length != 2) goto SaveFileDamaged_Index;
                    if (!dict.ContainsKey(s[0]) || !dict.ContainsKey(s[1])) goto SaveFileDamaged_Index;
                    HOT_KEYS.Add(new HotKey(MainForm.Handle, dict[s[0]], dict[s[1]]));
                }
                reader.Close();
                RegisterHotKeys();
                return;
            SaveFileDamaged_Index: ;
                reader.Close();
                HOT_KEYS.Clear();
                f.Delete();
                System.Windows.Forms.MessageBox.Show("Save file damaged, so it has been deleted.", "Error");
            }
        }
    }
}
