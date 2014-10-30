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


        //ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        List<PictureBox> clist = new List<PictureBox>();  //ブロック格納リスト
        static Stack<string> indent = new Stack<string>();       //インデントの種類（IfかWhileか）
        int indent_count;                                 //インデント回数
        delegate void safevelocityenable();

        int x;   //マウス座標
        int y;



        int insert_point; //ブロック挿入位置（リストインデックス）
        private PictureBox arrowPicture;//矢印の画像
        private PictureBox line;//挿入位置を示す横棒
        private PictureBox dummyBlock;//ドラックアンドドロップ用の画像


        public Form2()
            : base()
        {
            InitializeComponent();
            arrowPicture = new PictureBox();
            //PicutureBoxの上でドロップした場合でも通常通りの動作をするように
            arrowPicture.AllowDrop = true;
            arrowPicture.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            arrowPicture.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            arrowPicture.DragOver += new System.Windows.Forms.DragEventHandler(this.panel1_DragOver);
            line = new PictureBox();
            line.AllowDrop = true;
            line.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            line.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            line.DragOver += new System.Windows.Forms.DragEventHandler(this.panel1_DragOver);
            dummyBlock = new PictureBox();

            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { setTextBox1(gameInterpriter.getCurrentCode()); }; //デバッグ用(TextBox1に現在のコードを表示)
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { setArrowPicture(); }; //矢印の描画
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { if (gameInterpriter.isEnd() || gameForm.locked == true) { runAllTimer.Stop(); setButton6TextAndEnableButtons("すべて実行"); safevelocityenabled(); } }; //最後の行に達したら自動停止 
        }


        void safevelocityenabled()
        {

            if (this.velocityBar1.InvokeRequired)
            {
                safevelocityenable safeveloenabled = new safevelocityenable(() => safebarenable());
                this.Invoke(safeveloenabled);
            }
            else { this.velocityBar1.Enabled = true; }

        }

        void safebarenable()
        {

            velocityBar1.Enabled = true;
        }



        private void Form2_Load(object sender, EventArgs e)
        {

            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            //panel　ダブルクリック時のイベントハンドラ追加
            //panel1.DoubleClick += new EventHandler(panel_Click);

            ToolStripMenuItem mi1 = new ToolStripMenuItem("削除");
            mi1.Click += new EventHandler(onClick1);
            //ToolStripMenuItem mi2 = new ToolStripMenuItem("挿入");
            //mi2.Click += new EventHandler(onClick2);
            //ToolStripSeparator tsSep = new ToolStripSeparator();
            //ToolStripMenuItem miExit = new ToolStripMenuItem("終了(&X)");
            //miExit.Click += new EventHandler(onExitClick);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { mi1});
            //panel1.BackgroundImageLayout = ImageLayout.Zoom;
            //panel1.BackgroundImage = Properties.Resources.背景");

        }

        enum Comands { Go, Left, Right, If, While, End };
        enum Conditions { Front_Wall, Left_Wall, Right_Wall, Front_noWall, Left_noWall, Right_noWall, Forever };
        enum InsertPoint { Before, After };

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

                block_Create(comand, insert_point);
                block_View(0);
                //label1.Text = clist.Count.ToString();
                countBlocknumber();
                //line_View(40 + (countBlocknumber()) * 40);
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
            panel1.AutoScrollPosition = new Point(0, 0);               //スクロールの位置を（0,0）にしてから描画
            arrowPicture.Left = 20;
            arrowPicture.Height = 40;
            arrowPicture.Width = 60;
            arrowPicture.Top = top;
            arrowPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            arrowPicture.Image = Properties.Resources.矢印;
            panel1.Controls.Add(arrowPicture);
            panel1.AutoScrollPosition = new Point(0, top);

        }
        private void arrow_View()
        {
            try
            {
                if (!gameInterpriter.getCurrentCode().Contains(':')) return;//区切り文字の:が含まれていないなら、矢印を描画しない
                string number = gameInterpriter.getCurrentCode().Split(':')[0];
                arrow_View(40 + int.Parse(number) * 40);
            }
            catch (Exception exc)
            {
                textBox1.Text = exc.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        //-------------------------------------------------------------------------------------------
        private int countBlocknumber()
        {
            int count = 0;
            for (int i = 0; i < clist.Count; i++)
            {
                if (!clist[i].Name.Contains("Indent"))
                    count++;
            }
            label5.Text = count.ToString();
            return count;
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
            //pb.Click += new EventHandler(onClick1);
            //PicutureBoxの上でドロップした場合でも通常通りの動作をするように
            pb.AllowDrop = true;
            pb.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            pb.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            pb.DragOver += new System.Windows.Forms.DragEventHandler(this.panel1_DragOver);
            pb.AllowDrop = true;
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
        //-------------------------------------------------------------------------------------------------------------
        private bool canPutend()
        {
            int end_count = 0;
            int condition_count = 0;
            for (int i = 0; i < clist.Count; i++)
            {
                if ((clist[i].Name.Contains("If") || clist[i].Name.Contains("While")) && !clist[i].Name.Contains("Indent"))
                    condition_count++;
                else if (clist[i].Name.Contains("End"))
                    end_count++;
            }
            if (condition_count == end_count)
                return false;
            else
                return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void block_Create(Comands comand, int insertpoint)
        {
            string st = "";
            switch (comand)
            {

                case Comands.Go:
                    set_Block("Go", Properties.Resources.前へ, insertpoint);
                    break;
                case Comands.Left:
                    set_Block("Left", Properties.Resources.左, insertpoint);
                    break;
                case Comands.Right:
                    set_Block("Right", Properties.Resources.右, insertpoint);
                    break;

                case Comands.End:
                    if (indent_count > 0 && canPutend())
                        st = indent.Pop();
                    else
                    {
                        MessageBox.Show("これ以上【ここまでブロック】は置けません．", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        insert_point--;
                    }
                    if (st == "If")
                    {
                        set_Block("End", Properties.Resources.もしend, insertpoint);

                    }
                    else if (st == "While")
                    {
                        set_Block("End", Properties.Resources.繰り返しend, insertpoint);

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
                        set_Block("IffrontWall", Properties.Resources.もし正面壁あり, insertpoint);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("IfleftWall", Properties.Resources.もし左壁あり, insertpoint);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("IfrightWall", Properties.Resources.もし右壁あり, insertpoint);
                        break;
                    case Conditions.Front_noWall:
                        set_Block("Iffront", Properties.Resources.もし正面壁なし, insertpoint);
                        break;
                    case Conditions.Left_noWall:
                        set_Block("Ifleft", Properties.Resources.もし左壁なし, insertpoint);
                        break;
                    case Conditions.Right_noWall:
                        set_Block("Ifright", Properties.Resources.もし右壁なし, insertpoint);
                        break;
                }
            }

            else if (comand == Comands.While)
            {
                indent.Push("While");


                switch (condition)
                {
                    case Conditions.Front_Wall:
                        set_Block("WhilefrontWall", Properties.Resources.繰り返し正面壁あり, insertpoint);
                        break;
                    case Conditions.Left_Wall:
                        set_Block("WhileleftWall", Properties.Resources.繰り返し左壁あり, insertpoint);
                        break;
                    case Conditions.Right_Wall:
                        set_Block("WhilerightWall", Properties.Resources.繰り返し右壁あり, insertpoint);
                        break;
                    case Conditions.Front_noWall:
                        set_Block("Whilefront", Properties.Resources.繰り返し正面壁なし, insertpoint);
                        break;
                    case Conditions.Left_noWall:
                        set_Block("Whileleft", Properties.Resources.繰り返し左壁なし, insertpoint);
                        break;
                    case Conditions.Right_noWall:
                        set_Block("Whileright", Properties.Resources.繰り返し右壁なし, insertpoint);
                        break;
                    case Conditions.Forever:
                        set_Block("While", Properties.Resources.繰り返し, insertpoint);
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
                    if (listBox3.Items.Count == 7)
                        listBox3.Items.RemoveAt(6);
                    break;
                case 4:
                    comand = Comands.While;
                    if (listBox3.Items.Count == 6)
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
                    condition = Conditions.Front_noWall;
                    break;
                case 4:
                    condition = Conditions.Left_noWall;
                    break;
                case 5:
                    condition = Conditions.Right_noWall;
                    break;
                case 6:
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
                countBlocknumber();

            }
            else { };

        }
        //-------------------------------------------------------------------------------------------
        private void onClick1(object sender, EventArgs e)
        {
            deleteBlock(x, y);
        }
        private void onClick2(object sender, EventArgs e)
        {
            insertBlock();
        }
        //-------------------------------------------------------------------------------------------
        void onExitClick(object sender, EventArgs e)
        {
            contextMenuStrip1.Close();
        }
        //-------------------------------------------------------------------------------------------
        private void deleteBlock(int x, int y)
        {
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

                            setIndent(i, InsertPoint.Before);         //消す前にインデックス情報更新
                            clist.RemoveAt(i); //対象ブロックの削除

                            if (i < clist.Count)
                            {
                                while (clist[i].Name.Contains("Indent"))
                                {
                                    clist.RemoveAt(i);
                                    if (i >= clist.Count)
                                        break;
                                }
                            }
                            //再描画
                            block_View(0);
                            countBlocknumber();

                            break;
                        }
                    }
                    //If While ブロックを選択した場合は中身を全て削除．
                    else
                    {
                        if (y >= clist[i].Top && y < clist[i].Bottom)
                        {
                            b_name = clist[i].Name;
                            DialogResult result = MessageBox.Show(translate(b_name) + "のブロック全体を消去してもいいですか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {

                                deleteBlockSet(clist[i].Left, i);

                                //再描画
                                block_View(0);
                                countBlocknumber();
                                //line_View(40 + (countBlocknumber()) * 40);
                                //label1.Text = clist.Count.ToString();
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
        //-------------------------------------------------------------------------------------------
        //pictureboxクリック時のイベントハンドラ
        //ブロックの削除（EndとIndent以外）
        private void clist_Click(object sender, EventArgs e)
        {
            int cu_x = System.Windows.Forms.Cursor.Position.X;
            int cu_y = System.Windows.Forms.Cursor.Position.Y;
            x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
            y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;



            contextMenuStrip1.Show(cu_x, cu_y);

            //deleteBlock(x, y);

        }


        string translate(string English)
        {
            switch (English)
            {
                case "Go":
                    return "前へ進む";

                case "Left":
                    return "左を向く";

                case "Right":
                    return "右を向く";

                case "IffrontWall":
                    return "もし、正面に壁があるなら";


                case "IfleftWall":
                    return "もし、左に壁があるなら";


                case "IfrightWall":
                    return "もし、右に壁があるなら";

                case "Iffront":
                    return "もし、正面に壁がないなら";


                case "Ifleft":
                    return "もし、左に壁がないなら";


                case "Ifright":
                    return "もし、右に壁がないなら";

                case "WhilefrontWall":
                    return "正面に壁があるなら繰り返す";


                case "WhileleftWall":
                    return "左に壁があるなら繰り返す";


                case "WhilerightWall":
                    return "右に壁があるなら繰り返す";

                case "Whilefront":
                    return "正面に壁がないなら繰り返す";


                case "Whileleft":
                    return "左に壁がないなら繰り返す";


                case "Whileright":
                    return "右に壁がないなら繰り返す";


                case "End":
                    return "こ";

                default: return "こ";//"こ"のブロックを削除してもよろしいですか？


            }
            return "";
        }
        //---------------------------------------------------------------------------------------------------------------
        //if whileブロックセットの一斉削除
        private void deleteBlockSet(int condition_left, int index)
        {
            int last_endIndex = 0;
            int condition_count = 0;
            Stack<int> left_proximate = new Stack<int>();
            for (int i = index; i < clist.Count; i++)
            {
                if (!clist[i].Name.Contains("Indent"))
                {
                    if (clist[i].Name.Contains("If") || clist[i].Name.Contains("While"))
                    {
                        condition_count++;
                        left_proximate.Push(clist[i].Left);
                    }
                }
                if (clist[i].Name == "End")
                {
                    condition_count--;
                }
                if (condition_count == 0)
                {
                    last_endIndex = i;
                    break;
                }
            }

            if (condition_count != 0)
                MessageBox.Show("【ここまでブロック】を置いてね．", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                setIndent(index, InsertPoint.Before); //消す前にインデックス情報更新
                indent.Pop(); //削除するときはindentの先頭の情報は捨てる（挿入するときのみ使う）
                indent_count--;

                int count = last_endIndex - index + 1;
                while (count > 0)
                {
                    clist.RemoveAt(index);
                    count--;
                }

                while (clist.Count > index + 1 && clist[index].Name.Contains("Indent"))
                {
                    clist.RemoveAt(index);
                }


            }
        }
        //---------------------------------------------------------------------------------------------------------------
        private bool canInsert()
        {
            int condition_count = 0;
            int end_count = 0;
            for (int i = 0; i < clist.Count; i++)
            {
                if (!clist[i].Name.Contains("Indent"))
                    if (clist[i].Name.Contains("If") || clist[i].Name.Contains("While"))
                        condition_count++;
                if (clist[i].Name == "End")
                    end_count++;

            }
            if (condition_count == end_count)
                return true;
            else
                return false;
        }
        //---------------------------------------------------------------------------------------------------------------
        private void insertBlock()
        {
            for (int i = 0; i < clist.Count; i++)
            {
                if (y >= clist[i].Top && y < clist[i].Bottom)
                {
                    //if (!checkBracket())
                    //{
                    //    MessageBox.Show("「ここまで」ブロックを配置してから挿入位置を変えてね", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    break;
                    //}

                    //DialogResult result = MessageBox.Show(translate(clist[i].Name) + "のブロックの次に挿入しますか？", "けいこく", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    //if (result == DialogResult.Yes)
                    //{
                        setIndent(i, InsertPoint.After);
                        break;
                    //}
                    //else break;
                }

            }
        }
        //--------------------------------------------------------------------------------------------------------------
        //選択されたブロックの行にあるインデントの情報を設定
        private void setIndent(int point, InsertPoint where)
        {
            int first_point = point;
            bool firstisCondition = false;
            Stack<string> line_indents = new Stack<string>();

            if (clist[point].Name.Contains("If") || clist[point].Name.Contains("While"))
            {
                if (clist[point].Name.Contains("If"))
                    line_indents.Push("If");
                else if (clist[point].Name.Contains("While"))
                    line_indents.Push("While");

                firstisCondition = true;
            }


            if (point + 1 < clist.Count)
            {
                while (clist[point + 1].Name.Contains("Indent"))
                {
                    if (clist[point + 1].Name.Contains("If") && !clist[point + 1].Name.Contains("End"))
                        line_indents.Push("If");
                    else if (clist[point + 1].Name.Contains("While") && !clist[point + 1].Name.Contains("End"))
                        line_indents.Push("While");
                    point++;
                    if (point + 1 >= clist.Count)
                        break;
                }
            }

            indent_count = line_indents.Count; //インデント数の更新
            if (where == InsertPoint.Before)
                insert_point = first_point;
            else if (where == InsertPoint.After)
            {
                insert_point = first_point + indent_count + 1; //挿入開始位置の更新(後ろに挿入)
                if (firstisCondition)
                    insert_point--;
            }

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
                arrow_View();

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
                    case "IffrontWall":
                        _codeQueue.Enqueue(codeNumber + ":もし、正面が壁なら{");
                        codeNumber++;
                        break;
                    case "IfleftWall":
                        _codeQueue.Enqueue(codeNumber + ":もし、左が壁なら{");
                        codeNumber++;
                        break;
                    case "IfrightWall":
                        _codeQueue.Enqueue(codeNumber + ":もし、右が壁なら{");
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
                    case "WhilefrontWall":
                        _codeQueue.Enqueue(codeNumber + ":正面が壁なら繰り返す{");
                        codeNumber++;
                        break;
                    case "WhileleftWall":
                        _codeQueue.Enqueue(codeNumber + ":左が壁なら繰り返す{");
                        codeNumber++;
                        break;
                    case "WhilerightWall":
                        _codeQueue.Enqueue(codeNumber + ":右が壁なら繰り返す{");
                        codeNumber++;
                        break;
                    case "While":
                        _codeQueue.Enqueue(codeNumber + ":いつでも繰り返す{");
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
            arrow_View();
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
                MessageBox.Show("ブロックを配置しよう", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (clist.Last().Name == "Indent" || clist.Last().Name.Contains("If") || clist.Last().Name.Contains("While"))//コードの中かっこの数が正しくないときは、警告を出す
            {
                MessageBox.Show("「ここまで」ブロックが足りません", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!checkBracket())
            {
                MessageBox.Show("「ここまで」ブロックが足りません", "けいこく", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 中かっこの数が正しいかチェックする
        /// </summary>
        /// <returns></returns>
        private bool checkBracket()
        {
            Queue<string> code = block_to_queue();
            int countBracket = 0;
            foreach (string line in code)
            {
                if (line.Contains("{")) countBracket++;
                if (line.Contains("}")) countBracket--;
            }
            return countBracket == 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //ゲームのデータクラスの更新
                if (runAllTimer.Enabled == true)
                {
                    runAllTimer.Stop();//タイマーを停止する
                    safevelocityenabled();
                    button1.Enabled = true;//他のボタンを押せるようにする
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Text = "すべて実行";
                }
                else
                {
                    if (!validateCode()) return;//コードの形が正しくないときは何も実行しない
                    velocityBar1.Enabled = false;
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

        private void setArrowPicture()
        {
            if (this.panel1.InvokeRequired)
            {
                MyDelegate arrView = new MyDelegate(() => arrow_View());
                this.Invoke(arrView);
            }
            else { arrow_View(); }
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

        private void velocityBar1_Scroll(object sender, EventArgs e)
        {
            switch (velocityBar1.Value)
            {
                case 1: runAllTimer.Interval = 50; break;
                case 2: runAllTimer.Interval = 100; break;
                case 3: runAllTimer.Interval = 200; break;
                case 4: runAllTimer.Interval = 300; break;
                case 5: runAllTimer.Interval = 400; break;

            }
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            lineView(insert_point);
            DummyBlockView(x, y, (Bitmap)e.Data.GetData(DataFormats.Bitmap));
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            panel1.Controls.Remove(dummyBlock);
            panel1.Controls.Remove(line);

            //setDropTarget();
            insertBlock();
            block_Create(comand, insert_point);
            block_View(0);
            countBlocknumber();

            if (comand == Comands.If || comand == Comands.While)
            {
                //setDropTarget();
                comand = Comands.End;
                block_Create(comand, insert_point+1);
                block_View(0);
                countBlocknumber();
            }
            
        }

        private void setDropTarget()
        {
            for (int i = 0; i < clist.Count; i++)
            {
                if (y >= clist[i].Top && y < clist[i].Bottom)
                {
                   setIndent(i, InsertPoint.After);
                }
            }
        }

        private void panel1_DragLeave(object sender, EventArgs e)
        {
            panel1.Controls.Remove(dummyBlock);
            panel1.Controls.Remove(line);
        }

        private void panel1_DragOver(object sender, DragEventArgs e)
        {
            //panel1.Controls.Add(clist.Last());
            //clist.Last().Left = System.Windows.Forms.Cursor.Position.X;
            //clist.Last().Top = System.Windows.Forms.Cursor.Position.Y;
            int cu_x = System.Windows.Forms.Cursor.Position.X;
            int cu_y = System.Windows.Forms.Cursor.Position.Y;
            x = panel1.PointToClient(System.Windows.Forms.Cursor.Position).X; //スクリーン座標　⇒　クライエント座標
            y = panel1.PointToClient(System.Windows.Forms.Cursor.Position).Y;

            setDropTarget();//挿入位置の取得
            lineMove(insert_point);//挿入位置の追従

            DummyBlockMove(x, y);//挿入するブロックの追従
        }
        private void DummyBlockView(int left, int top, Bitmap bm)
        {
            dummyBlock.SizeMode = PictureBoxSizeMode.StretchImage;
            dummyBlock.Height = 40;
            dummyBlock.Width = 150;
            DummyBlockMove(left, top);
            dummyBlock.Image = bm;
            panel1.Controls.Add(dummyBlock);
            dummyBlock.BringToFront();//前面に表示
        }
        private void DummyBlockMove(int left, int top)
        {
            dummyBlock.Left = left;
            dummyBlock.Top = top;
        }
        private void lineView(int insert_p)
        {
            //System.Windows.Forms.Timer timer1;
            //timer1 = new System.Windows.Forms.Timer();
            //timer1.Enabled = true;
            //timer1.Interval = 200;
            //timer1.Tick += new System.EventHandler(timer1_Tick);
            line.SizeMode = PictureBoxSizeMode.StretchImage;
            line.Height = 10;
            line.Width = 150;
            lineMove(insert_p);
            line.Image = Properties.Resources.線２;
            panel1.Controls.Add(line);
            line.BringToFront();//前面に表示
            //timer1.Start();
        }
        private void lineMove(int insert_p)
        {
            //System.Windows.Forms.Timer timer1;
            //timer1 = new System.Windows.Forms.Timer();
            //timer1.Enabled = true;
            //timer1.Interval = 200;
            //timer1.Tick += new System.EventHandler(timer1_Tick);
            if (insert_p - 1 < 0) line.Left = 100;
            else if (clist.Count >= 1) {
                if (!clist[insert_p - 1].Name.Contains("Indent")) line.Left = clist[insert_p - 1].Left;
                else
                {
                    int insert_p2 = insert_p;//indentの時はindentでなくなるまで前に辿る
                    int _left = 0;
                    while (clist[insert_p2 - 1].Name.Contains("Indent"))
                    {
                        insert_p2--;
                        _left = clist[insert_p2 - 1].Left;
                    }
                    line.Left = _left;
                }
            }

            if (insert_p - 1 < 0) line.Top = 40;
            else if (clist.Count >= 1) line.Top = clist[insert_p - 1].Top + clist[insert_p - 1].Height;
            //timer1.Start();
        }

        //ドラッグの設定
        #region
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.Go;
            pictureBox1.DoDragDrop(pictureBox1.Image, DragDropEffects.Copy |
      DragDropEffects.Move);
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.Left;
            pictureBox2.DoDragDrop(pictureBox2.Image, DragDropEffects.Copy |
      DragDropEffects.Move);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.Right;
            pictureBox3.DoDragDrop(pictureBox3.Image, DragDropEffects.Copy |
      DragDropEffects.Move);
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox4.DoDragDrop(pictureBox4.Image, DragDropEffects.Copy |
      DragDropEffects.Move);
            condition = Conditions.Front_Wall;
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox5.DoDragDrop(pictureBox5.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Left_Wall;
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox6.DoDragDrop(pictureBox6.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Right_Wall;
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox7.DoDragDrop(pictureBox7.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Front_noWall;
        }

        private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox8.DoDragDrop(pictureBox8.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Left_noWall;
        }

        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.If;
            pictureBox9.DoDragDrop(pictureBox9.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Right_noWall;
        }

        private void pictureBox10_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox10.DoDragDrop(pictureBox10.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Front_Wall;
        }

        private void pictureBox11_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox11.DoDragDrop(pictureBox11.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Left_Wall;
        }

        private void pictureBox12_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox12.DoDragDrop(pictureBox12.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Right_Wall;
        }

        private void pictureBox13_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox13.DoDragDrop(pictureBox13.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Front_noWall;
        }

        private void pictureBox14_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox14.DoDragDrop(pictureBox14.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Left_noWall;
        }

        private void pictureBox15_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox15.DoDragDrop(pictureBox15.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Right_noWall;
        }

        private void pictureBox16_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.While;
            pictureBox16.DoDragDrop(pictureBox16.Image, DragDropEffects.Copy |
DragDropEffects.Move);
            condition = Conditions.Forever;
        }
        private void pictureBox17_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.End;
            pictureBox17.DoDragDrop(pictureBox17.Image, DragDropEffects.Copy |
DragDropEffects.Move);
        }

        private void pictureBox18_MouseDown(object sender, MouseEventArgs e)
        {
            comand = Comands.End;
            pictureBox18.DoDragDrop(pictureBox18.Image, DragDropEffects.Copy |
DragDropEffects.Move);
        }

        #endregion

    }
}