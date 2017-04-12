using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C___Code_Reformator
{
    public partial class Form1 : Form
    {
        TextBox TXB = new TextBox();
        ToolStripMenuItem[] ITEM = new ToolStripMenuItem[7];
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormClosed += Form1_FormClosed;
            {
                TXB.Dock = DockStyle.Fill;
                TXB.Font = new Font("Arial", 20);
                TXB.Multiline = true;
                TXB.ScrollBars = ScrollBars.Both;
                TXB.ShortcutsEnabled = true;
                TXB.WordWrap = false;
                TXB.MaxLength = int.MaxValue;
                TXB.MouseDoubleClick += TXB_MouseDoubleClick;
                TXB.KeyDown += SelectAll;
                TXB.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                int idx = 0;
                ITEM[idx] = new ToolStripMenuItem("Reformat Code");
                ITEM[idx].Click += RefoamatCode;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Remove Line Number");
                ITEM[idx].Click += RemoveLineNumber;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Remove Left Number");
                ITEM[idx].Click += RemoveLeftNumber;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Remove Left Space");
                ITEM[idx].Click += RemoveLeftSpace;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Replace Left Space With Tab Or Remove");
                ITEM[idx].Click += ReplaceWithTabOrRemove;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Remove Right Space");
                ITEM[idx].Click += RemoveRightSpace;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
                ITEM[idx] = new ToolStripMenuItem("Split Input Data");
                ITEM[idx].Click += SplitInputData;
                TXB.ContextMenuStrip.Items.Add(ITEM[idx]);
                idx++;
            } this.Controls.Add(TXB);
        }
        private void ReplaceWithTabOrRemove(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.ReplaceWithTabOrRemove(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        private void SplitInputData(object sender, EventArgs e)
        {
            int start = TXB.SelectionLength > 0 ? TXB.SelectionStart : 0;
            string ans = Input_Data_Splitter.Reformat(TXB.Text.Substring(start));
            TXB.Text = TXB.Text.Remove(start) + ans;
        }
        private void RemoveLeftSpace(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.RemoveLeftSpace(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        private void RemoveRightSpace(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.RemoveRightSpace(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        private void RemoveLeftNumber(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.RemoveLeftNumber(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        private void RemoveLineNumber(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.RemoveLineNumber(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        int CNT = 0;
        void RefoamatCode(object sender, EventArgs e)
        {
            TXB.Text = My_CPP_Code.Reformat(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        void TXB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TXB.Text = My_CPP_Code.Reformat(TXB.Text, this);
            this.Text = "#" + CNT++.ToString();
        }
        void SelectAll(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
                e.SuppressKeyPress = true;
            }
        }
    }
}
