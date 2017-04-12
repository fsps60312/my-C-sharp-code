using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace draw_with_pencil
{
    public partial class Form1 : Form
    {
        int a, b, c; //每一行的石頭數
        int acon, bcon, ccon; //一開始每一行的石頭數
        //int trya, tryb, tryc;//減法的時候檢查會不會小於零
        int count1=0, count2=0, count3=0; //數每一行拿了幾個
        int mnsa, mnsb, mnsc;//玩家每一行要減幾個
        int commnsa, commnsb, commnsc;//電腦每一行要拿幾個


        Random rand = new Random();
        int[] lose = new int[1300];

        Label label1 = new Label();
        Label label2 = new Label();
        Label label3 = new Label(); //顯示a,b,c

        TextBox textbox1 = new TextBox();
        TextBox textbox2 = new TextBox();
        TextBox textbox3 = new TextBox();//輸入要拿幾個

        PictureBox playingbutton = new PictureBox();
        PictureBox computer = new PictureBox();//電腦的人物圖片
        PictureBox player = new PictureBox();//玩家人物的圖片
        PictureBox computerwarn = new PictureBox();//電腦會說的錯誤訊息

        Charactor gongching=new Charactor();//腳色1小工
        Charactor aixi = new Charactor();//腳色2小愛
        Charactor xialing = new Charactor();//腳色3小夏

        Charactor[] fragment = new Charactor[500];
        
        Charactor[] computerremove = new Charactor[500];//電腦得到的碎片
        Charactor[] playerremove = new Charactor[500];//玩家得到的碎片

        int yellow=0,blue=0,violet=0,red=0,green=0;//蒐集到的碎片數

        List<PictureBox>[] computerstone = new List<PictureBox>[3] { new List<PictureBox>(), new List<PictureBox>(), new List<PictureBox>() };
        List<PictureBox>[] playerstone = new List<PictureBox>[3] { new List<PictureBox>(), new List<PictureBox>(), new List<PictureBox>() };


        public Form1()
        {
            this.DoubleBuffered = true;
            //computerstone[0] = new List<PictureBox>();
            computerstone[0].Add(computer);
            computerstone[0].Clear();
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            //this.TopMost = true;*/
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackgroundImage = Image.FromFile("白底.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Load += Form_Load;
            
            int count = 0;
            StreamReader reader = new StreamReader("rock2.txt", Encoding.Default);
            while (!reader.EndOfStream)
            {
                string read = reader.ReadLine();
                lose[count++] = Int32.Parse(read);
            }
            reader.Close();
            /*List<int> s = new List<int>();
            for (int i = 0; i < 2; i++) s.Add(3);
            MessageBox.Show(s.Count.ToString());*/
        }

        public void startbutton_Click(object sender, EventArgs e)
        {
            startpanel.Visible = false;

            if (levelchoose == 1)   { level1(); }
            if (levelchoose == 2)   { level2(); }
            if (levelchoose == 3)   { level3(); }
        }

        public void level1()
        { 
            MessageBox.Show("Level 1.");

            a = rand.Next(3, 10);
            b = rand.Next(3, 10);
            c = rand.Next(3, 10);

            acon = a; bcon = b; ccon = c;//把初始值丟給常數

            label1.Text = a.ToString();
            drawing(1, a);

            label2.Text = b.ToString();
            drawing(101, b);

            label3.Text = c.ToString();
            drawing(201, c);
        }

        public void level2()
        { MessageBox.Show("Level 2."); level1(); }
        public void level3()
        { MessageBox.Show("Level 3."); level1(); }

        public void drawing(int start, int number)
        {
            for (int i = start; i < start + number; i++)
            {
                fragment[i] = new Charactor();
                string pic_name = "";

                if (start == 1)
                {
                    fragment[i].pic_number = i;
                    if (number % 2 == 0)
                        fragment[i].Left = (500 - (number / 2) * 56) + 56 * (i - 1);
                    else
                        fragment[i].Left = (500 - (number / 2) * 56 - 56 / 2) + 56 * (i - 1);
                    fragment[i].pic_visit = 0;
                    pic_name = "畫紅碎.png";

                    fragment[i].Top = 60;
                    fragment[i].Height = 50;
                    fragment[i].Width = 50;
                    fragment[i].Click += new System.EventHandler(click1);
                }
                else if (start == 101)
                {
                    fragment[i].pic_number = i;
                    if (number % 2 == 0)
                        fragment[i].Left = (500 - (number / 2) * 56) + 56 * (i - 101);
                    else
                        fragment[i].Left = (500 - (number / 2) * 56 - 56 / 2) + 56 * (i - 101);
                    fragment[i].pic_visit = 0;
                    pic_name = "畫紫碎.png";

                    fragment[i].Top = 170;
                    fragment[i].Height = 50;
                    fragment[i].Width = 50;
                    fragment[i].Click += new System.EventHandler(click2);
                }
                else if (start == 201)
                {
                    fragment[i].pic_number = i;
                    if (number % 2 == 0)
                        fragment[i].Left = (500 - (number / 2) * 56) + 56 * (i - 201);
                    else
                        fragment[i].Left = (500 - (number / 2) * 56 - 56 / 2) + 56 * (i - 201);
                    fragment[i].pic_visit = 0;
                    pic_name = "畫藍碎.png";

                    fragment[i].Top = 280;
                    fragment[i].Height = 50;
                    fragment[i].Width = 50;
                    fragment[i].Click += new System.EventHandler(click3);
                }
                fragment[i].Visible = true;
                fragment[i].SizeMode = PictureBoxSizeMode.Zoom;
                fragment[i].Image = Image.FromFile(pic_name);
                fragment[i].BackColor = Color.Transparent;
                Controls.Add(fragment[i]);
            }
            textbox1.Visible = true;
            textbox2.Visible = true;
            textbox3.Visible = true;

            

            playingbutton.Visible = true;
        }
        private void click1(object sender, System.EventArgs e)
        {
            int r, visit; //圖片編號 & 是否點過
            Charactor pbx = sender as Charactor;
            r = pbx.pic_number;

            visit = pbx.pic_visit;

            if (visit != 1)
            {
                fragment[r].pic_visit = 1;
                fragment[r].Image = Image.FromFile("取紅碎.png");
                count1++;
            }
            else
            {
                fragment[r].pic_visit = 0;
                fragment[r].Image = Image.FromFile("畫紅碎.png");
                count1--;
            }
            textbox1.Text = count1.ToString();
        }
        private void click2(object sender, System.EventArgs e)
        {
            int r, visit; //圖片編號 & 是否點過
            Charactor pbx = sender as Charactor;
            r = pbx.pic_number;

            visit = pbx.pic_visit;
            if (visit != 1)
            {
                fragment[r].pic_visit = 1;
                fragment[r].Image = Image.FromFile("取紫碎.png");
                count2++;
            }
            else
            {
                fragment[r].pic_visit = 0;
                fragment[r].Image = Image.FromFile("畫紫碎.png");
                count2--;
            }
            textbox2.Text = count2.ToString();
        }
        public void click3(object sender, System.EventArgs e)
        {
            int r, visit; //圖片編號 & 是否點過
            Charactor pbx = sender as Charactor;
            r = pbx.pic_number;

            visit = pbx.pic_visit;
            if (visit != 1)
            {
                fragment[r].pic_visit = 1;
                fragment[r].Image = Image.FromFile("取藍碎.png");
                count3++;
            }
            else
            {
                fragment[r].pic_visit = 0;
                fragment[r].Image = Image.FromFile("畫藍碎.png");
                count3--;
            }
            textbox3.Text = count3.ToString();
        }

        

        public void playingbutton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("playingbutton_Click");
            if (int.TryParse(textbox1.Text, out mnsa) == false) mnsa = 0;
            else mnsa = int.Parse(textbox1.Text);
            if (int.TryParse(textbox2.Text, out mnsb) == false) mnsb = 0;
            else mnsb = int.Parse(textbox2.Text);
            if (int.TryParse(textbox3.Text, out mnsc) == false) mnsc = 0;
            else mnsc = int.Parse(textbox3.Text);
            //把textbox的數字存給mns
            bool has_error = false;
            if (mnsa == 0 && mnsb == 0 && mnsc == 0) 
            {
                MessageBox.Show("Wrong Input 1.");
                has_error = true;
            }
            else if (mnsa == 0 && mnsb != 0 && mnsc != 0&&mnsb != mnsc) 
            {
                MessageBox.Show("Wrong Input 2.");
                has_error = true;
            }
            else if (mnsb == 0 && mnsa != 0 && mnsc != 0&&mnsa != mnsc) 
            {
                MessageBox.Show("Wrong Input 3.");
                has_error = true;
            }
            else if (mnsc == 0 && mnsa != 0 && mnsb != 0 && mnsa != mnsb)
            {
                MessageBox.Show("Wrong Input 4.");
                has_error = true;
            }

            else if (mnsa != 0 && mnsb != 0 && mnsc != 0)
            {
                if ((mnsa != mnsb) || (mnsb != mnsc) || (mnsa != mnsc))
                {
                    MessageBox.Show("Wrong Input 5.");
                    has_error = true;
                }
            }

            /*trya = a - mnsa; tryb = b - mnsb; tryc = c - mnsc;
            if ((trya < 0) || (tryb < 0) || (tryc < 0))
            {
                MessageBox.Show("Wrong Input 6.");
                mnsa = 0; mnsb = 0; mnsc = 0;
                count1 = 0; count2 = 0; count3 = 0;
                textbox1.Text = ""; textbox2.Text = ""; textbox3.Text = "";
                return;
            }*/
            if (has_error)
            {
                mnsa = 0; mnsb = 0; mnsc = 0;
                count1 = 0; count2 = 0; count3 = 0;
                wrongdrawing(acon, bcon, ccon);
                textbox1.Text = ""; textbox2.Text = ""; textbox3.Text = "";
                return;
            }
            //以上是各種不合規定的輸入

            a -= mnsa; b -= mnsb; c -= mnsc;//輸入可以，執行減法
            redrawing(acon, bcon, ccon);//拿掉被選掉的碎片

            //莫帝威
            playingbutton.Enabled = false;
            ProgressBar pgb = new ProgressBar();
            pgb.Size = new Size(500, 20);
            pgb.Maximum = 1000;
            pgb.Location = new Point((this.ClientSize.Width - pgb.Width)/2, 10);
            pgb.Value = 0;
            this.Controls.Add(pgb);
            Application.DoEvents();
            Label lab = new Label();
            lab.Text = "Thinking...";
            lab.Location = new Point(pgb.Left - lab.Width-50, pgb.Top);
            lab.Font = new Font("Consolas", 15, FontStyle.Italic);
            this.Controls.Add(lab);
            DateTime starttime = DateTime.Now;
            for (double t; (t = (DateTime.Now - starttime).TotalSeconds) < 3.0; )
            {
                pgb.Value = (int)(1000 * t / 3.0);
                Application.DoEvents();
            }
            pgb.Value = pgb.Maximum;
            //莫帝威

            label1.Text = a.ToString();
            label2.Text = b.ToString();
            label3.Text = c.ToString();//label的數字更新
            count1 = 0; count2 = 0; count3 = 0;//把計算拿取數量的值歸零


            if (a + b + c == 0)
            {
                //莫帝威
                lab.Text = "Oh no!";
                MessageBox.Show("Oh no!");
                this.Controls.Remove(pgb);
                this.Controls.Remove(lab);
                playingbutton.Enabled = true;
                //莫帝威

                DialogResult yesno = MessageBox.Show("You win! Play again?", "winner message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yesno == DialogResult.Yes) brandnew();
                else this.Close();
            }//玩家贏，是否重來?

            mnsa = 0; mnsb = 0; mnsc = 0;//每一行減幾個歸零
            textbox1.Text = count1.ToString(); 
            textbox2.Text = count2.ToString(); 
            textbox3.Text = count3.ToString();

            //

            int[] candidate = new int[1000000];//現在的狀況下的所有候選人
            int[] stage = new int[3];//現在的狀況
            stage[0] = a; stage[1] = b; stage[2] = c;//把現在狀況塞進stage
            int candidate_count;//候選人個數
            candidate_count = gen_next(a, b, c, candidate);//把候選人塞進candidate

            Array.Sort(stage);//把現在狀況排序
            int check = stage[0] * 1000000 + stage[1] * 1000 + stage[2];
                                                           //檢查現在狀況的輸贏

            if (findinglose(check))//true=現在輸了 所有候選人都是win
            {
                int ranchoose = rand.Next(0, candidate_count - 1);//隨便選個候選人
                int newa = candidate[ranchoose] / 1000000;
                int newb = candidate[ranchoose] / 1000 % 1000;
                int newc = candidate[ranchoose] % 1000;
                                         //電腦拿走之後想要變成"幾"個
                int i,count;
                commnsa = a - newa;
                commnsb = b - newb;
                commnsc = c - newc;//電腦每一行想要拿走幾個

                string compute = "第一行拿走" + (commnsa) + "個\n";
                compute += "第二行拿走" + (commnsb) + "個\n";
                compute += "第三行拿走" + (commnsc) + "個\n";
                //MessageBox.Show(compute);

                count = 0;
                for (i = 1; i < 1 + acon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsa)
                        {count++;  fragment[i].pic_visit = 1;}
                    }

                }
                count = 0;
                for (i = 101; i < 101 + bcon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsb)
                        {count++;  fragment[i].pic_visit = 1;}
                    }

                }
                count = 0;
                for (i = 201; i < 201 + ccon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsc)
                        {count++;  fragment[i].pic_visit = 1;}
                    }

                }


                //莫帝威
                lab.Text = "Ahha!";
                MessageBox.Show("Ahha!");
                this.Controls.Remove(pgb);
                this.Controls.Remove(lab);
                playingbutton.Enabled = true;
                //莫帝威


                redrawing(acon, bcon, ccon);//電腦重畫

                a = newa; b = newb; c = newc;

                label1.Text = a.ToString();
                label2.Text = b.ToString();
                label3.Text = c.ToString();

                if (a + b + c == 0)
                {
                    DialogResult yesno = MessageBox.Show("Computer win! Play again?", "loser message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yesno == DialogResult.Yes) brandnew();
                    else this.Close();
                }
            }
            else
            {
                int losenum = 0;//所有候選人之中為lose的個數
                int i;
                int[] losecandidate = new int[250];//裝lose候選人的陣列
                for (i = 0; i < candidate_count; i++)
                {
                    int x1 = candidate[i] / 1000000;
                    int y1 = candidate[i] / 1000 % 1000;
                    int z1 = candidate[i] % 1000;
                    
                    int[] s = new int[3];
                    s[0] = x1; s[1] = y1; s[2] = z1;//把每個候選人丟到陣列裡
                    Array.Sort(s);
                    int newcheck = s[0] * 1000000 + s[1] * 1000 + s[2];
                                                    //檢查該候選人是不是lose
                    if (findinglose(newcheck))
                    {
                        losecandidate[losenum++] = x1 * 1000000 + y1 * 1000 + z1;
                    } //如果是lose,丟進losecandidate,losenum++
                }
                int ranchoose = rand.Next(0, losenum - 1);//從lose候選人中隨便選
                int newa = losecandidate[ranchoose] / 1000000;
                int newb = losecandidate[ranchoose] / 1000 % 1000;
                int newc = losecandidate[ranchoose] % 1000;


                commnsa = a - newa;
                commnsb = b - newb;
                commnsc = c - newc;//電腦想要拿走幾個
                string compute = "第一行拿走" + (commnsa) + "個\n";
                compute += "第二行拿走" + (commnsb) + "個\n";
                compute += "第三行拿走" + (commnsc) + "個\n";
                //MessageBox.Show(compute);

                int count = 0;//數電腦已經拿幾個
                for (i = 1; i < 1 + acon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsa)
                        {count++;  fragment[i].pic_visit = 1;}
                    }

                }
                count = 0;
                for (i = 101; i < 101 + bcon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsb)
                        {count++; fragment[i].pic_visit = 1;}
                    }

                }
                count = 0;
                for (i = 201; i < 201 + ccon; i++)
                {
                    if (fragment[i].Visible && fragment[i].pic_visit == 0)
                    {
                        if (count < commnsc)
                        {count++; fragment[i].pic_visit = 1;}
                    }

                }

                
                //莫帝威
                lab.Text = "Ahha!";
                MessageBox.Show("Ahha!");
                this.Controls.Remove(pgb);
                this.Controls.Remove(lab);
                playingbutton.Enabled = true;
                //莫帝威

                redrawing(acon, bcon, ccon);//電腦重畫


                a = newa; b = newb; c = newc;

                label1.Text = a.ToString();
                label2.Text = b.ToString();
                label3.Text = c.ToString();

                if (a + b + c == 0)
                {
                    DialogResult yesno = MessageBox.Show("Computer win! Play again?", "loser message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yesno == DialogResult.Yes) brandnew();
                    else this.Close();
                }
            }
        }
        
        private int gen_next(int a, int b, int c, int[] candidate)
        {
            int can_count = 0;//數候選人幾個
            int i;

            for (i = a; i > 0; i--) //a慢慢減1
            { candidate[can_count++] = (i - 1) * 1000000 + (b * 1000) + (c); }

            for (i = b; i > 0; i--) //b減1
            {
                candidate[can_count++] = a * 1000000 + (i - 1) * 1000 + c;
            }

            for (i = c; i > 0; i--) //c減1
            {
                candidate[can_count++] = a * 1000000 + b * 1000 + (i - 1);
            }

            int aa = a, bb = b, cc = c;
            for (i = Math.Min(a, b); i > 0; i--)//a&b同時減
            {
                candidate[can_count++] = (--aa * 1000000) + (--bb * 1000) + cc;
            }

            aa = a; bb = b; cc = c;
            for (i = Math.Min(a, c); i > 0; i--)//a&c同時減
            {
                candidate[can_count++] = --aa * 1000000 + b * 1000 + --cc;
            }

            aa = a; bb = b; cc = c;
            for (i = Math.Min(b, c); i > 0; i--)//b&c同時減
            {
                candidate[can_count++] = aa * 1000000 + --bb * 1000 + --cc;
            }

            aa = a; bb = b; cc = c;
            for (i = Math.Min(Math.Min(a, b), c); i > 0; i--)//a&b&c同時減
            {
                candidate[can_count++] = --aa * 1000000 + --bb * 1000 + --cc;
            }

            return can_count;
        }

        public bool findinglose(int check)
        {
            for (int i = 0; i < 1264; i++)
            {
                if (lose[i] == check) return true;
            }
            return false;
        }

        public void redrawing(int a, int b, int c)
        {
            //MessageBox.Show(a.ToString() + " " + b.ToString() + " " + c.ToString());
            for (int i = 1; i < 1 + a; i++)
            { if (fragment[i].Visible && fragment[i].pic_visit == 1) fragment[i].Visible = false; }
            for (int i = 101; i < 101 + b; i++)
            { if (fragment[i].Visible && fragment[i].pic_visit == 1) fragment[i].Visible = false; }
            for (int i = 201; i < 201 + c; i++)
            { if (fragment[i].Visible && fragment[i].pic_visit == 1) fragment[i].Visible = false; }
        }

        public void wrongdrawing(int a, int b, int c)
        {
            for(int i=1;i<1+a;i++)
            {
                if(fragment[i].Visible)
                {
                    fragment[i].pic_visit=0; 
                    fragment[i].Image=Image.FromFile("畫紅碎.png");
                }
            }
            for(int i=101;i<101+b;i++)
            {
                if(fragment[i].Visible)
                {
                    fragment[i].pic_visit=0; 
                    fragment[i].Image=Image.FromFile("畫紫碎.png");
                }
            }
            for(int i=201;i<201+c;i++)
            {
                if(fragment[i].Visible)
                {
                    fragment[i].pic_visit=0; 
                    fragment[i].Image=Image.FromFile("畫藍碎.png");
                }
            }
        }

        
        public void Form_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("first");
            brandnew();
            //MessageBox.Show("Form_Load");
            createstartpanel();
        }
        public void brandnew()
        {
            reform();
            //this.SuspendLayout();
            basicll.Image = Image.FromFile("basic.png");
            interll.Image = Image.FromFile("intermediate.png");
            advanll.Image = Image.FromFile("advanced.png");
            startpanel.Visible = true;
            textbox1.Visible = false;
            textbox2.Visible = false;
            textbox3.Visible = false;

            label1.Size = new Size(70, 40);
            label1.Location = new Point(176, 110);
            label1.BackColor = Color.Transparent;
            label2.Size = new Size(70, 40);
            label2.Location = new Point(176, 212);
            label2.BackColor = Color.Transparent;
            label3.Size = new Size(70, 40);
            label3.Location = new Point(176, 319);
            label3.BackColor = Color.Transparent;

            textbox1.Size = new Size(80, 40);
            textbox1.Location = new Point(900, 99);
            textbox2.Size = new Size(80, 40);
            textbox2.Location = new Point(900, 201);
            textbox3.Size = new Size(80, 40);
            textbox3.Location = new Point(900, 308);

            textbox1.Text = ""; textbox2.Text = ""; textbox3.Text = "";


            playingbutton.Image = Image.FromFile("ok鈕.jpg");
            playingbutton.Size = new Size(80, 70);
            playingbutton.Location = new Point(900, 409);
            playingbutton.SizeMode = PictureBoxSizeMode.Zoom;
            playingbutton.Visible = false;

            this.Controls.Add(playingbutton);
            //playingbutton.Click+=playingbutton_Click;
            //this.ResumeLayout();
            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(textbox1);
            this.Controls.Add(textbox2);
            this.Controls.Add(textbox3);


        }
        public void reform()
        {
            levelchoose = 0;
            mnsa = 0; mnsb = 0; mnsc = 0;
            //count1 = count2 = count3 = 0;
        }
        Panel startpanel = new Panel();
        PictureBox gametitle = new PictureBox();
        PictureBox penera = new PictureBox();
        PictureBox levell = new PictureBox();
        PictureBox basicll = new PictureBox();
        PictureBox interll = new PictureBox();
        PictureBox advanll = new PictureBox();
        PictureBox startbutton = new PictureBox();
        int levelchoose=0; //選擇等級
        
        public void createstartpanel()
        {
            this.Controls.Add(startpanel);
            startpanel.Controls.Add(gametitle);
            startpanel.Controls.Add(penera);
            startpanel.Controls.Add(levell);
            startpanel.Controls.Add(basicll);
            startpanel.Controls.Add(interll);
            startpanel.Controls.Add(advanll);
            startpanel.Controls.Add(startbutton);

            startpanel.Size = new Size(1920, 1080);
            startpanel.Location = new Point(0, 0);
            startpanel.BackgroundImageLayout=ImageLayout.Stretch;
            startpanel.BackgroundImage = Image.FromFile("白底.jpg");
            
            gametitle.Image = Image.FromFile("標題.png");
            gametitle.Location = new Point(250, 0);
            gametitle.Size = new Size(700, 382);
            gametitle.SizeMode = PictureBoxSizeMode.Zoom;
            gametitle.BackColor = Color.Transparent;

            penera.Image = Image.FromFile("鉛筆橡皮擦.png");
            penera.Location = new Point(600, 330);
            penera.Size = new Size(600, 338);
            penera.SizeMode = PictureBoxSizeMode.Zoom;
            penera.BackColor = Color.Transparent;

            levell.Image = Image.FromFile("level.png");
            levell.Location = new Point(0, 350);
            levell.Size = new Size(220, 80);
            levell.SizeMode = PictureBoxSizeMode.Zoom;
            levell.BackColor = Color.Transparent;

            basicll.Image = Image.FromFile("basic.png");
            basicll.Location = new Point(0, 430);
            basicll.Size = new Size(220, 80);
            basicll.SizeMode = PictureBoxSizeMode.Zoom;
            basicll.BackColor = Color.Transparent;
            basicll.Click += new System.EventHandler(clickbasic);

            interll.Image = Image.FromFile("intermediate.png");
            interll.Location = new Point(0, 510);
            interll.Size = new Size(220, 80);
            interll.SizeMode = PictureBoxSizeMode.Zoom;
            interll.BackColor = Color.Transparent;
            interll.Click += new System.EventHandler(clickinter);;

            advanll.Image = Image.FromFile("advanced.png");
            advanll.Location = new Point(0, 590);
            advanll.Size = new Size(220, 80);
            advanll.SizeMode = PictureBoxSizeMode.Zoom;
            advanll.BackColor = Color.Transparent;
            advanll.Click += new System.EventHandler(clickadvan);
            
            startbutton.Image = Image.FromFile("開始遊戲.png");
            startbutton.Size = new Size(250, 100);
            startbutton.Location = new Point(270, 580);
            startbutton.SizeMode = PictureBoxSizeMode.Zoom;
            startbutton.BackColor = Color.Transparent;
            startbutton.Click += startbutton_Click;

            playingbutton.Click += playingbutton_Click;
        }
        public void clickbasic(object sender, System.EventArgs e)
        { if (levelchoose == 2)interll.Image = Image.FromFile("intermediate.png"); if (levelchoose == 3)advanll.Image = Image.FromFile("advanced.png"); basicll.Image = Image.FromFile("cbasic.png"); levelchoose = 1; }
        public void clickinter(object sender, System.EventArgs e)
        { if (levelchoose == 1) basicll.Image = Image.FromFile("basic.png"); if (levelchoose == 3)advanll.Image = Image.FromFile("advanced.png"); interll.Image = Image.FromFile("cintermediate.png"); levelchoose = 2; }
        public void clickadvan(object sender, System.EventArgs e)
        { if (levelchoose == 1) basicll.Image = Image.FromFile("basic.png"); if (levelchoose == 2)interll.Image = Image.FromFile("intermediate.png"); advanll.Image = Image.FromFile("cadvanced.png"); levelchoose = 3; }
        
    }
    public class Charactor : PictureBox
    {
        public int pic_number; //每張圖片的編號
        public int pic_visit; //圖片是否有被點過
    }
}
