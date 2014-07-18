using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plock
{
    public partial class Form2 : InterpriterController
    {
        Comands comand;
        Conditions condition;

        // List<Label> clist = new List<Label>();
        //List<Block> block = new List<Block>();
        List<PictureBox> clist = new List<PictureBox>();  //ブロック格納リスト
        static Stack<string> indent = new Stack<string>();       //インデントの種類（IfかWhileか）
        int indent_count;                                 //インデント回数

        int x;   //マウス座標
        int y;


        public Form2()
            : base()
        {
            InitializeComponent();
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { setTextBox1(gameInterpriter.getCurrentCode()); }; //デバッグ用(TextBox1に現在のコードを表示)
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            //panel1.BackgroundImageLayout = ImageLayout.Zoom;
            //panel1.BackgroundImage = Properties.Resources.背景");

        }

        enum Comands { Go, Left, Right, If, While, End };
        enum Conditions { Front_Wall, Left_Wall, Right_Wall };

        //-------------------------------------------------------------------------------------------
        //配置ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("命令セットを選択してください．", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if ((listBox1.SelectedIndex == 3 || listBox1.SelectedIndex == 4) && listBox3.SelectedIndex == -1)
            {
                MessageBox.Show("条件セットを選択してください．", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                block_Create(comand);
                block_View(0);
            }

        }
        //-------------------------------------------------------------------------------------------
        private void block_View(int k)
        {
            int top;
            int y = 10; //ブロック描画開始位置

            panel1.Controls.Clear();
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
        private void set_Block(string prop_name, Image img)
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
            string indent_type2;
            //Stack<string> indent_copy = indent;//indentの値も変わってしまう
            // スタックのコピー
            Stack<string> indent_copy = new Stack<string>(indent.ToArray());
            indent_copy = new Stack<string>(indent_copy.ToArray());
            int indent_size = 150 / 2;
            int count = 0;


            //--------------------------------------------------------
            pb.Image = img;
            if (prop_name == "End")
                indent_count--;                          //Endを配置するとインデントを１つ減らす．
            pb.Left = 10 + indent_count * indent_size;
            pb.Name = prop_name;
            //Clickイベントにイベントハンドラ追加　0714
            pb.Click += new EventHandler(clist_Click);
            clist.Add(pb);

            //インデントブロックの設定・登録　0708追加
            while (count < indent_count)
            {

                //Stackクラスは参照型　⇒　indentの値も変わってしまう
                //command==If or While 最新のindentの次のindentから参照
                if (comand == Comands.If || comand == Comands.While)
                {
                    if (count == 0)
                        indent_copy.Pop(); //最新のindentだけ捨てる．
                    indent_type2 = indent_copy.Pop();
                    if (indent_type2 == "If")
                        pb1[count].Image = Properties.Resources.もしinterval;

                    else if (indent_type2 == "While")
                        pb1[count].Image = Properties.Resources.繰り返しinterval;


                }
                // command != If and While　最新のindentから参照
                else
                {
                    indent_type = indent_copy.Pop();
                    if (indent_type == "If")
                        pb1[count].Image = Properties.Resources.もしinterval;

                    else if (indent_type == "While")
                        pb1[count].Image = Properties.Resources.繰り返しinterval;
                }

                pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;　//インデントブロックは右から配置（先表示が前面）
                pb1[count].Name = "Indent";
                clist.Add(pb1[count]);
                count++;
            }

            if (prop_name.Contains("If") || prop_name.Contains("While"))
                indent_count++;                                                  //If Whileを配置するとインデントを１つ増やす．
        }
        //-------------------------------------------------------------------------------------------
        private void block_Create(Comands comand)
        {
            string st;
            switch (comand)
            {

                case Comands.Go:
                    set_Block("Go", Properties.Resources.前へ);
                    break;
                case Comands.Left:
                    set_Block("Left", Properties.Resources.左);
                    break;
                case Comands.Right:
                    set_Block("Right", Properties.Resources.右);
                    break;

                case Comands.End:
                    st = indent.Pop();
                    if (st == "If")
                    {
                        set_Block("End", Properties.Resources.もしend);

                    }
                    else if (st == "While")
                    {
                        set_Block("End", Properties.Resources.繰り返しend);

                    }
                    break;
                default: break;

            }
            if (comand == Comands.If)
            {
                indent.Push("If");

                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("Iffront", Properties.Resources.もし正面壁なし);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Ifleft", Properties.Resources.もし左壁なし);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Ifright", Properties.Resources.もし右壁なし);
                        break;
                }
            }

            else if (comand == Comands.While)
            {
                indent.Push("While");

                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("Whilefront", Properties.Resources.繰り返し正面壁なし);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Whileleft", Properties.Resources.繰り返し左壁なし);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Whileright", Properties.Resources.繰り返し右壁なし);
                        break;
                }
            }

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //-------------------------------------------------------------------------------------------
        //命令文選択リスト
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.SelectedIndex = -1;
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    comand = Comands.Go;
                    //listBox3.SelectedIndex = -1;
                    break;
                case 1:
                    comand = Comands.Left;
                    //listBox3.SelectedIndex = -1;
                    break;
                case 2:
                    comand = Comands.Right;
                    // listBox3.SelectedIndex = -1;
                    break;
                case 3:
                    comand = Comands.If;
                    break;
                case 4:
                    comand = Comands.While;
                    break;
                case 5:
                    comand = Comands.End;
                    //listBox3.SelectedIndex = -1;
                    break;
                default: break;
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
        //-------------------------------------------------------------------------------------------
        //pictureboxクリック時のイベントハンドラ
        private void clist_Click(object sender, EventArgs e)
        {
            x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
            y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;
            string b_name;


            for (int i = 0; i < clist.Count; i++)
            {
                if (clist[i].Name != "Indent" && clist[i].Name != "End")
                {
                    if (!clist[i].Name.Contains("If") && !clist[i].Name.Contains("While"))
                    {
                        if (y >= clist[i].Top && y < clist[i].Bottom)
                        {
                            b_name = clist[i].Name;
                            DialogResult result = MessageBox.Show(b_name + "のブロックを消去してもいいですか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {
                                clist.RemoveAt(i);
                                //再描画
                                block_View(0);
                            }
                            else
                            {

                            }
                            break;
                        }
                    }
                    //If While ブロックを選択した場合は中身を全て削除．
                    else
                    {
                        if (y >= clist[i].Top && y < clist[i].Bottom)
                        {
                            b_name = clist[i].Name;
                            DialogResult result = MessageBox.Show(b_name + "のブロック全体を消去してもいいですか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {
                                int k = i;
                                while (clist[k].Name != "End")
                                {
                                    clist.RemoveAt(k);
                                    //k++;

                                }
                                clist.RemoveAt(i);
                                //再描画
                                block_View(0);
                            }
                            else
                            {

                            }
                            break;
                        }

                    }
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                Queue<string> codeQueue = block_to_queue();
                if (runAllTimer.Enabled == false) gameInterpriter.build(codeQueue);
                textBox1.Text = gameInterpriter.getCurrentCode();

            }
            catch (Exception exc)
            {
                textBox1.Text = exc.ToString();
            }
        }

        private Queue<string> block_to_queue()
        {
            Queue<string> _codeQueue = new Queue<string>();
            int codeNumber = 0;
            for (int i = 0; i < clist.Count; i++)
            {
                switch (clist[i].Name)
                {
                    case "Go":
                        _codeQueue.Enqueue(codeNumber + ":前へ進む");
                        codeNumber++;
                        break;
                    case "Left":
                        _codeQueue.Enqueue(codeNumber + ":左を向く");
                        codeNumber++;
                        break;
                    case "Right":
                        _codeQueue.Enqueue(codeNumber + ":右を向く");
                        codeNumber++;
                        break;
                    case "Iffront":
                        _codeQueue.Enqueue(codeNumber + ":もし、正面に壁がないなら{");
                        codeNumber++;
                        break;
                    case "Ifleft":
                        _codeQueue.Enqueue(codeNumber + ":もし、左に壁がないなら{");
                        codeNumber++;
                        break;
                    case "Ifright":
                        _codeQueue.Enqueue(codeNumber + ":もし、右に壁がないなら{");
                        codeNumber++;
                        break;
                    case "Whilefront":
                        _codeQueue.Enqueue(codeNumber + ":正面に壁がないなら繰り返す{");
                        codeNumber++;
                        break;
                    case "Whileleft":
                        _codeQueue.Enqueue(codeNumber + ":左に壁がないなら繰り返す{");
                        codeNumber++;
                        break;
                    case "Whileright":
                        _codeQueue.Enqueue(codeNumber + ":右に壁がないなら繰り返す{");
                        codeNumber++;
                        break;
                    case "End":
                        _codeQueue.Enqueue(codeNumber + ":}");
                        codeNumber++;
                        break;

                }
            }
            return _codeQueue;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (runAllTimer.Enabled == false) gameForm = gameInterpriter.runOneLine(block_to_queue(), gameForm);//一行実行
            textBox1.Text = gameInterpriter.getCurrentCode();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Queue<string> codeQueue = block_to_queue();
                gameInterpriter.build(codeQueue);
                //ゲームのデータクラスの更新
                if (runAllTimer.Enabled == true)
                {
                    runAllTimer.Stop();
                    button6.Text = "すべて実行";
                }
                else
                {
                    runAllTimer.Start();
                    button6.Text = "停止する";
                }
            }
            catch (Exception exc)
            {
                textBox1.Text = exc.ToString();
            }
        }

        //デバッグ用(TextBox1に現在のコードを表示する用)
        delegate void forSetTextBox1();
        private void setTextBox1(string str)
        {
            if (this.textBox1.InvokeRequired)
            {
                forSetTextBox1 setTex1 = new forSetTextBox1(() => textBox1.Text = str);
                this.Invoke(setTex1);
            }
            else { this.textBox1.Refresh(); }
        }

    }
}
