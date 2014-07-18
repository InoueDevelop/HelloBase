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

            //描画のクラス
            internal PictureBox pictureBox = new PictureBox();//ブロックっぽい画像
            internal List<Block> indentList = new List<Block>();//インデントのブロックを左から順に格納する
            internal Comands comand;
            internal Conditions condition;

            public Block(Comands _comand, Conditions _condition)
            {
                this.comand = _comand;
                this.condition = _condition;
            }
            internal Block buildNewBlock(Comands _comand, Conditions _condition)
            {
                Block newBlock = new Block(_comand, _condition);//新しいブロックを生成
                for (int i = 0; i < indentList.Count(); i++) { newBlock.indentList.Add(new Block(indentList[i].comand, indentList[i].condition)); }//インデントを流用
                if (comand == Comands.If || comand == Comands.While) newBlock.indentList.Add(new Block(comand, condition));//このブロックが条件文ならインデントを追加
                return newBlock;
            }

            /// <summary>
            /// 命令文とインデントのブロックの位置を設定する
            /// </summary>
            /// <param name="blockListindex">blockListのインデックス</param>
            public void setPositions(int blockListindex)
            {
                int indent_width = 75;

                pictureBox.Top = 14 + blockListindex * 40;
                pictureBox.Left = 10 + indentList.Count * indent_width;

                pictureBox.Height = 40;
                pictureBox.Width = 150;
                pictureBox.AutoSize = false;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                //画像を選ぶ
                switch (comand)
                {
                    case Comands.Go: pictureBox.Image = Properties.Resources.前へ; break;
                    case Comands.Left: pictureBox.Image = Properties.Resources.左; break;
                    case Comands.Right: pictureBox.Image = Properties.Resources.右; break;
                    case Comands.End: pictureBox.Image = Properties.Resources.ここまで; break;
                    case Comands.If: switch (condition)
                        {
                            case Conditions.Front_Wall: pictureBox.Image = Properties.Resources.もし正面壁なし; break;
                            case Conditions.Left_Wall: pictureBox.Image = Properties.Resources.もし左壁なし; break;
                            case Conditions.Right_Wall: pictureBox.Image = Properties.Resources.もし右壁なし; break;
                        } break;
                    case Comands.While: switch (condition)
                        {
                            case Conditions.Front_Wall: pictureBox.Image = Properties.Resources.繰り返し正面壁なし; break;
                            case Conditions.Left_Wall: pictureBox.Image = Properties.Resources.繰り返し左壁なし; break;
                            case Conditions.Right_Wall: pictureBox.Image = Properties.Resources.繰り返し右壁なし; break;
                        } break;
                }

                //インデントにも画像を設定する
                for (int i = 0; i < indentList.Count; i++)
                {
                    indentList[i].pictureBox.Top = pictureBox.Top;
                    indentList[i].pictureBox.Left = 10 + i * indent_width;

                    indentList[i].pictureBox.Height = pictureBox.Height;
                    indentList[i].pictureBox.Width = pictureBox.Width;
                    indentList[i].pictureBox.AutoSize = pictureBox.AutoSize;
                    indentList[i].pictureBox.SizeMode = pictureBox.SizeMode;
                    if (indentList[i].comand == Comands.If) indentList[i].pictureBox.Image = Properties.Resources.もしinterval;
                    else if (indentList[i].comand == Comands.While) indentList[i].pictureBox.Image = Properties.Resources.繰り返しinterval;
                }
            }
        }

        /// <summary>
        /// ブロックを全て描画する
        /// </summary>
        internal void drawBlockList()
        {
            int top = 0;
            foreach (Block bList in blockList)
            {
                bList.setPositions(top);
                panel1.Controls.Add(bList.pictureBox);
                foreach (Block iList in bList.indentList)
                {
                    panel1.Controls.Add(iList.pictureBox);
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
                case 0: comand = Comands.Go; break;
                case 1: comand = Comands.Left; break;
                case 2: comand = Comands.Right; break;
                case 3: comand = Comands.If; break;
                case 4: comand = Comands.While; break;
                case 5: comand = Comands.End; break;
            }
        }
        //-------------------------------------------------------------------------------------------
        //条件文選択リスト
        enum Conditions { Front_Wall, Left_Wall, Right_Wall };
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBox3.SelectedIndex)
            {
                case 0: condition = Conditions.Front_Wall; break;
                case 1: condition = Conditions.Left_Wall; break;
                case 2: condition = Conditions.Right_Wall; break;
            }
        }

        //-------------------------------------------------------------------------------------------
        //配置ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            //ブロックが何もないときは新しいBlockを作る。既にブロックがあるときは、インデント部分を流用してBlockを作る
            if (blockList.Count == 0)blockList.Add(new Block(comand, condition));
            else blockList.Add(blockList[blockList.Count()-1].buildNewBlock(comand, condition));

            drawBlockList();
        }

        //-------------------------------------------------------------------------------------------

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

        /// <summary>
        /// コード用の文字列を返す
        /// </summary>
        /// <returns></returns>
        private Queue<string> block_to_queue()
        {
            Queue<string> _codeQueue = new Queue<string>();

            for (int i = 0; i < blockList.Count; i++)
            {
                String codeWord = "";
                switch (blockList[i].comand)
                {
                    case Comands.Go: codeWord = ":前へ進む"; break;
                    case Comands.Left: codeWord = ":左を向く"; break;
                    case Comands.Right: codeWord = ":右を向く"; break;
                    case Comands.End: codeWord = ":}"; break;
                    case Comands.If: switch (blockList[i].condition)
                        {
                            case Conditions.Front_Wall: codeWord = ":もし、正面に壁がないなら{"; break;
                            case Conditions.Left_Wall: codeWord = ":もし、左に壁がないなら{"; break;
                            case Conditions.Right_Wall: codeWord = ":もし、右に壁がないなら{"; break;
                        } break;
                    case Comands.While: switch (blockList[i].condition)
                        {
                            case Conditions.Front_Wall: codeWord = ":正面に壁がないなら繰り返す{"; break;
                            case Conditions.Left_Wall: codeWord = ":左に壁がないなら繰り返す{"; break;
                            case Conditions.Right_Wall: codeWord = ":右に壁がないなら繰り返す{"; break;
                        } break;
                }
                _codeQueue.Enqueue(i + codeWord);
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
