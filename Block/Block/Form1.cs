using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Block
{
    public partial class Form1 : Form
    {
        Comands comand;
        Conditions condition;

        // List<Label> clist = new List<Label>();
        //List<Block> block = new List<Block>();
        List<PictureBox> clist = new List<PictureBox>();  //ブロック格納リスト
        Stack<string> indent = new Stack<string>();       //インデントの種類（IfかWhileか）
        int indent_count;                                 //インデント回数


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            //panel1.BackgroundImageLayout = ImageLayout.Zoom;
            //panel1.BackgroundImage = System.Drawing.Image.FromFile("背景.png");

        }

        enum Comands { Go, Left, Right, If, While, End };
        enum Conditions { Front_Wall, Left_Wall, Right_Wall };

        //-------------------------------------------------------------------------------------------
        //配置ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            block_Create(comand);
            block_View(0);

        }
        //-------------------------------------------------------------------------------------------
        private void block_View(int k)
        {
            int top = 0;
            int y = 14;
            //int k=0;
            int pn_height = 0;
            for (int i = k; i < clist.Count; i++)
            {
                if (clist[i].Name == "Indent")
                {
                    k--;
                }
                top = y + k * 40;
                clist[i].Top = top;
                clist[i].Height = 40;
                clist[i].Width = 150;
                clist[i].AutoSize = false;
                // ドラッグ&ドロップを行なう時のドロップ先のコントロール（フォーム）に、ドロップを受け入れるように指示
                clist[i].AllowDrop = true;

                //clist[i].Select();
                //lb.Cursor
                //clist[i].Font = new Font(clist[i].Font.FontFamily, 16, FontStyle.Bold);

                panel1.Controls.Add(clist[i]);
                k++;
                //pn_height = 0;
            }

        }
        //-------------------------------------------------------------------------------------------
        private void block_into_Panel(Panel panel, List<PictureBox> llist)
        {
            for (int i = 0; i < llist.Count; i++)
            {
                panel.Controls.Add(llist[i]);
            }
        }
        //-------------------------------------------------------------------------------------------
        private void block_Create(Comands comand)           //もっとブロックらしくする．⇒グラデーション（Ifは一つのブロックに見えるように） 同じ処理多数あり　⇒　メソッド化できるわ
        {
            PictureBox pb = new PictureBox();
            //画像の大きさをPictureBoxに合わせる
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBox[] pb1 = new PictureBox[indent_count];
            for (int i = 0; i < indent_count; i++)
            {
                pb1[i] = new PictureBox();
                pb1[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }
            string indent_type;
            int indent_size = 150 / 2;
            int count = 0;

            switch (comand)
            {

                case Comands.Go:

                    pb.Image = System.Drawing.Image.FromFile("前へ.png");
                    pb.Left = 10 + indent_count * indent_size;    //Left=10
                    pb.Name = "Go";
                    clist.Add(pb);
                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                        pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");

                        if (indent_type == "While")
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        clist.Add(pb1[count]);
                        count++;
                    }
                    break;
                case Comands.Left:
                    pb.Image = System.Drawing.Image.FromFile("左.png");
                    pb.Left = 10 + indent_count * indent_size;
                    pb.Name = "Left";
                    clist.Add(pb);
                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                        pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");
                        if (indent_type == "While")
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        clist.Add(pb1[count]);
                        count++;
                    }
                    break;
                case Comands.Right:
                    pb.Image = System.Drawing.Image.FromFile("右.png");
                    pb.Left = 10 + indent_count * indent_size;
                    pb.Name = "Right";
                    clist.Add(pb);
                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                        pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");
                        if (indent_type == "While")
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        clist.Add(pb1[count]);
                        count++;
                    }
                    break;

                case Comands.End:

                    pb.Image = System.Drawing.Image.FromFile("ここまで.png");
                    pb.Name = "End";
                    clist.Add(pb);
                    indent_count--;
                    pb.Left = 10 + indent_count * indent_size;
                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                        pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");

                        if (indent_type == "While")
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        clist.Add(pb1[count]);
                        count++;
                    }
                    break;

            }
            if (comand == Comands.If)
            {
                indent.Push("If");
                switch (condition)
                {
                    case Conditions.Front_Wall:
                        pb.Image = System.Drawing.Image.FromFile("もし正面壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Iffront";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {

                            pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                    case Conditions.Left_Wall:
                        pb.Image = System.Drawing.Image.FromFile("もし左壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Ifleft";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                    case Conditions.Right_Wall:
                        pb.Image = System.Drawing.Image.FromFile("もし右壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Ifright";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                }
            }

            else if (comand == Comands.While)
            {
                indent.Push("While");
                switch (condition)
                {
                    case Conditions.Front_Wall:
                        pb.Image = System.Drawing.Image.FromFile("繰り返し正面壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Whilefront";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                    case Conditions.Left_Wall:
                        pb.Image = System.Drawing.Image.FromFile("繰り返し左壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Whileleft";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                    case Conditions.Right_Wall:
                        pb.Image = System.Drawing.Image.FromFile("繰り返し右壁なし.png");
                        pb.Left = 10 + indent_count * indent_size;
                        pb.Name = "Whileright";
                        clist.Add(pb);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            clist.Add(pb1[count]);
                            count++;
                        }
                        indent_count++;
                        break;
                }
            }

        }
        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //-------------------------------------------------------------------------------------------
        //変換ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            
                block_to_string();
            
        }
        //-------------------------------------------------------------------------------------------
        //命令文選択リスト
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (listBox1.SelectedIndex)
            {
                case 0:
                    comand = Comands.Go;
                    break;
                case 1:
                    comand = Comands.Left;
                    break;
                case 2:
                    comand = Comands.Right;
                    break;
                case 3:
                    comand = Comands.If;
                    break;
                case 4:
                    comand = Comands.While;
                    break;
                case 5:
                    comand = Comands.End;
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------
        //条件文選択リスト
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBox3.SelectedIndex)
            {
                case 0:
                    condition = Conditions.Front_Wall;
                    break;
                case 1:
                    condition = Conditions.Left_Wall;
                    break;
                case 2:
                    condition = Conditions.Right_Wall;
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------
        //ブロックから文字列へ
        public void block_to_string()
        {
            for(int i=0;i<clist.Count;i++)
            {
                switch(clist[i].Name)
                {
                    case "Go" :
                        listBox2.Items.Add("前へ進む．");
                    break;
                    case "Left":
                    listBox2.Items.Add("左へ向きを変える．");
                    break;
                    case "Right":
                    listBox2.Items.Add("右へ向きを変える．");
                    break;
                    case "Iffront":
                    listBox2.Items.Add("もし、正面に壁があるなら、");
                    listBox2.Items.Add("{");
                    break;
                    case "Ifleft":
                    listBox2.Items.Add("もし、左に壁があるなら、");
                    listBox2.Items.Add("{");
                    break;
                    case "Ifright":
                    listBox2.Items.Add("もし、右に壁があるなら、");
                    listBox2.Items.Add("{");
                    break;
                    case "Whilefront":
                    listBox2.Items.Add("正面に壁がある間は、");
                    listBox2.Items.Add("{");
                    break;
                    case "Whileleft":
                    listBox2.Items.Add("左に壁がある間は、");
                    listBox2.Items.Add("{");
                    break;
                    case "Whileright":
                    listBox2.Items.Add("右に壁がある間は、");
                    listBox2.Items.Add("{");
                    break;
                    case "End":
                    listBox2.Items.Add("}");
                    break;
                   
                }
            }
        }
        //-------------------------------------------------------------------------------------------
        //ブロックの取り消し
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
