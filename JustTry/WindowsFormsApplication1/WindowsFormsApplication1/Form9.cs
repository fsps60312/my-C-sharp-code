using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication1
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        decimal a, b;
        private void Form9_Load(object sender, EventArgs e)
        {
            while(true)
            {
                try
                {
                    a = decimal.Parse(Interaction.InputBox("請輸入第一個數", "第一個數", "請輸入一個數"));
                    b = decimal.Parse(Interaction.InputBox("請輸入第二個數", "第二個數", "請輸入一個數"));
                }
                catch(FormatException)
                {
                    MessageBox.Show("輸入格式錯誤，即將退回主畫面");
                    this.Close();
                    return;
                }
                MessageBox.Show((a + b).ToString(), "相加結果");
            }
        }
    }
}
