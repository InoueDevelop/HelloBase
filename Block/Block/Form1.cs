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
        //-------------------------------------------------------------------------------------------
        // マウスのボタンが押下された時に起動するイベントプロシージャ
        //  --- ドラッグ元となるpictureBox1上で、マウスボタンが押された時に、
        //      ドラッグ&ドロップを開始する処理を実行する。
        private void clist_MouseDown(object sender, MouseEventArgs e)
        {
            // マウスの左ボタンが押されている場合
            if (e.Button == MouseButtons.Left)
            {
                // ドラッグ&ドロップを開始
                //  --- ドラッグ ソースのデータは、pictureBox1とするように指示。
                //      また、ドラッグ ソースのデータは、ドロップ先に移動するよう
                //      に指示。
                DoDragDrop(clist, DragDropEffects.Move);
            }
        }

        // オブジェクトがコントロールの境界内にドラッグされた時に起動
        // するイベントプロシージャ
        //  --- ドロップ先となるForm1上の領域内に、マウスポインターが
        //      ドラッグされた時に、ドロップ先での準備関係の処理を実行
        //      する。
        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.Label))
               && (e.AllowedEffect == DragDropEffects.Move))
            {
                // ドロップ先に、移動を許可するように指示する。
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                // ドロップ先に、ドロップを受け入れないように指示する。
                e.Effect = DragDropEffects.None;
            }
        }

        // ドロップされた時に起動するイベントプロシージャ
        //  --- ドロップ先となるForm1上で、マウスボタンが離された時（
        //      すなわち、ドラッグ&ドロップ操作が完了した時）に、画像
        //      の移動処理を実行する。
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグされているデーターがPictureBoxで、かつ、
            // ドラッグ ソースのデータは、ドロップ先に移動するよう指示さ
            // れている場合（すなわち、コピー等の別の指示ではない場合）
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.PictureBox))
                && (e.Effect == DragDropEffects.Move))
            {
                // ドラッグ ソースのデータ（pictureBox1）を取得
                System.Windows.Forms.PictureBox picture = (System.Windows.Forms.PictureBox)e.Data.GetData(typeof(System.Windows.Forms.PictureBox));

                // ドラッグ ソースのデータ（pictureBox1）における座標位置を取得
                Point posi = new Point(e.X, e.Y);

                // ドラッグ ソースのデータ（pictureBox1）における座標位置
                // を、スクリーン座標からクライアント座標に変換
                posi = this.PointToClient(posi);

                // ドラッグ ソースのデータ（pictureBox1）の座標位置を設定
                //  --- なお、ドロップ先でのマウスの座標位置に、pictureBox1
                //      の中心が来るように指定する。
                posi = new Point(posi.X - picture.Width / 2, posi.Y - picture.Height / 2);
                picture.Location = posi;
            }
            // ドラッグされているデーターがPictureBoxではない場合、又は、
            // ドラッグ ソースのデータは、ドロップ先に移動するよう指示され
            // ていない場合（すなわち、移動ではなく、コピー等の別の指示の場合）
            else
            {
                // 特に処理はなし
            }
        }
        //-------------------------------------------------------------------------------------------
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //-------------------------------------------------------------------------------------------
        //変換ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clist.Count; i++)
            {
                listBox2.Items.Add(clist[i].Name); //ホントはソースコード辞書の内容を書き出す．
            }
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
        //ブロックの取り消し
        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}
