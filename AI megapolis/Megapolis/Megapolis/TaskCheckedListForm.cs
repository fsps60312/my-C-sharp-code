using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motivation;

namespace Megapolis
{
    partial class TaskCheckedListForm : Form
    {
        public delegate void OKButtonClickedEventHandler(List<MyTask> tasks);
        public OKButtonClickedEventHandler OKButtonClicked;
        private void OnOKButtonClicked(List<MyTask> tasks) { OKButtonClicked?.Invoke(tasks); }
        public TaskCheckedListForm(List<MyTask> tasks)
        {
            this.Size = new Size(600, 600);
            {
                TLP = new MyTableLayoutPanel(2, 1, "PA", "P");
                {
                    CLB = new CheckedListBox();
                    CLB.Dock = DockStyle.Fill;
                    CLB.Font = DefaultSetting.fontE;
                    foreach (MyTask t in tasks) CLB.Items.Add(t);
                    TLP.AddControl(CLB, 0, 0);
                }
                {
                    BTN = new MyButton("Start");
                    BTN.Click += BTN_Click;
                    TLP.AddControl(BTN, 1, 0);
                }
                this.Controls.Add(TLP);
            }
        }
        private void BTN_Click(object sender, EventArgs e)
        {
            List<MyTask> answer = new List<MyTask>();
            foreach (MyTask t in CLB.CheckedItems)
            {
                answer.Add(t);
            }
            OnOKButtonClicked(answer);
            this.Close();
        }
        MyTableLayoutPanel TLP;
        CheckedListBox CLB;
        MyButton BTN;
    }
}
