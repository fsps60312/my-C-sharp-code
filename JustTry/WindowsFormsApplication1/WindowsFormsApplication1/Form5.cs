using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            label1.Text = "+";
            label2.Text = "=";
            label3.Text = "";
            button1.Text = "+";
            button2.Text = "-";
            button3.Text = "*";
            button4.Text = "/";
            button5.Text = "End";
        }
        private void button_Click(object sender,EventArgs e)
        {
            switch(Char.Parse((sender as Button).Text))
            {
                case '+':
                    {
                        try { label3.Text = (decimal.Parse(textBox1.Text) + decimal.Parse(textBox2.Text)).ToString(); }
                        catch (FormatException) { MessageBox.Show("格式錯誤"); }
                    } break;
                case '-':
                    {
                        try { label3.Text = (decimal.Parse(textBox1.Text) - decimal.Parse(textBox2.Text)).ToString(); }
                        catch (FormatException) { MessageBox.Show("格式錯誤"); }
                    } break;
                case '*':
                    {
                        try { label3.Text = (decimal.Parse(textBox1.Text) * decimal.Parse(textBox2.Text)).ToString(); }
                        catch (FormatException) { MessageBox.Show("格式錯誤"); }
                    } break;
                case '/':
                    {
                        try { label3.Text = (decimal.Parse(textBox1.Text) / decimal.Parse(textBox2.Text)).ToString(); }
                        catch (FormatException) { MessageBox.Show("格式錯誤"); }
                        catch (DivideByZeroException) { MessageBox.Show("除數為零"); }
                    } break;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
