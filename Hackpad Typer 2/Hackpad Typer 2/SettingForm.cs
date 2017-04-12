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
    public partial class SettingForm : Form
    {
        abstract partial class SettingPanel:GroupBox
        {
            public SettingPanel(SettingForm parent)
            {
                LoadSettings();
                SettingChanged += parent.OnSettingChanged;
                this.Dock = DockStyle.Top;
                this.AutoSize = true;
                this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
            abstract public void SaveSettings();
            abstract public void LoadSettings();
            public delegate void EventHandler();
            public event EventHandler SettingChanged;
            public void OnSettingChanged()
            {
                if (SettingChanged != null) SettingChanged();
            }
        }
        unsafe partial class TwoOptionSettingPanel : SettingPanel
        {
            //delegate void MyAction();
            bool* VALUE;
            public TwoOptionSettingPanel(SettingForm parent,string text,string option1,string option2, ref bool _value)
                : base(parent)
            {
                this.Text = text;
                RBS.Text = option1;
                RBE.Text = option2;
                //ACTION1 = new MyAction(() => { value = true; });
                //ACTION1 = new MyAction(() => { value = false; });
                fixed (bool *v = &_value)
                {
                    VALUE = v;
                }
                if (*VALUE) RBS.Checked = true;
                else RBE.Checked = true;
            }
            public override void SaveSettings()
            {
                if (RBS.Checked) *VALUE = true;
                else if (RBE.Checked) *VALUE = false;
                else throw new Exception();
            }
            RadioButton RBS = new RadioButton();
            RadioButton RBE = new RadioButton();
            TableLayoutPanel TLP = new TableLayoutPanel();
            public override void LoadSettings()
            {
                TLP.Dock = DockStyle.Fill;
                TLP.AutoSize = true;
                TLP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                TLP.RowCount = 2;
                for (int i = 0; i < TLP.RowCount; i++) TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                {
                    RBS.AutoSize = true;
                    RBS.CheckedChanged += CB_SelectedIndexChanged;
                }
                TLP.Controls.Add(RBS); TLP.SetCellPosition(RBS, new TableLayoutPanelCellPosition(0, 0));
                {
                    RBE.AutoSize = true;
                    RBE.CheckedChanged += CB_SelectedIndexChanged;
                }
                TLP.Controls.Add(RBE); TLP.SetCellPosition(RBE, new TableLayoutPanelCellPosition(0, 1));
                this.Controls.Add(TLP);
            }
            void CB_SelectedIndexChanged(object sender, EventArgs e)
            {
                OnSettingChanged();
            }
        }
        unsafe partial class SendWithShiftEnterPanel : TwoOptionSettingPanel
        {
            public SendWithShiftEnterPanel(SettingForm parent)
                : base(parent, "Send texts with:", "Shift+Enter (\"Enter\" to insert a new line)", "Enter (\"Shift+Enter\" to insert a new line)", ref Environment.SendWithShiftEnter)
            {
            }
        }
        partial class HideFormInsteadOfMinimizePanel : TwoOptionSettingPanel
        {
            public HideFormInsteadOfMinimizePanel(SettingForm parent)
                : base(parent, "Action after inserting texts:", "Hide the form", "Minimize the form", ref Environment.HideFormInsteadOfMinimize)
            {
            }
        }
        partial class HotKeysListPanel : SettingPanel
        {
            partial class HotKeyOption:Panel
            {
                Button ADD_HOT_KEY_BUTTON;
                Control PARENT;
                public HotKeyOption(HotKeysListPanel parent)
                {
                    PARENT = parent;
                    this.SettingChanged += parent.OnSettingChanged;
                    //this.BackColor = Color.FromArgb(255, 255, 255);
                    this.Dock = DockStyle.Top;
                    this.AutoSize = true;
                    this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    ADD_HOT_KEY_BUTTON = new Button();
                    {
                        ADD_HOT_KEY_BUTTON.Text = "Add a Hot Key";
                        ADD_HOT_KEY_BUTTON.Dock = DockStyle.Fill;
                        ADD_HOT_KEY_BUTTON.AutoSize = true;
                        ADD_HOT_KEY_BUTTON.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        ADD_HOT_KEY_BUTTON.Click += AddHotKeyButton_Click;
                    }
                    this.Controls.Add(ADD_HOT_KEY_BUTTON);
                }
                public HotKeyOption(HotKeysListPanel parent,Keys _COMBO_KEY,Keys _HOT_KEY):this(parent)
                {
                    AddHotKeyButton_Click(ADD_HOT_KEY_BUTTON, null);
                    KeyDetected(null, new HotKeyEventArgs(_HOT_KEY,_COMBO_KEY));
                    BTN_V_Click(BTN_V, null);
                }
                public Keys COMBO_KEY = Keys.None, HOT_KEY = Keys.None;
                void DfsDisposeControls(Control ctrl)
                {
                    foreach (Control c in ctrl.Controls) DfsDisposeControls(c);
                    ctrl.Dispose();
                }
                List<HotKey> HOTKEYS = new List<HotKey>();
                Label LBL;
                Button BTN_X,BTN_V;
                GlobalKeysDetector KEY_DETECTOR;
                void AddHotKeyButton_Click(object sender, EventArgs e)
                {
                    this.Controls.Clear();
                    TableLayoutPanel tlp = new TableLayoutPanel();
                    tlp.Dock = DockStyle.Fill;
                    tlp.AutoSize = true;
                    tlp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    tlp.ColumnCount = 3;
                    tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                    tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                    tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                    {
                        int column_count = 0;
                        LBL = new Label();
                        {
                            LBL.Text = "Type your hotkey now...";
                            LBL.Dock = DockStyle.Fill;
                            LBL.AutoSize = true;
                            KEY_DETECTOR = new GlobalKeysDetector(Environment.MainForm.Handle);
                            KEY_DETECTOR.OnKeyDetected += new HotKey.HotkeyEventHandler(KeyDetected);
                        }
                        tlp.Controls.Add(LBL); tlp.SetCellPosition(LBL, new TableLayoutPanelCellPosition(column_count++, 0));
                        BTN_V = new Button();
                        {
                            BTN_V.Text = "√";
                            BTN_V.Dock = DockStyle.Fill;
                            BTN_V.AutoSize = true;
                            BTN_V.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                            BTN_V.Click += BTN_V_Click;
                        }
                        tlp.Controls.Add(BTN_V); tlp.SetCellPosition(BTN_V, new TableLayoutPanelCellPosition(column_count++, 0));
                        BTN_X = new Button();
                        {
                            BTN_X.Text = "X";
                            BTN_X.Dock = DockStyle.Fill;
                            BTN_X.AutoSize = true;
                            BTN_X.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                            BTN_X.Click += BTN_X_Click;
                            BTN_X.Enabled = false;
                        }
                        tlp.Controls.Add(BTN_X); tlp.SetCellPosition(BTN_X, new TableLayoutPanelCellPosition(column_count++, 0));
                    }
                    this.Controls.Add(tlp);
                    (sender as Button).Dispose();
                }
                private bool KEY_SET { get { return COMBO_KEY != Keys.None || HOT_KEY != Keys.None; } }
                bool _IS_READY = false;
                public bool IS_READY { get { return _IS_READY; } }
                void KeyDetected(object sender, HotKeyEventArgs e)
                {
                    //MessageBox.Show("pressed");
                    if (LBL == null) throw new Exception();
                    //if (!KEY_SET) this.Parent.Controls.Add(new HotKeyOption());//補上新增按鈕
                    COMBO_KEY = e.ComboKey;
                    HOT_KEY = e.HotKey;
                    //MessageBox.Show(COMBO_KEY.ToString() + "\r\n" + HOT_KEY.ToString());
                    LBL.Text = (COMBO_KEY == Keys.None ? "" :( COMBO_KEY.ToString() + " + ")) + HOT_KEY.ToString();
                }
                void BTN_X_Click(object sender, EventArgs e)
                {
                    OnSettingChanged();
                    this.Parent.Controls.Remove(this);
                    this.DfsDisposeControls(this);
                }
                void BTN_V_Click(object sender, EventArgs e)
                {
                    if(!KEY_SET)
                    {
                        MessageBox.Show("You haven't set the hot key");
                        return;
                    }
                    OnSettingChanged();
                    (sender as Button).Enabled = false;
                    _IS_READY = true;
                    BTN_X.Enabled = true;
                    KEY_DETECTOR.Dispose();
                    (this.PARENT as HotKeysListPanel).Controls.Add(new HotKeyOption(this.PARENT as HotKeysListPanel));
                }
                ~HotKeyOption()
                {
                    DfsDisposeControls(this);
                }
                public delegate void EventHandler();
                public event EventHandler SettingChanged;
                public void OnSettingChanged()
                {
                    if (SettingChanged != null) SettingChanged();
                }
            }
            public HotKeysListPanel(SettingForm parent)
                : base(parent)
            {
            }
            public override void SaveSettings()
            {
                //MessageBox.Show("Saving HotKey");
                foreach(HotKey hk in Environment.HOT_KEYS)hk.Dispose();
                Environment.HOT_KEYS.Clear();
                foreach (Control c in this.Controls)
                {
                    if (c.GetType() == typeof(HotKeyOption))
                    {
                        HotKeyOption ho=c as HotKeyOption;
                        if (ho.IS_READY)
                        {
                            HotKey hk = new HotKey(Environment.MainForm.Handle,ho.HOT_KEY, ho.COMBO_KEY);
                            Environment.HOT_KEYS.Add(hk);
                        }
                    }
                }
                Environment.RegisterHotKeys();
            }
            public override void LoadSettings()
            {
                this.Text = "Hot Keys";
                if (Environment.HOT_KEYS.Count > 0)
                {
                    foreach (HotKey hk in Environment.HOT_KEYS)
                    {
                        this.Controls.Add(new HotKeyOption(this, hk.GetComboKey, hk.GetHotKey));
                    }
                }
                else this.Controls.Add(new HotKeyOption(this));
            }
        }
        TableLayoutPanel TLP_MAIN = new TableLayoutPanel();
        TableLayoutPanel TLP_BTN = new TableLayoutPanel();
        Panel PNL_MAIN = new Panel();
        Button BTN_OK = new Button();
        Button BTN_APPLY = new Button();
        public SettingForm()
        {
            this.Text = "Settings";
            TLP_MAIN.Dock = DockStyle.Fill;
            TLP_MAIN.AutoSize = true;
            TLP_MAIN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TLP_MAIN.RowCount = 2;
            TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
            {
                PNL_MAIN.Dock = DockStyle.Fill;
                //PNL_MAIN.AutoSize = true;
                //PNL_MAIN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                PNL_MAIN.AutoScroll = true;
                PNL_MAIN.Controls.Add(new HotKeysListPanel(this));
                PNL_MAIN.Controls.Add(new SendWithShiftEnterPanel(this));
                PNL_MAIN.Controls.Add(new HideFormInsteadOfMinimizePanel(this));
            }
            TLP_MAIN.Controls.Add(PNL_MAIN); TLP_MAIN.SetCellPosition(PNL_MAIN, new TableLayoutPanelCellPosition(0, 0));
            {
                TLP_BTN.Dock = DockStyle.Fill;
                TLP_BTN.AutoSize = true;
                TLP_BTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                TLP_BTN.ColumnCount = 2;
                for (int i = 0; i < TLP_BTN.ColumnCount; i++) TLP_BTN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                {
                    BTN_OK.Text = "OK";
                    BTN_OK.Dock = DockStyle.Fill;
                    BTN_OK.AutoSize = true;
                    BTN_OK.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    BTN_OK.Click += BTN_OK_Click;
                }
                TLP_BTN.Controls.Add(BTN_OK); TLP_BTN.SetCellPosition(BTN_OK, new TableLayoutPanelCellPosition(0, 0));
                {
                    BTN_APPLY.Text = "Apply";
                    BTN_APPLY.Dock = DockStyle.Fill;
                    BTN_APPLY.AutoSize = true;
                    BTN_APPLY.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    BTN_APPLY.Click += BTN_APPLY_Click;
                }
                TLP_BTN.Controls.Add(BTN_APPLY); TLP_BTN.SetCellPosition(BTN_APPLY, new TableLayoutPanelCellPosition(1, 0));
            }
            TLP_MAIN.Controls.Add(TLP_BTN); TLP_MAIN.SetCellPosition(TLP_BTN, new TableLayoutPanelCellPosition(0, 1));
            this.Controls.Add(TLP_MAIN);
        }
        public void OnSettingChanged()
        {
            BTN_APPLY.Enabled = true;
        }
        void BTN_APPLY_Click(object sender, EventArgs e)
        {
            foreach (Control c in PNL_MAIN.Controls)
            {
                if (typeof(SettingPanel).IsAssignableFrom(c.GetType()))
                {
                    (c as SettingPanel).SaveSettings();
                }
            }
            BTN_APPLY.Enabled = false;
        }
        void BTN_OK_Click(object sender, EventArgs e)
        {
            BTN_APPLY_Click(BTN_APPLY, null);
            this.Close();
        }
    }
}
