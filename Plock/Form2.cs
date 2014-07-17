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

        // List<Label> blockList = new List<Label>();
        //List<Block> block = new List<Block>();
        List<Block> blockList = new List<Block>();

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

        class Block
        {
            internal Label label = new Label();//ブロックが表示する文字
            public string Name;//ブロックの名前

            //描画のクラス
            internal PictureBox pictureBox = new PictureBox();//ブロックっぽい画像
            internal List<Block> indexList = new List<Block>();//インデックスのブロックを左から順に格納する

            /// <summary>
            /// ブロックの位置を設定する
            /// </summary>
            /// <param name="blockListindex">blockListのインデックス</param>
            public void setPositions(int blockListindex){
                for (int i = 0; i < indexList.Count; i++)
                {
                    pictureBox.Top = 14 + blockListindex * 40;
                    pictureBox.Height = 40;
                    pictureBox.Width = 150;
                    pictureBox.AutoSize = false;
                }
            }
        }

        /// <summary>
        /// ブロックを全て描画する
        /// </summary>
        internal void drawBlockList(){
            int top = 0;
            foreach(Block bList in blockList){
                bList.setPositions(top);
                panel1.Controls.Add(bList.pictureBox);
                foreach (Block iList in bList.indexList)
                {
                    panel1.Controls.Add(bList.pictureBox);
                }
                top++;
            }
        }

        //-------------------------------------------------------------------------------------------
        //命令文選択リスト
        enum Comands { Go, Left, Right, If, While, End };
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
        enum Conditions { Front_Wall, Left_Wall, Right_Wall };
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
        //配置ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            block_Create(comand);
            block_View(0);

        }
        //-------------------------------------------------------------------------------------------
        private void block_View(int k)//ピクチャーボックスを表示する
        {
            int top = 0;
            int y = 14;
            //int k=0;
            int pn_height = 0;
            for (int i = k; i < blockList.Count; i++)
            {
                if (blockList[i].pictureBox.Name == "Indent")
                {
                    k--;
                }
                top = y + k * 40;
                blockList[i].pictureBox.Top = top;
                blockList[i].pictureBox.Height = 40;
                blockList[i].pictureBox.Width = 150;
                blockList[i].pictureBox.AutoSize = false;
                // ドラッグ&ドロップを行なう時のドロップ先のコントロール（フォーム）に、ドロップを受け入れるように指示
                blockList[i].pictureBox.AllowDrop = true;

                //blockList[top].Select();
                //lb.Cursor
                //blockList[top].Font = new Font(blockList[top].Font.FontFamily, 16, FontStyle.Bold);

                panel1.Controls.Add(blockList[i].pictureBox);
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
            Block _block = new Block();
            //画像の大きさをPictureBoxに合わせる
            _block.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

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

                    _block.pictureBox.Image = Properties.Resources.前へ;
                    _block.pictureBox.Left = 10 + indent_count * indent_size;    //Left=10
                    _block.pictureBox.Name = "Go";
                    blockList.Add(_block);


                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                            pb1[count].Image = Properties.Resources.もしinterval;

                        if (indent_type == "While")
                            pb1[count].Image = Properties.Resources.繰り返しinterval;
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        blockList.Add(pb1[count]);
                        count++;
                    }
                    break;

                case Comands.End:
                    if (indent_count == 0) {setTextBox1("インデントのないときには使えない"); return;}//if文かWhile文が使われていないときは配置できないようにする
                    _block.Image = Properties.Resources.ここまで;
                    _block.Name = "End";
                    blockList.Add(_block);
                    indent_count--;
                    _block.Left = 10 + indent_count * indent_size;
                    //0708追加
                    while (count < indent_count)
                    {
                        indent_type = indent.Pop();
                        if (indent_type == "If")
                            pb1[count].Image = Properties.Resources.もしinterval;

                        if (indent_type == "While")
                            pb1[count].Image = Properties.Resources.繰り返しinterval;
                        indent.Push(indent_type);

                        pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                        pb1[count].Name = "Indent";
                        blockList.Add(pb1[count]);
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
                        _block.Image = Properties.Resources.もし正面壁なし;
                        _block.Left = 10 + indent_count * indent_size;
                        _block.Name = "Iffront";
                        blockList.Add(_block);
                        //0708追加
                        while (count < indent_count)
                        {

                            pb1[count].Image = Properties.Resources.もしinterval;
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            blockList.Add(pb1[count]);
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
                        _block.Image = Properties.Resources.繰り返し正面壁なし;
                        _block.Left = 10 + indent_count * indent_size;
                        _block.Name = "Whilefront";
                        blockList.Add(_block);
                        //0708追加
                        while (count < indent_count)
                        {
                            pb1[count].Image = Properties.Resources.繰り返しinterval;
                            pb1[count].Left = 10 + (indent_count - count - 1) * indent_size;    //Left=10
                            pb1[count].Name = "Indent";
                            blockList.Add(pb1[count]);
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
        //ブロックの取り消し
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

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
            for (int i = 0; i < blockList.Count; i++)
            {
                switch (blockList[i].Name)
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
                if (runAllTimer.Enabled==true)
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
                forSetTextBox1 setTex1 = new forSetTextBox1(() => textBox1.Text=str);
                this.Invoke(setTex1);
            }
            else { this.textBox1.Refresh(); }
        }

    }
}
