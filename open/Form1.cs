using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace open
{
    public partial class Open : Form
    {
        Bitmap bmp;
        Timer timer = new Timer();
        int count = 0;
        int[,] r;
        int[,] g;
        int[,] b;
        public bool cFlag = false;
        public bool eFlag = false;

        public Open()
        {
            InitializeComponent();
            timer.Enabled = true;
            timer.Interval = 55;  // 更新間隔 (ミリ秒)　精度の上限は55ms
            // タイマ用のイベントハンドラを登録
            //timer.Tick += new EventHandler(timer_Tick);
            // タイマ用のイベントハンドラをフォームにも登録
            this.Load += new EventHandler(timer_Tick);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//画像の大きさをpictureBoxに合わせる
            bmp = Properties.Resources.intro;
            r =new int[bmp.Width, bmp.Height];
            g =new int[bmp.Width, bmp.Height];
            b =new int[bmp.Width, bmp.Height];
            pictureBox1.Controls.Add(label1);//labelの透過処理
            pictureBox1.Controls.Add(label2);
            pictureBox1.Controls.Add(label3);
            pictureBox1.Controls.Add(label4);
            count = 24;
            timer.Start();  // タイマ ON
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (count == 0)
            {
                bmp = Properties.Resources.intro;
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        r[i, j] = (bmp.GetPixel(i, j).R);
                        g[i, j] = (bmp.GetPixel(i, j).G);
                        b[i, j] = (bmp.GetPixel(i, j).B);
                    }
                }
            }

            if (count <= 8)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(r[i, j] / 8 * count, g[i, j] / 8 * count, b[i, j] / 8 * count));
                    }
                }
            }

            else if (count > 8 && count <= 16)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(r[i, j] / 8 * (count - ((count - 8) * 2 - 1)), g[i, j] / 8 * (count - ((count - 8) * 2 - 1)), b[i, j] / 8 * (count - ((count - 8) * 2 - 1))));
                    }
                }
            }

            else if (count > 16 && count <= 22)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(0,0,0));
                    }
                }
                string[] load;
                load = new string[6] { "","ロ", "ー", "ド", "中", "…" }; 
                label1.Text += load[count-17];
            }

            else if (count == 23)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }


           
            if (count > 23)

            {
                bmp = Properties.Resources.background;
                Graphics gra = Graphics.FromImage(bmp);
                label1.Text = "";
                label2.Text = "スタート";
                label3.Text = "終了";
                label4.Text = "はじめてのプログラミング";
                if (count == 24) timer.Stop();
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//画像の大きさをpictureBoxに合わせる
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            count++;

        }

        
        private void Open_FormClosed(object sender, FormClosedEventArgs e)
        {
            cFlag = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            cFlag = false;
            eFlag = true;

            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
