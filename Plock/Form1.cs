using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace Plock
{
    using GameForm = HelloMaze.BoardData;//TODO:利用したいゲームのFormを登録
    using System.Threading;

    public class InterpriterController:Form
    {
        internal GameForm gameForm;

        internal GameInterpriter gameInterpriter;

        internal System.Timers.Timer runAllTimer;

        public InterpriterController(){
            //ゲームのFormのインスタンスを生成
            gameForm = new GameForm();
            gameForm.Show();

            //インタプリタの実体を生成
            gameInterpriter = new GameInterpriter();

            //自動実行のためのTimerを用意
            runAllTimer = new System.Timers.Timer();
            runAllTimer.Elapsed += (object o, System.Timers.ElapsedEventArgs eea) => { gameInterpriter.runOneLine("", gameForm); if (gameForm.locked == true)runAllTimer.Stop(); };
            runAllTimer.AutoReset = true;
            runAllTimer.Interval =400;
            

        }

        internal void RunAll(string code)
        {
            gameInterpriter.build(code);
            //ゲームのデータクラスの更新
            if (runAllTimer.Enabled == true)
            {
                runAllTimer.Stop();
            }
            else
            {
                runAllTimer.Start();
            }
        }

        internal void RunAll(Queue<String> code)
        {
            gameInterpriter.build(code);
            //ゲームのデータクラスの更新
            if (runAllTimer.Enabled == true)
            {
                runAllTimer.Stop();
            }
            else
            {
                runAllTimer.Start();
            }
        }
    }


    public partial class Form1 : InterpriterController
    {
        //GameForm gameForm;

        //GameInterpriter gameInterpriter;

        //Thread forRunAll;

        public Form1():base()
        {
            InitializeComponent();

            ////ゲームのFormのインスタンスを生成
            //gameForm = new GameForm();
            //gameForm.Show();

            ////インタプリタの実体を生成
            //gameInterpriter = new GameInterpriter();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunAll(textBox1.Text);

            //ゲームのデータクラスの更新
            //gameForm=gameInterpriter.run(textBox1.Text, gameForm);

            //表示の更新(refreshObjectは未完成のメソッド)
            //gameForm.refreshObject(gameForm.controlobj);
        }

        //private void RunAll()
        //{
        //    //ゲームのデータクラスの更新
        //    if (forRunAll != null && forRunAll.IsAlive)
        //    {
        //        //無視する
        //    }
        //    else
        //    {
        //        forRunAll = new Thread(new ThreadStart(() => { gameInterpriter.run(textBox1.Text, gameForm); }));
        //        forRunAll.Start();//開始する
        //    }
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ゲームのデータクラスの更新
            gameForm = gameInterpriter.runOneLine(textBox1.Text, gameForm);
            ////表示の更新(refreshObjectは未完成のメソッド)
            //gameForm.refreshObject(gameForm.controlobj);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gameInterpriter.build(textBox1.Text);
        }
    }
}
