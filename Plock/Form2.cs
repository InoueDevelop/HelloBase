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

        int insert_point; //ブロック挿入位置（リストインデックス）


        public Form2()
            : base()
        {
            InitializeComponent();
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { setTextBox1(gameInterpriter.getCurrentCode()); }; //デバッグ用(TextBox1に現在のコードを表示)
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { if (gameInterpriter.isEnd()) { runAllTimer.Stop(); setButton6TextAndEnableButtons("すべて実行"); } }; //最後の行に達したら自動停止 

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            //panel　ダブルクリック時のイベントハンドラ追加
            panel1.DoubleClick += new EventHandler(panel_Click);
            //panel1.BackgroundImageLayout = ImageLayout.Zoom;
            //panel1.BackgroundImage = Properties.Resources.背景");

        }

        enum Comands { Go, Left, Right, If, While, End };
        enum Conditions { Front_Wall, Left_Wall, Right_Wall, Forever };

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
                //block_Create(comand, clist.Count);
                block_Create(comand, insert_point);
                block_View(0);
                insert_point++;

            }

        }
        //-------------------------------------------------------------------------------------------
        private void block_View(int k)
        {
            int top;
            int y = 40; //ブロック描画開始位置
            int max_left = 0;

            if (clist.Count > 1 && indent_count > 0)
                max_left = clist[clist.Count - indent_count].Left;     //スクロールが左端の時のLeftの最大値
            panel1.AutoScrollPosition = new Point(0, 0);               //スクロールの位置を（0,0）にしてから描画
            panel1.Controls.Clear();

            for (int i = k; i < clist.Count; i++)
            {
                if (clist[i].Name.Contains("Indent"))
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

                panel1.Controls.Add(clist[i]);
                k++;

            }
            if (clist.Count > 1)
                panel1.AutoScrollPosition = new Point(max_left, clist[clist.Count - 1].Top);

        }
        //-------------------------------------------------------------------------------------------
        private void arrow_View(int top)
        {
            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Image = Properties.Resources.矢印;
            pb.Left = 20;
            pb.Height = 40;
            pb.Width = 60;
            pb.Top = top;
            panel1.Controls.Add(pb);

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
        private void set_Block(string prop_name, Image img, int point) //point==clist.Count ⇒　末尾に追加
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
            int left_pos = 100;
            int indent_size = 150 / 2;
            int count = 0;


            //--------------------------------------------------------
            pb.Image = img;
            if (prop_name == "End")

                indent_count--;                          //Endを配置するとインデントを１つ減らす．



            pb.Left = left_pos + indent_count * indent_size;
            pb.Name = prop_name;
            //Clickイベントにイベントハンドラ追加　0714
            pb.Click += new EventHandler(clist_Click);
            clist.Insert(point, pb);

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
                    {
                        pb1[count].Image = Properties.Resources.もしinterval;
                        pb1[count].Name = "Indent_If";
                    }

                    else if (indent_type2 == "While")
                    {
                        pb1[count].Image = Properties.Resources.繰り返しinterval;
                        pb1[count].Name = "Indent_While";
                    }


                }
                // command != If and While　最新のindentから参照
                else
                {
                    indent_type = indent_copy.Pop();
                    if (indent_type == "If")
                    {
                        pb1[count].Image = Properties.Resources.もしinterval;
                        pb1[count].Name = "Indent_If";
                    }

                    else if (indent_type == "While")
                    {
                        pb1[count].Image = Properties.Resources.繰り返しinterval;
                        pb1[count].Name = "Indent_While";
                    }
                }

                pb1[count].Left = left_pos + (indent_count - count - 1) * indent_size;　//インデントブロックは右から配置（先表示が前面）

                clist.Insert(point + count + 1, pb1[count]);
                count++;
                insert_point++;
            }

            if (!prop_name.Contains("Indent"))
                if (prop_name.Contains("If") || prop_name.Contains("While"))
                    indent_count++;                                                  //If Whileを配置するとインデントを１つ増やす．
        }
        //-------------------------------------------------------------------------------------------
        private void block_Create(Comands comand, int insert_point)
        {
            string st = "";
            switch (comand)
            {

                case Comands.Go:
                    set_Block("Go", Properties.Resources.前へ, insert_point);
                    break;
                case Comands.Left:
                    set_Block("Left", Properties.Resources.左, insert_point);
                    break;
                case Comands.Right:
                    set_Block("Right", Properties.Resources.右, insert_point);
                    break;

                case Comands.End:
                    if (indent_count > 0)
                        st = indent.Pop();
                    else
                        MessageBox.Show("これ以上【ここまでブロック】は置けません．", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (st == "If")
                    {
                        set_Block("End", Properties.Resources.もしend, insert_point);

                    }
                    else if (st == "While")
                    {
                        set_Block("End", Properties.Resources.繰り返しend, insert_point);

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
                        set_Block("Iffront", Properties.Resources.もし正面壁なし, insert_point);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Ifleft", Properties.Resources.もし左壁なし, insert_point);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Ifright", Properties.Resources.もし右壁なし, insert_point);
                        break;
                }
            }

            else if (comand == Comands.While)
            {
                indent.Push("While");


                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("Whilefront", Properties.Resources.繰り返し正面壁なし, insert_point);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("Whileleft", Properties.Resources.繰り返し左壁なし, insert_point);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("Whileright", Properties.Resources.繰り返し右壁なし, insert_point);
                        break;
                    case Conditions.Forever:
                        set_Block("While", Properties.Resources.繰り返し, insert_point);
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
            listBox3.Enabled = true;
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    comand = Comands.Go;
                    listBox3.Enabled = false;
                    break;
                case 1:
                    comand = Comands.Left;
                    listBox3.Enabled = false;
                    break;
                case 2:
                    comand = Comands.Right;
                    listBox3.Enabled = false;
                    break;
                case 3:
                    comand = Comands.If;
                    if (listBox3.Items.Count == 4)
                        listBox3.Items.RemoveAt(3);
                    break;
                case 4:
                    comand = Comands.While;
                    if (listBox3.Items.Count == 3)
                        listBox3.Items.Add("ずっと");
                    break;
                case 5:
                    comand = Comands.End;
                    listBox3.Enabled = false;
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
                case 3:
                    condition = Conditions.Forever;
                    break;
            }
        }

        //-------------------------------------------------------------------------------------------
        //ブロック全削除
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("すべてのブロックを消してもいいですか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                clist.Clear();
                indent_count = 0;
                insert_point = 0;
                indent.Clear();
                block_View(0);
            }
            else { };

        }
        //-------------------------------------------------------------------------------------------
        //pictureboxクリック時のイベントハンドラ
        //ブロックの削除（EndとIndent以外）
        private void clist_Click(object sender, EventArgs e)
        {
            x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
            y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;
            string b_name;


            for (int i = 0; i < clist.Count; i++)
            {
                if (!clist[i].Name.Contains("Indent") && clist[i].Name != "End")
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
                                insert_point = clist.Count;            //挿入開始ポイントをリスト最後に移動
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
                                //int k = i;
                                //while (clist[k].Name != "End")
                                //{
                                //    clist.RemoveAt(k);
                                //    //k++;
                                //}
                                //clist.RemoveAt(i);
                                deleteBlockSet(clist[i].Left, i);
                                insert_point = clist.Count;            //挿入開始ポイントをリスト最後に移動
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
        //---------------------------------------------------------------------------------------------------------------
        //if whileブロックセットの一斉削除
        private void deleteBlockSet(int condition_left, int index)
        {
            int condition_count = 0;
            for (int i = index; i < clist.Count; i++)
            {
                if (!clist[i].Name.Contains("Indent"))
                {
                    if (clist[i].Name.Contains("If") || clist[i].Name.Contains("While"))
                    {
                        condition_count++;
                    }
                }
                if (clist[i].Name == "End")
                {
                    condition_count--;
                }
                //while (condition_count > 0)
                //{
                //if (clist[i].Left >= condition_left)
                //{
                clist.RemoveAt(i);
                i--;
                //}
                //}
                if (condition_count == 0)
                {
                    while (clist.Count > i + 1 && clist[i + 1].Name.Contains("Indent"))
                    {
                        clist.RemoveAt(i + 1);
                    }
                    break;
                }
            }

        }
        //---------------------------------------------------------------------------------------------------------------
        //panelクリック時のイベントハンドラ
        //private void panel_Click(object sender, EventArgs e)
        //{
        //    string b_name = "";
        //    DialogResult result = MessageBox.Show(b_name + "のブロックの前に挿入するブロックを選んでね。", "お願い", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

        //    int x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
        //    int y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;

        //    int indent_count = 0;
        //    Stack<string> st = new Stack<string>();
        //    for (int i = 0; i < clist.Count; i++)
        //    {
        //        if (!clist[i].Name.Contains("Indent") && clist[i].Name != "End")    //*
        //        {
        //            if (y >= clist[i].Top && y < clist[i].Bottom)
        //            {
        //                b_name = clist[i].Name;

        //                if (result == DialogResult.Yes)
        //                {

        //                    insert_block();
        //                    //再描画
        //                    block_View(0);
        //                }
        //                break;
        //            }

        //        }

        //    }


        //}
        //---------------------------------------------------------------------------------------------------------------
        private void panel_Click(object sender, EventArgs e)
        {
            int x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
            int y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;
            for (int i = 0; i < clist.Count; i++)
            {
                if (y >= clist[i].Top && y < clist[i].Bottom)
                {
                    DialogResult result = MessageBox.Show(i + "番目のブロックの次に挿入しますか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        setIndent(i);
                        break;
                    }
                    else break;
                }

            }
        }
        //---------------------------------------------------------------------------------------------------------------
        //ブロックの途中挿入
        private void insert_block()
        {

            int indent_count = 0;
            Stack<string> st = new Stack<string>();
            for (int i = 0; i < clist.Count; i++)
            {

                int k = i + 1;
                while (clist[k].Name.Contains("Indent"))
                {
                    indent_count++;
                    k++;
                }



                PictureBox[] pb1 = new PictureBox[indent_count];
                int count = 0;
                block_Create(comand, i - indent_count);
                //clist.Insert(i-indent_count, pb);                                                   //new Block
                while (indent_count < 0)
                {
                    string indent_type = st.Pop();
                    if (indent_type == "If")
                        pb1[count].Image = Properties.Resources.もしinterval;

                    else if (indent_type == "While")
                        pb1[count].Image = Properties.Resources.繰り返しinterval;

                    clist.Insert(i + 1, pb1[count]);                                    //Indent Block
                    count++;
                }




                break;


            }

        }


        //--------------------------------------------------------------------------------------------------------------- 8/1
        //選択されたブロックの行にあるインデントの情報を設定
        private void setIndent(int point)
        {
            Stack<string> line_indents = new Stack<string>();
            if (point+1<clist.Count)
            {
                while (clist[point + 1].Name.Contains("Indent"))
                {
                    if (clist[point + 1].Name == "Indent_If")
                        line_indents.Push("If");
                    else
                        line_indents.Push("While");
                    point++;
                }
            }

            indent_count = line_indents.Count; //インデント数の更新
            insert_point = point+1; //挿入開始位置の更新
            indent = new Stack<string>(line_indents.ToArray()); // スタックのコピー 参照型であることに注意
        }
        //---------------------------------------------------------------------------------------------------------------
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            if (!validateCode()) return;//コードの形が正しくないときは何も実行しない

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
            if (!validateCode()) return;//コードの形が正しくないときは何も実行しない
            if (runAllTimer.Enabled == false) gameForm = gameInterpriter.runOneLine(block_to_queue(), gameForm);//一行実行
            textBox1.Text = gameInterpriter.getCurrentCode();

        }

        /// <summary>
        /// コードが正しいかどうか（中かっこの数が足りているか？）（一行実行で現在のコードがビルド時と同じかどうか？　もあとで増やす）を返す
        /// 正しくないときは警告を出す
        /// </summary>
        /// <returns></returns>
        private bool validateCode()
        {
            if (clist == null || clist.Count == 0)
            {
                MessageBox.Show("ブロックを選択しよう", "お願い", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                return false;
            }
            if (clist.Last().Name == "Indent" || clist.Last().Name.Contains("If") || clist.Last().Name.Contains("While"))//コードの中かっこの数が正しくないときは、警告を出す
            {
                MessageBox.Show("「ここまで」ブロックが足りません", "お願い", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                return false;
            }
            return true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //ゲームのデータクラスの更新
                if (runAllTimer.Enabled == true)
                {
                    runAllTimer.Stop();//タイマーを停止する
                    button1.Enabled = true;//他のボタンを押せるようにする
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Text = "すべて実行";
                }
                else
                {
                    if (!validateCode()) return;//コードの形が正しくないときは何も実行しない

                    Queue<string> codeQueue = block_to_queue();
                    gameInterpriter.build(codeQueue);
                    runAllTimer.Start();//タイマーをスタートする
                    button1.Enabled = false;////他のボタンを押せなくする
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Text = "停止する";
                }
            }
            catch (Exception exc)
            {
                textBox1.Text = exc.ToString();
            }
        }


        delegate void MyDelegate();
        private void setButton6TextAndEnableButtons(string str)
        {
            if (this.button6.InvokeRequired)
            {
                MyDelegate setTex6 = new MyDelegate(() => button6.Text = str);
                this.Invoke(setTex6);
            }
            else { this.button6.Text = str; }

            MyDelegate changeEnableTrue = new MyDelegate(() =>
            {
                button1.Enabled = true;//他のボタンを押せるようにする
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            });
            this.Invoke(changeEnableTrue);
        }

        private void setArrowPicture(string str)
        {
            if (this.button6.InvokeRequired)
            {
                MyDelegate setTex6 = new MyDelegate(() => button6.Text = str);
                this.Invoke(setTex6);
            }
            else { this.button6.Text = str; }
        }


        private void setTextBox1(string str)
        {
            if (this.textBox1.InvokeRequired)
            {
                MyDelegate setTex1 = new MyDelegate(() => textBox1.Text = str);
                this.Invoke(setTex1);
            }
            else { this.textBox1.Refresh(); }
        }

    }
}
