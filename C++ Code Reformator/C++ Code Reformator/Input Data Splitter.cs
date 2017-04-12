using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace C___Code_Reformator
{
    class Input_Data_Splitter
    {
        partial class InputRowColumn : Form
        {
            TableLayoutPanel TLP = new TableLayoutPanel();
            DomainUpDown[] DUD = new DomainUpDown[2];
            Button[] BTN = new Button[2];
            InputRowColumn()
            {
                this.Size = new Size(600, 200);
                TLP.ColumnCount = TLP.RowCount = 2;
                TLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                TLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                TLP.Dock = DockStyle.Fill;
                for (int i = 0; i < 2; i++)
                {
                    DUD[i] = new DomainUpDown();
                    DUD[i].Dock = DockStyle.Fill;
                    DUD[i].Font = new Font("微軟正黑體", 30);
                    TLP.Controls.Add(DUD[i]);
                    TLP.SetCellPosition(DUD[i], new TableLayoutPanelCellPosition(i, 0));
                    BTN[i] = new Button();
                    BTN[i].Dock = DockStyle.Fill;
                    BTN[i].Font = new Font("微軟正黑體", 30);
                    TLP.Controls.Add(BTN[i]);
                    TLP.SetCellPosition(BTN[i], new TableLayoutPanelCellPosition(i, 1));
                }
                BTN[0].Text = "OK";
                BTN[1].Text = "Cancel";
                BTN[0].Click += InputRowColumn_OK_Click;
                BTN[1].Click += InputRowColumn_Cancel_Click;
                this.Controls.Add(TLP);
            }
            private void InputRowColumn_Cancel_Click(object sender, EventArgs e)
            {
                result = DialogResult.Cancel;
            }
            private void InputRowColumn_OK_Click(object sender, EventArgs e)
            {
                if(!int.TryParse(DUD[0].Text,out OUTC)||!int.TryParse(DUD[1].Text,out OUTR))
                {
                    MessageBox.Show("Incorrect Format");
                    return;
                }
                result = DialogResult.OK;
            }
            DialogResult result = DialogResult.None;
            int OUTR, OUTC;
            DialogResult InputData(out int c, out int r)
            {
                result = DialogResult.None;
                this.Show();
                while (result == DialogResult.None) Application.DoEvents();
                c = OUTC;r = OUTR;
                this.Hide();
                return result;
            }
            static InputRowColumn MAIN = new InputRowColumn();
            public static DialogResult Show(out int c, out int r)
            {
                return MAIN.InputData(out c, out r);
            }
        }
        static bool Empty(char a) { return a == ' ' || a == '\r' || a == '\n' || a == '\t'; }
        public static string Reformat(string s)
        {
            int c, r;
            DialogResult result = InputRowColumn.Show(out c, out r);
            if (result == DialogResult.Cancel) return s;
            int idx = 0;
            StringBuilder ans = new StringBuilder();
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    while (idx < s.Length && Empty(s[idx])) idx++;
                    if (j > 0) ans.Append(' ');
                    while (idx < s.Length && !Empty(s[idx]))
                    {
                        ans.Append(s[idx]);
                        idx++;
                    }
                }
                ans.Append("\r\n");
            }
            while (idx < s.Length && (s[idx] < '0' || s[idx] > '9')) idx++;
            return ans.ToString()+s.Substring(idx);
        }
    }
}
