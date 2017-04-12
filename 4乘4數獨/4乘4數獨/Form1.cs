using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _4乘4數獨
{
    public partial class Form1 : Form
    {
        public struct sudo
        {
            public int[,] layout;
            public int count;
        }
        public TableLayoutPanel[] tlp = new TableLayoutPanel[2];
        public TextBox[,] txb = new TextBox[4, 4];
        public TextBox output = new TextBox();
        public Thread thread = null;
        public HashSet<string> solutions = new HashSet<string>();
        public Form1()
        {
            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < tlp.Length; i++) tlp[i] = new TableLayoutPanel();
            for (int i = 0; i < txb.GetLength(0); i++)
                for (int j = 0; j < txb.GetLength(1); j++)
                    txb[i, j] = new TextBox();
            this.Controls.Add(tlp[0]);
            tlp[0].Dock=DockStyle.Fill;
            tlp[0].RowCount = 1;
            tlp[0].ColumnCount = 2;
            tlp[0].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp[0].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp[0].Controls.Add(tlp[1]);
            tlp[0].Controls.Add(output);
            tlp[0].SetCellPosition(tlp[1], new TableLayoutPanelCellPosition(0, 0));
            {
                tlp[1].Dock = DockStyle.Fill;
                tlp[1].RowCount = 4;
                tlp[1].ColumnCount = 4;
                for(int i=0;i<tlp[1].ColumnCount;i++)
                    for(int j=0;j<tlp[1].RowCount;j++)
                    {
                        tlp[1].Controls.Add(txb[i, j]);
                        tlp[1].SetCellPosition(txb[i, j], new TableLayoutPanelCellPosition(i, j));
                        txb[i, j].Dock = DockStyle.Fill;
                        txb[i, j].Multiline = true;
                        txb[i, j].Font = new Font("標楷體", 20);
                        //txb[i, j].TextChanged += Form1_TextChanged;
                    }
                for(int i=0;i<4;i++)
                {
                    tlp[1].RowStyles.Add(new RowStyle(SizeType.Percent, 25));
                    tlp[1].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                }
            }
            tlp[0].SetCellPosition(output, new TableLayoutPanelCellPosition(1, 0));
            {
                output.Dock = DockStyle.Fill;
                output.Multiline = true;
                output.Font = new Font("標楷體", 20);
                output.ScrollBars = ScrollBars.Both;
                output.DoubleClick += Form1_TextChanged;
            }
            txb[0, 0].Text = "1";
            txb[2, 1].Text = "2";
            txb[1, 2].Text = "3";
            this.WindowState = FormWindowState.Maximized;
        }

        void Form1_TextChanged(object sender, EventArgs e)
        {
            int[,] layout = new int[4, 4];
            int count = 0;
            for(int i=0;i<4;i++)
                for(int j=0;j<4;j++)
                {
                    try
                    {
                        int k=int.Parse(txb[j, i].Text);
                        if (k <= 0 || k >= 5)
                        {
                            continue;
                        }
                        else layout[i, j] = 30;
                        layout[i, j] = 1 << k;
                        count++;
                    }
                    catch (Exception)
                    {
                        layout[i, j] = 30;//2+4+8+16=30
                    }
                }
            //if (count < 4) return;
            output.ResetText();
            solutions.Clear();
            if (thread != null) thread.Abort();
            thread = new Thread(() => 
            {
                this.Text = dfs(layout,0).ToString();
                string a = output.Text;
                a = a.Replace("\t", "").Replace("\r\n", "");
                for(int i=0;i<16;i++)
                {
                    bool[] visited = new bool[] { false, false, false, false };
                    bool ishint = true;
                    for(int j=i;j<a.Length;j+=16)
                    {
                        int k = a[j] - '1';
                        if (visited[k])
                        {
                            ishint = false;
                            break;
                        }
                        else visited[k] = true;
                    }
                    if(ishint)
                    {
                        output.AppendText(((char)('A' + i % 4)).ToString() + ((char)('1' + i / 4)).ToString() + "\r\n");
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        public int dfs(int[,] layout,int depth)
        {
            this.Text = depth.ToString();
            int count = 0;
            while (IsDifferent(layout, layout = fillsudo(layout)))
            {
                if (IsIllegal(layout)) return 0;
            }
            if (IsSolved(layout))
            {
                string a = export(layout);
                if (solutions.Contains(a)) return 0;
                else
                {
                    solutions.Add(a);
                    output.AppendText(a + "\r\n");
                    Application.DoEvents();
                    return 1;
                }
            }
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if (!IsSingle(layout[i, j]))
                    {
                        int[,] k = new int[4, 4];
                        for (int i0 = 0; i0 < 4; i0++)
                        {
                            for (int j0 = 0; j0 < 4; j0++)
                            {
                                k[i0, j0] = layout[i0, j0];
                            }
                        }
                        for (int l = 1; l <= 4; l++)
                        {
                            if ((layout[i, j] & (1 << l)) != 0)
                            {
                                k[i, j] = 1 << l;
                                count += dfs(k,depth+1);
                            }
                        }
                    }
                }
            }
            return count;
        }
        public string export(int[,] a)
        {
            string b = "";
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    for (int k = 1; k <= 4;k++ )
                    {
                        if ((a[i, j] & (1 << k)) != 0) b += k.ToString();
                    }
                    b += "\t";
                }
                b+="\r\n";
            }
            return b;
        }
        public bool IsSolved(int[,] a)
        {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if (!IsSingle(a[i, j])) return false;
                }
            }
            return true;
        }
        public bool IsIllegal(int[,] a)
        {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if (a[i, j] == 0) return true;
                }
            }
            return false;
        }
        public bool IsDifferent(int[,] a,int[,] b)
        {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if (a[i, j] != b[i, j]) return true;
                }
            }
            return false;
        }
        public int[,] fillsudo(int[,] layout)
        {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if (IsSingle(layout[i, j]))
                    {
                        int r = i % 2, l = j % 2, R = i - r, L = j - l, rl = r * 2 + l;
                        for (int k = 0; k < 4; k++)
                        {
                            if (k != i)
                            {
                                layout[k, j] &= ~layout[i, j];
                            }
                            if (k != j)
                            {
                                layout[i, k] &= ~layout[i, j];
                            }
                            if (k != rl)
                            {
                                layout[R + k / 2, L + k % 2] &= ~layout[i, j];
                            }
                        }
                    }
                }
            }
            return layout;
        }
        public bool IsSingle(int a)
        {
            if (a == 1 << 1 || a == 1 << 2 || a == 1 << 3 || a == 1 << 4) return true;
            else return false;
        }
    }
}
