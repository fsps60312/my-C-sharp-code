using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Motivation;
using System.IO;
using System.Diagnostics;

namespace Megapolis
{
    class XButtonForm:Form
    {
        private string saveFileFolder { get { return @"XButtons\"; } }
        private string saveFile { get { return saveFileFolder + "Settings.ini"; } }
        private Dictionary<string, Rectangle> settings = new Dictionary<string, Rectangle>();
        private List<string> imageFiles = new List<string>();
        private int _browseIndex = 0;
        private int browseIndex
        {
            get { return _browseIndex; }
            set
            {
                if ((TLPmain.Enabled = (imageFiles.Count > 0)))
                {
                    BTNprev.Enabled = (value - 1 >= 0);
                    BTNnext.Enabled = (value + 1 < imageFiles.Count);
                    if (PBXmain.Image != null) PBXmain.Image.Dispose();
                    PBXmain.Image = Image.FromFile(imageFiles[value]);
                    TXBfileName.Text = $"{value + 1} / {imageFiles.Count}\r\n{PBXmain.Image.Width} * {PBXmain.Image.Height} pixels\r\n{imageFiles[value]}";
                    if (settings.ContainsKey(imageFiles[value]))
                    {
                        Rectangle r = settings[imageFiles[value]];
                        IFD.GetTextBox("X").Text = r.X.ToString();
                        IFD.GetTextBox("Y").Text = r.Y.ToString();
                        IFD.GetTextBox("Width").Text = r.Width.ToString();
                        IFD.GetTextBox("Height").Text = r.Height.ToString();
                        if (PBXsub.Image != null) PBXsub.Image.Dispose();
                        PBXsub.Image = MyBitmap.Capture(PBXmain.Image as Bitmap, r);
                    }
                    else
                    {
                        if (PBXsub.Image != null)
                        {
                            PBXsub.Image.Dispose();
                            PBXsub.Image = null;
                        }
                    }
                }
                _browseIndex = value;
            }
        }
        public void LoadSettings()
        {
            if (!new FileInfo(saveFile).Exists)
            {
                StreamWriter writer = new StreamWriter(saveFile, false);
                writer.Close();
            }
            StreamReader reader = new StreamReader(saveFile, Encoding.UTF8);
            List<string> data = new List<string>();
            for (string s; (s = reader.ReadLine()) != null;) data.Add(s);
            reader.Close();
            try
            {
                Trace.Assert(data.Count % 5 == 0);
                settings.Clear();
                for (int i = 0; i < data.Count; i += 5)
                {
                    settings[data[i]] = new Rectangle(int.Parse(data[i + 1]), int.Parse(data[i + 2]), int.Parse(data[i + 3]), int.Parse(data[i + 4]));
                }
            }
            catch(Exception error)
            {
                MessageBox.Show($"Save file damaged, loading failed.\r\n{error}", "Error");
                settings.Clear();
            }
            var fileList = new DirectoryInfo(saveFileFolder).GetFiles();
            //if (imageFiles.Count == 0)
            //{
            //    List<KeyValuePair<double, int>> idx = new List<KeyValuePair<double, int>>();
            //    for (int i = 0; i < fileList.Length; i++) idx.Add(new KeyValuePair<double, int>(Constant.random.NextDouble(), i));
            //    idx.Sort(new Comparison<KeyValuePair<double, int>>((a, b) => { return a.Key < b.Key ? -1 : 1; }));
            //    var newFileList = new List<FileInfo>();
            //    for (int i = 0; i < fileList.Length; i++) newFileList.Add(fileList[idx[i].Value]);
            //    fileList = newFileList.ToArray();
            //}
            //else
            //{
                imageFiles.Clear();
            //}
            foreach (var info in fileList)
            {
                if (info.Extension.ToLower() == ".png")
                {
                    imageFiles.Add(info.FullName);
                }
            }
            if (browseIndex >= imageFiles.Count && imageFiles.Count > 0) browseIndex = imageFiles.Count - 1;
            else browseIndex = browseIndex;
            OnSettingsSaving();
        }
        private void SaveSettings()
        {
            StreamWriter writer = new StreamWriter(saveFile, false, Encoding.UTF8);
            foreach(var p in settings)
            {
                writer.WriteLine(p.Key);
                writer.WriteLine(p.Value.X);
                writer.WriteLine(p.Value.Y);
                writer.WriteLine(p.Value.Width);
                writer.WriteLine(p.Value.Height);
            }
            writer.Close();
            OnSettingsSaving();
        }
        private void BTNreload_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }
        private void BTNnext_Click(object sender, EventArgs e)
        {
            browseIndex++;
        }
        private void BTNprev_Click(object sender, EventArgs e)
        {
            browseIndex--;
        }
        public delegate void SettingsSavingEventHandler(List<KeyValuePair<Bitmap, Point>> data);
        public SettingsSavingEventHandler SettingsSaving;
        private void OnSettingsSaving()
        {
            List<KeyValuePair<Bitmap, Point>> data = new List<KeyValuePair<Bitmap, Point>>();
            foreach(var p in settings)
            {
                Bitmap bmp = new Bitmap(p.Key);
                data.Add(new KeyValuePair<Bitmap, Point>(MyBitmap.Capture(bmp, p.Value), p.Value.Location));
                bmp.Dispose();
            }
            SettingsSaving?.Invoke(data);
        }
        private void BTNsave_Click(object sender, EventArgs e)
        {
            try
            {
                Rectangle r = new Rectangle(int.Parse(IFD.GetField("X")), int.Parse(IFD.GetField("Y")), int.Parse(IFD.GetField("Width")), int.Parse(IFD.GetField("Height")));
                PBXsub.Image = MyBitmap.Capture(PBXmain.Image as Bitmap, r);
                settings[imageFiles[browseIndex]] = r;
                SaveSettings();
            }
            catch(Exception error)
            {
                MessageBox.Show($"Can't save the setting\r\n{error}", "Error");
            }
        }
        private void BTNwhatsNew_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < imageFiles.Count; i++)
            {
                if (!settings.ContainsKey(imageFiles[i]))
                {
                    browseIndex = i;
                    return;
                }
            }
            MessageBox.Show("There are no new captures to set");
        }
        private PictureBox PBXmain, PBXsub;
        private MyTableLayoutPanel TLPmain, TLPsub, TLPctrl, TLPbtn;
        private MyInputField IFD;
        private MyTextBox TXBfileName;
        private MyButton BTNsave,BTNreload, BTNwhatsNew, BTNprev, BTNnext;
        public XButtonForm() : base()
        {
            this.Text = "X Button settings";
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                e.Cancel = true;
                this.Hide();
            };
            {
                TLPmain = new MyTableLayoutPanel(2, 1, "AA", "A");
                TLPmain.Enabled = false;
                {
                    PBXmain = new PictureBox();
                    PBXmain.MinimumSize = Constant.MyTaskCaptureSize;
                    PBXmain.SizeMode = PictureBoxSizeMode.AutoSize;
                    TLPmain.AddControl(PBXmain, 0, 0);
                }
                {
                    TLPsub = new MyTableLayoutPanel(1, 3, "A", "AAA");
                    {
                        TLPctrl = new MyTableLayoutPanel(2, 1, "AA", "P");
                        {
                            TLPbtn = new MyTableLayoutPanel(1, 5, "A", "P2P2P2P1P1");
                            {
                                BTNsave = new MyButton("View && Save");
                                BTNsave.Click += BTNsave_Click;
                                TLPbtn.AddControl(BTNsave, 0, 0);
                            }
                            {
                                BTNreload = new MyButton("Reload");
                                BTNreload.Click += BTNreload_Click;
                                TLPbtn.AddControl(BTNreload, 0, 1);
                            }
                            {
                                BTNwhatsNew = new MyButton("What's New");
                                BTNwhatsNew.Click += BTNwhatsNew_Click;
                                TLPbtn.AddControl(BTNwhatsNew, 0, 2);
                            }
                            {
                                BTNprev = new MyButton("<");
                                BTNprev.Click += BTNprev_Click;
                                TLPbtn.AddControl(BTNprev, 0, 3);
                            }
                            {
                                BTNnext = new MyButton(">");
                                BTNnext.Click += BTNnext_Click;
                                TLPbtn.AddControl(BTNnext, 0, 4);
                            }
                            TLPctrl.AddControl(TLPbtn, 0, 0);
                        }
                        {
                            TXBfileName = new MyTextBox(false,"(File name)");
                            TXBfileName.Enabled = false;
                            TXBfileName.WordWrap = true;
                            TLPctrl.AddControl(TXBfileName, 1, 0);
                        }
                        TLPsub.AddControl(TLPctrl, 0, 0);
                    }
                    {
                        IFD = new MyInputField();
                        IFD.AddField("X");
                        IFD.AddField("Y");
                        IFD.AddField("Width");
                        IFD.AddField("Height");
                        TLPsub.AddControl(IFD, 0, 1);
                    }
                    {
                        PBXsub = new PictureBox();
                        PBXsub.SizeMode = PictureBoxSizeMode.AutoSize;
                        TLPsub.AddControl(PBXsub, 0, 2);
                    }
                    TLPmain.AddControl(TLPsub, 1, 0);
                }
                this.Controls.Add(TLPmain);
            }
        }
    }
    static class XButton
    {
        private static XButtonForm form;
        private static List<KeyValuePair<Bitmap, Point>> data = new List<KeyValuePair<Bitmap, Point>>();
        static XButton()
        {
            form = new XButtonForm();
            form.SettingsSaving += delegate (List<KeyValuePair<Bitmap, Point>> _data)
              {
                  foreach (var p in data) p.Key.Dispose();
                  data = _data;
              };
            form.LoadSettings();
        }
        public static void ShowWindow()
        {
            form.Show();
        }
        public static Point GetXButtonLocation(Bitmap bmp)
        {
            foreach(var p in data)
            {
                if (MyBitmap.IsMatch(bmp, p.Key, p.Value)) return new Point(p.Value.X + p.Key.Width / 2, p.Value.Y + p.Key.Height / 2);
            }
            return MyBitmap.failedPoint;
        }
        public static void SetRecoverButton(Button btn)
        {
            form.FormClosing += delegate
            {
                btn.Enabled = true;
            };
        }
    }
}
