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
        //ブロックの設定とリストへの登録
        private void set_Block(string prop_name,string png_filename)
        {
            //-------------------------------------------------------
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
            //--------------------------------------------------------

            pb.Image = System.Drawing.Image.FromFile(png_filename);
            if (prop_name == "End")
                indent_count--;                          //Endを配置するとインデントを１つ減らす．
            pb.Left = 10 + indent_count * indent_size;
            pb.Name = prop_name;
            clist.Add(pb);
            
            //インデントブロックの設定・登録　0708追加
            while (count < indent_count)
            {
                indent_type = indent.Pop();
                if (indent_type == "If")
                    pb1[count].Image = System.Drawing.Image.FromFile("もし（間）.png");

                if (indent_type == "While")
                    pb1[count].Image = System.Drawing.Image.FromFile("繰り返し（間）.png");
                indent.Push(indent_type);

                pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;　//インデントブロックは右から配置（先表示が前面）
                pb1[count].Name = "Indent";
                clist.Add(pb1[count]);
                count++;
            }
            if (prop_name.Contains("If") || prop_name.Contains("While"))
                indent_count++;                                                  //If Whileを配置するとインデントを１つ増やす．
        }
        //-------------------------------------------------------------------------------------------
        private void block_Create(Comands comand)           //もっとブロックらしくする．⇒グラデーション（Ifは一つのブロックに見えるように） 同じ処理多数あり　⇒　メソッド化できるわ ⇒　したわ
        {
            switch (comand)
            {

                case Comands.Go:
                    set_Block("Go", "前へ.png");
                    break;
                case Comands.Left:
                    set_Block("Left", "左.png");
                    break;
                case Comands.Right:
                    set_Block("Right", "右.png");
                    break;

                case Comands.End:
                    set_Block("End", "ここまで.png");
                    break;

            }
            if (comand == Comands.If)
            {
                indent.Push("If");
                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("Iffront", "もし正面壁なし.png");
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Ifleft", "もし左壁なし.png");
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Ifright", "もし右壁なし.png");
                        break;
                }
            }

            else if (comand == Comands.While)
            {
                indent.Push("While");
                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("Whilefront", "繰り返し正面壁なし.png");
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Whileleft", "繰り返し左壁なし.png");
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Whileright", "繰り返し右壁なし.png");
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
