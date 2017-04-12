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
using System.IO;
using System.Diagnostics;

namespace English_Report_An_Interesting_Experiment
{
    public partial class Form1 : Form
    {
        TableLayoutPanel TLP = new TableLayoutPanel();
        Button START = new Button();
        Label[] LAB = new Label[6];
        ProgressPBX[] PBX = new ProgressPBX[6];
        static void Swap(ref int a, ref int b) { int c = a; a = b; b = c; }
        int[] DATA;
        double SelectionSort(int[] _data, int L)
        {
            DATA = new int[L];
            for (int i = 0; i < L; i++) DATA[i] = _data[i];
            DateTime start = DateTime.Now;
            for (int i = 0; i < L; i++)
            {
                int k = i;
                for (int j = i + 1; j < L; j++)
                {
                    if (DATA[j] < DATA[k]) k = j;
                }
                Swap(ref DATA[i], ref DATA[k]);
            }
            return (DateTime.Now - start).TotalMilliseconds;
        }
        double BubbleSort(int[] _data, int L)
        {
            DATA = new int[L];
            for (int i = 0; i < L; i++) DATA[i] = _data[i];
            DateTime start = DateTime.Now;
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 1; i < L; i++)
                {
                    if (DATA[i - 1] > DATA[i])
                    {
                        Swap(ref DATA[i - 1], ref DATA[i]);
                        changed = true;
                    }
                }
            }
            return (DateTime.Now - start).TotalMilliseconds;
        }
        double InsertionSort(int[] _data, int L)
        {
            DATA = new int[L];
            DateTime start = DateTime.Now;
            for (int i = 0; i < L; i++)
            {
                int j;
                for (j = i - 1; j >= 0 && DATA[j] > _data[i]; j--) ;
                j++;
                for (int k = i - 1; k >= j; k--) DATA[k + 1] = DATA[k];
                DATA[j] = _data[i];
            }
            return (DateTime.Now - start).TotalMilliseconds;
        }
        int[] TARAY;
        void dfs_Merge(int[] data, int l, int r)
        {
            if (l == r) return;
            int mid = (l + r) / 2;
            dfs_Merge(data, l, mid);
            dfs_Merge(data, mid + 1, r);
            int a = l, b = mid + 1;
            for (int i = l; i <= r; i++)
            {
                if (b > r || (a <= mid && data[a] < data[b])) TARAY[i] = data[a];
                else TARAY[i] = data[b];
            }
            for (int i = l; i <= r; i++) data[i] = TARAY[i];
        }
        double MergeSort(int[] _data, int L)
        {
            TARAY = new int[L];
            DATA = new int[L];
            for (int i = 0; i < L; i++) DATA[i] = _data[i];
            DateTime start = DateTime.Now;
            dfs_Merge(DATA, 0, L - 1);
            return (DateTime.Now - start).TotalMilliseconds;
        }
        struct Heap
        {
            static int[] V;
            static int SZ;
            public static void Init(int[] source, int L)
            {
                SZ = L;
                V = new int[SZ + 1];
                for (int i = 0; i < SZ; i++)
                {
                    V[i + 1] = source[i];
                    int j = i + 1;
                    while (j > 1 && V[j] < V[j / 2])
                    {
                        Swap(ref V[j], ref V[j / 2]);
                        j /= 2;
                    }
                }
            }
            public static int Min() { return V[1]; }
            public static void Pop()
            {
                V[1] = V[SZ];
                SZ--;
                int i = 1;
                while (i * 2 + 1 <= SZ && V[i] > Math.Min(V[i * 2], V[i * 2 + 1]))
                {
                    if (V[i * 2] < V[i * 2 + 1])
                    {
                        Swap(ref V[i], ref V[i * 2]);
                        i = i * 2;
                    }
                    else
                    {
                        Swap(ref V[i], ref V[i * 2 + 1]);
                        i = i * 2 + 1;
                    }
                }
                if (i * 2 <= SZ && V[i] > V[i * 2]) Swap(ref V[i], ref V[i * 2]);
            }
        }
        double HeapSort(int[] _data, int L)
        {
            DATA = new int[L];
            DateTime start = DateTime.Now;
            Heap.Init(_data, L);
            for (int i = 0; i < L; i++)
            {
                DATA[i] = Heap.Min();
                Heap.Pop();
            }
            return (DateTime.Now - start).TotalMilliseconds;
        }
        void dfs_Quick(int[] data, int l, int r)
        {
            if (l >= r) return;
            int mv = data[l];
            int mi = l;
            Swap(ref data[l], ref data[r]);
            for (int i = l; i < r; i++)
            {
                if (data[i] < mv)
                {
                    Swap(ref data[mi], ref data[i]);
                    mi++;
                }
            }
            Swap(ref data[mi], ref data[r]);
            dfs_Quick(data, l, mi - 1);
            dfs_Quick(data, mi + 1, r);
        }
        double QuickSort(int[] _data, int L)
        {
            DATA = new int[L];
            for (int i = 0; i < L; i++) DATA[i] = _data[i];
            DateTime start = DateTime.Now;
            dfs_Quick(DATA, 0, L - 1);
            return (DateTime.Now - start).TotalMilliseconds;
        }
        partial class ProgressPBX : Panel
        {
            int L;
            double[] Value;
            TableLayoutPanel TLP = new TableLayoutPanel();
            ProgressBar[] BAR;
            public ProgressPBX(int l)
            {
                L = l;
                Value = new double[L];
                BAR = new ProgressBar[L];
                this.Controls.Add(TLP);
                {
                    TLP.Dock = DockStyle.Fill;
                    TLP.RowCount = L;
                    for (int i = 0; i < L; i++)
                    {
                        TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                        BAR[i] = new ProgressBar();
                        TLP.Controls.Add(BAR[i]); TLP.SetCellPosition(BAR[i], new TableLayoutPanelCellPosition(0, i));
                        {
                            BAR[i].Dock = DockStyle.Fill;
                        }
                    }
                }
            }
            public void SetValue(int i, double v)
            {
                Value[i] = v;
            }
            public void UpdateLayout()
            {
                this.SuspendLayout();
                int W = this.Width;
                for (int i = 0; i < L; i++)
                {
                    BAR[i].Value = Math.Min(BAR[i].Maximum, (int)(W * Value[i]));
                    BAR[i].Maximum = W;
                    BAR[i].Value = (int)(W * Value[i]);
                }
                this.ResumeLayout();
            }
        }
        List<double>[] VALUE = new List<double>[6];
        Random RAND = new Random();
        bool IsSorted()
        {
            for (int i = 1; i < DATA.Length; i++)
            {
                if (DATA[i - 1] > DATA[i]) return false;
            }
            return true;
        }
        double RunTime(int i, int[] data, int L)
        {
            switch (i)
            {
                case 0: return SelectionSort(data, L);
                case 1: return BubbleSort(data, L);
                case 2: return InsertionSort(data, L);
                case 3: return MergeSort(data, L);
                case 4: return HeapSort(data, L);
                case 5: return QuickSort(data, L);
                default: throw new NotImplementedException();
            }
        }
        void START_Click(object sender, EventArgs e)
        {
            string[] SortAlgorithm = new string[] { "SelectionSort", "BubbleSort", "InsertionSort", "MergeSort", "HeapSort", "QuickSort" };
            for (int i = 0; i < 6; i++)
            {
                StreamWriter writer = new StreamWriter(SortAlgorithm[i] + ".txt", false);
                double v = 0;
                int addv = 10;
                for (int L = 10; v < 1000; L+=addv)
                {
                    if (L >= 100) addv = (int)Math.Sqrt(L);
                    int[] data = new int[L];
                    for (int j = 0; j < L; j++) data[j] = RAND.Next();
                    v=RunTime(i, data, L);
                    writer.WriteLine(L.ToString() + "\t" + v.ToString());
                    this.Text = SortAlgorithm[i] + " : " + v.ToString() + " s";
                    PBX[i].SetValue(0, Math.Min(1, v / 1000));
                    PBX[i].UpdateLayout();
                    Application.DoEvents();
                }
                writer.Close();
            }
            this.Text = "Done";
        }
        public Form1()
        {
            //MessageBox.Show(RAND.Next().ToString());
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(TLP);
            TLP.Dock = DockStyle.Fill;
            TLP.RowCount = 1 + 6 * 2;
            TLP.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
            TLP.Controls.Add(START);
            TLP.SetCellPosition(START, new TableLayoutPanelCellPosition(0, 0));
            START.Dock = DockStyle.Fill;
            START.Text = "Start";
            START.Click += START_Click;
            string[] Texts = new string[] { "Selection Sort", "Bubble Sort", "Insertion Sort", "Merge Sort", "Binery Tree Sort", "Quick Sort" };
            for (int i = 0; i < 6; i++)
            {
                TLP.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                LAB[i] = new Label();
                PBX[i] = new ProgressPBX(1);
                TLP.Controls.Add(LAB[i]);
                TLP.Controls.Add(PBX[i]);
                TLP.SetCellPosition(LAB[i], new TableLayoutPanelCellPosition(0, i * 2 + 1));
                LAB[i].Dock = DockStyle.Fill;
                LAB[i].Text = Texts[i];
                TLP.SetCellPosition(PBX[i], new TableLayoutPanelCellPosition(0, i * 2 + 2));
                PBX[i].Dock = DockStyle.Fill;
            }
            this.FormClosing += Form1_FormClosing;
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
