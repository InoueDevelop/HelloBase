using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BitmapPaint;

namespace Plock
{
    

    /// <summary>
    /// BoardDataを管理するクラス
    /// </summary>
    public partial class BoardData : Form, BoardPosition
    {
        static int tutorialcount = 0;
        static int tutorial = 0;
        [Serializable]
        public class Dataset
        {
            public int cpsquarelength;
            public bool[,] cpCanPutObjectOnBoard;
            public BoardObject cpcontrolobj;
            public List<BoardObject> cp_ListObjectBoard;
            public Bitmap cpback;
            public Bitmap cpfore;
          
           
            public Dataset(BoardData sdata)
            {
                cpsquarelength = sdata._sql;
                cpback = sdata.back;
                cpfore = sdata.fore;
                cpcontrolobj = sdata.controlobj;
                cpCanPutObjectOnBoard = sdata.BoardObjectCanMove;
                cp_ListObjectBoard = new List<BoardObject>();
              
            }

        }

        #region //fieldおよびプロパティ
        private static int squarelength = 40;
        private int BoardSizeWidth;
        private int BoardSizeHeight;
        private int gridsizewidth;
        private int gridsizeheight;
        public bool[,] CanPutObjectOnBoard;
        public BoardObject controlobj;
        private List<BoardObject> _ListObjectBoard = new List<BoardObject>();
        public BitmapPaintClass bmppaint = new BitmapPaintClass(squarelength);
        Bitmap back;
        delegate void RefreshPictureBox1();
        delegate void stagepresent();
        public Bitmap fore;
        public bool locked = false;
        Point sp;    //イベント発生時に保持されるマウスの画面座標
        public int stagecount = 21;
        public bool gostage = false;

        int setswitch=(int) set.None;
        public int _sql
        {
            get { return squarelength; }
        }

        public int BoardPositionXmax
        {
            get { return gridsizewidth; }
        }

        public int BoardPositionYmax
        {
            get { return gridsizeheight; }
        }
        public List<BoardObject> cplist
        {
            get { return _ListObjectBoard; }
        }


        public bool[,] BoardObjectCanMove
        {
            get { return CanPutObjectOnBoard; }
            set { CanPutObjectOnBoard = value; }
        }
        public List<BoardObject> ListObjectBoard
        {
            get { return _ListObjectBoard; }
            set { _ListObjectBoard = value; }
        }

        enum set{
            Wall,
            Enemy,
            Item,
            Goal,
            Del,
            None
        }

		private bool contextFlag = false;

           

        #endregion

		#region //コンストラクタ
		public BoardData() 
        {
            InitializeComponent();
            constructer();
           
			

        }

        public void constructer()
        {
            setswitch = (int)set.None;
            settingobj.Text = "";
            dragevent = new System.Threading.Timer(new System.Threading.TimerCallback(called), null, System.Threading.Timeout.Infinite, 50);
            locked = false;
            ListObjectBoard = new List<BoardObject>();
            back = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            fore = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            stagecount = 1;
            button2.Text = "次へ";
            button2.Visible=false;


            this.pictureBox1.BackgroundImage = back;
            this.pictureBox1.Image = fore;

            this.KeyPreview = true;

            
            
            Graphics g = Graphics.FromImage(back);

            BoardSizeWidth = pictureBox1.Width;
            BoardSizeHeight = pictureBox1.Height;
            gridsizewidth = BoardSizeWidth / squarelength;
            gridsizeheight = BoardSizeHeight / squarelength;

            CanPutObjectOnBoard = new bool[gridsizewidth, gridsizeheight];


            for (int i = 0; i < gridsizewidth; i++)
            {
                for (int j = 0; j < gridsizeheight; j++)
                {

                    CanPutObjectOnBoard[i, j] = true;


                    g.FillRectangle(Brushes.White, i * squarelength, j * squarelength, squarelength, squarelength);

                }
            }

            controlobj = new PlayerObject(gridsizeheight / 2-1, gridsizeheight / 2);
            ListObjectBoard.Add(controlobj);
            bmppaint.ObjectSetPaint(controlobj.ObjectPositionX, controlobj.ObjectPositionY, fore, ref CanPutObjectOnBoard, controlobj.ObjectSelectNum);

            //pictureBox1.Refresh();
            refreshPictureBox1();
            g.Dispose();

            try
            {
               
            }
            catch (FileNotFoundException)
            {
                using (StreamWriter sw = new StreamWriter("Userdata/clear.csv"))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        sw.WriteLine(0);
                    }
                }

            }

			(tabControl1.TabPages[1] as Control).Enabled = false;
        }

#endregion



        public void GetCursolPosition(int X, int Y, ref int x, ref int y)//クライアントを盤面座標に直すメソッド
        {
            for (int i = 0; i < gridsizewidth; i++)
            {
                for (int j = 0; j < gridsizeheight; j++)
                {
                    if ((i * squarelength < X && X < (i + 1) * squarelength) && (j * squarelength < Y && Y < (j + 1) * squarelength))
                    {
                        x = i;
                        y = j;
                    }
                }
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)//debugマウスクリックによるオブジェクトの操作権限の移行
        {
            //int x = -1;
            //int y = -1;

            //List<int> checkman = new List<int>();


            //Point sp = System.Windows.Forms.Cursor.Position;
            //System.Drawing.Point cp = pictureBox1.PointToClient(sp);

            //GetCursolPosition(cp.X, cp.Y, ref x, ref y);  
            //squareX.Text = "squareX:" + x;
            //squareY.Text = "squareY:" + y;



            //if (-1 < x)
            //{
            //    //bmppaint.PointSquare(x,y,fore);
            //    //pictureBox1.Refresh();

            //    switch (CanPutObjectOnBoard[x, y])
            //    {
            //        case (false):
            //            {
            //                if (ListObjectBoard != null)
            //                {

            //                    controlobj = ListObjectBoard.Find(p => p.ObjectPositionX == x && p.ObjectPositionY == y&&(p is PlayerObject ||p is EnemyObject||p is WallObject));
            //                }

            //                break;
            //            }

            //        case (true):
            //            {
            //                //bmppaint.ObjectSetPaint(x, y, fore, ref CanPutObjectOnBoard,wall.ObjectSelectNum);
            //                //pictureBox1.Refresh();
            //                break;
            //            }
            //    }
            //}
        }


        public int CountToObject(int x, int y, int directionnum)
        {
            int count=0;

            switch (directionnum)
            {


                case 1:
                    for (count = 1; x + count < BoardPositionXmax; count++)
                    {
                        if (BoardObjectCanMove[x + count, y] == false)
                        {
                            return count;
                        }
                    }
                    break;

                case 2:
                    for (count = 1; y + count < BoardPositionYmax; count++)
                    {
                        if (BoardObjectCanMove[x, y + count] == false)
                        {
                            return count;
                        }
                    }
                    break;

                case 3:

                    for (count = 1; x - count >= 0; count++)
                    {
                        if (BoardObjectCanMove[x - count, y] == false)
                        {
                            return count;
                        }
                    }
                    break;
                case 4:
                    for (count = 1; y - count >= 0; count++)
                    {
                        if (BoardObjectCanMove[x, y - count] == false)
                        {
                            return count;
                        }
                    }
                    break;
            }
            if (count == 0) throw new Exception("countToObjectにエラー");
            return count;
        }


        private void BoardData_KeyDown(object sender, KeyEventArgs e) //十字キー入力後オブジェクトを移動するメソッド
        {
            if (e.KeyCode == Keys.X)  //right
            {
                MoveOperation(controlobj, 2, 1);
                controlobj.objectDirection = (int)BoardObject.ObjectDirection.Right;
            }
            if (e.KeyCode == Keys.A) //left
            {

                MoveOperation(controlobj, 3, 1);
                controlobj.objectDirection = (int)BoardObject.ObjectDirection.Left;
            }
            if (e.KeyCode == Keys.S)//Up
            {
                MoveOperation(controlobj, 1, 1);
                controlobj.objectDirection = (int)BoardObject.ObjectDirection.Up;
            }
            if (e.KeyCode == Keys.Z) //down
            {
                MoveOperation(controlobj, 4, 1);
                controlobj.objectDirection = (int)BoardObject.ObjectDirection.Down;
            }
        }

        public void moveStraight()////ブロックスクリプト用移動命令に向きの概念を追加
        {
            switch (controlobj.objectDirection)
            {
                case (int)BoardObject.ObjectDirection.Up:
                    MoveOperation(controlobj, 1, 0);
                    break;
                case (int)BoardObject.ObjectDirection.Down:
                    MoveOperation(controlobj, 4, 0);
                    break;
                case (int)BoardObject.ObjectDirection.Right:
                    MoveOperation(controlobj, 2, 0);
                    break;
                case (int)BoardObject.ObjectDirection.Left:
                    MoveOperation(controlobj, 3, 0);
                    break;
            }
        }
        /// <summary>
        /// ブロックスクリプト用上回転命令
        /// </summary>
        public void turnRight() { controlobj.turnRight(); directionchange();}
        public void turnLeft() { controlobj.turnLeft(); directionchange();}

        public void MoveOperation(BoardObject obj, int directionselect, int repititionnum)  //ブロックスクリプト用移動命令
        {
       
          lock(this){
              if (locked == true) return;

              switch (directionselect)
            {
                case 1:
                    if (0 < controlobj.ObjectPositionY && BoardObjectCanMove[controlobj.ObjectPositionX, controlobj.ObjectPositionY - 1] == true)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bmppaint.ObjectMovePaint(obj.ObjectPositionX, obj.ObjectPositionY, fore, back,obj.ObjectSelectNum, ref CanPutObjectOnBoard, 1, i);
                            fore.MakeTransparent(Color.White);
                            //pictureBox1.Refresh();
                            refreshPictureBox1();
                            System.Threading.Thread.Sleep(1);

                        }
                        obj.moveUp();
                        CanPutObjectOnBoard[obj.ObjectPositionX, obj.ObjectPositionY] = false;
                        gameevent();
                    }
                    break;

                case 2:
                    if (controlobj.ObjectPositionX < BoardPositionXmax - 1 && BoardObjectCanMove[controlobj.ObjectPositionX + 1, controlobj.ObjectPositionY] == true)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bmppaint.ObjectMovePaint(obj.ObjectPositionX, obj.ObjectPositionY, fore,back, obj.ObjectSelectNum, ref CanPutObjectOnBoard, 2, i);
                            fore.MakeTransparent(Color.White);
                            //pictureBox1.Refresh();
                            refreshPictureBox1();
                            System.Threading.Thread.Sleep(1);

                        }

                        obj.moveRight();
                        CanPutObjectOnBoard[obj.ObjectPositionX, obj.ObjectPositionY] = false;
                        gameevent();
                    }
                    break;

                case 3:
                    if (0 < controlobj.ObjectPositionX && BoardObjectCanMove[controlobj.ObjectPositionX - 1, controlobj.ObjectPositionY] == true)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bmppaint.ObjectMovePaint(obj.ObjectPositionX, obj.ObjectPositionY, fore,back, obj.ObjectSelectNum, ref CanPutObjectOnBoard, 3, i);
                            fore.MakeTransparent(Color.White);
                            //pictureBox1.Refresh();
                            refreshPictureBox1();
                            System.Threading.Thread.Sleep(1);

                        }
                        obj.moveLeft();
                        CanPutObjectOnBoard[obj.ObjectPositionX, obj.ObjectPositionY] = false;
                        gameevent();
                    }
                    break;

                case 4:
                    if (controlobj.ObjectPositionY < BoardPositionYmax - 1 && BoardObjectCanMove[controlobj.ObjectPositionX, controlobj.ObjectPositionY + 1] == true)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bmppaint.ObjectMovePaint(obj.ObjectPositionX, obj.ObjectPositionY, fore,back, obj.ObjectSelectNum, ref CanPutObjectOnBoard, 4, i);
                            fore.MakeTransparent(Color.White);
                            //pictureBox1.Refresh();
                            refreshPictureBox1();
                            System.Threading.Thread.Sleep(1);
                        }
                        obj.moveDown();
                        CanPutObjectOnBoard[obj.ObjectPositionX, obj.ObjectPositionY] = false;
                        gameevent();
                    }
                    break;
            

            }
          }
            }

        void gameevent() {
            bool genoside = false;
            
            foreach (var n in ListObjectBoard)
            {
                if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                {
                    if(stagecount!=30)
                    {
                        RefreshPictureBox1 refreshPic1 = new RefreshPictureBox1(() =>
                        {
                            pictureBox3.Image = Properties.Resources.goal;
                            button2.Text = "次へ";
                            if(stagecount == 20)
                            {
                                stagecount = 0;
                                button2.Text = "はじめから";
                                pictureBox3.Image = Properties.Resources.perfectgoal;
                            }
                            pictureBox3.Visible = true;
                            button2.Visible = true;
                            tabControl1.SelectedIndex = 1;
                            pictureBox3.Visible = true;
                            pictureBox3.Size = new Size(600, 600);
                            pictureBox3.Location = new Point(0, 50);
                            

                        locked = true;
                        });
                        this.Invoke(refreshPic1);
                        //locked = true;
                        //Task goalevent = new Task(() => { Goalevent(); });
                        //goalevent.Start();
                        //Goalevent();
                    }
                    else
                    {
                        //Goalevent();
                        RefreshPictureBox1 refreshPic1 = new RefreshPictureBox1(() =>
                        {
                            pictureBox3.Image = Properties.Resources.goal;
                            pictureBox3.Visible = true;
                            locked = true;
                            tabControl1.SelectedIndex = 1;
                            richTextBox1.Text = "ゴールできたよ！\n次へを押してね！";
                        });
                        this.Invoke(refreshPic1);
                    }
                    
 
                }

                if (n is ItemObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                {
                    genosideenemy();
                    if (genoside == false) genoside = true;
                }
            }
            if (genoside == true)
            {
                ListObjectBoard.RemoveAll(p => p is EnemyObject);
                refreshPictureBox1();
            }
        }         //ゲーム中に起こるイベントのまとめ

        /// <summary>
        /// スレッドセーフなPictureBox1.Refresh()
        /// </summary>
        private void refreshPictureBox1()
        {
            if (this.pictureBox1.InvokeRequired)
            {
                RefreshPictureBox1 refreshPic1 = new RefreshPictureBox1(() => pictureBox1.Refresh());
                this.Invoke(refreshPic1);
                 
                
            }
            else { this.pictureBox1.Refresh(); }
        }


           public void genosideenemy()
        {       
            foreach (var t in ListObjectBoard)
            {
                if (t is EnemyObject)
                {
                    bmppaint.ResetObject(ref CanPutObjectOnBoard, fore, t.ObjectPositionX, t.ObjectPositionY);
                                    }   
            } 
        }

       
           public void Goalevent() {

			   //writer(stagecount-1);

                   stagecount++;

                   locked = true;

                   //ClearForm clearwindow = new ClearForm(stagecount-1);


                   //if (stagecount == 21)
                   //{
                   //    clearwindow.Loadtext = "全クリア！！";
                   //    stagecount = 1;
                   //}
                   //clearwindow.ShowDialog();


                   stagepresent st = new stagepresent(() =>
                   {
                       button2.Visible = true;
                   });
                   this.Invoke(st);
               if(gostage == true)
               {
                   if (this.stage.InvokeRequired)
                   {
                       stagepresent stapre = new stagepresent(() => stage.Text = "現在のステージ:" + stagecount);
                       this.Invoke(stapre);

                       

                   }
                   else
                   {
                       stage.Text = "現在のステージ:" + stagecount;
                   }
                   string pathnext = "stage" + stagecount.ToString();
                   byte[] da = (byte[])Properties.Resources.ResourceManager.GetObject(pathnext);
                   try
                   {
                       locked = false;

                       loadDataset2(pathnext, da);

                   }
                   catch (Exception exc)
                   {

                   }
               }
                   //if (clearwindow.newgamestart == true)
                   //{
                   //    if (this.pictureBox1.InvokeRequired)
                   //    {
                   //        RefreshPictureBox1 constr = new RefreshPictureBox1(() => constructer());
                   //        this.Invoke(constr);
                   //    }
                   //    else {constructer(); }
                   //}
                   //else if (clearwindow.Loaddatastart == true)
                   //{

                   //    if (this.stage.InvokeRequired)
                   //    {
                   //        stagepresent stapre = new stagepresent(() => stage.Text = "現在のステージ:" + stagecount);
                   //        this.Invoke(stapre);
                   //    }
                   //    else
                   //    {
                   //        stage.Text = "現在のステージ:" + stagecount;
                   //    }
                   //    string pathnext = "stage" + stagecount.ToString();
                   //    byte[] da = (byte[])Properties.Resources.ResourceManager.GetObject(pathnext);
                   //    try
                   //    {
                   //        locked = false;

                   //        loadDataset2(pathnext, da);

                   //    }
                   //    catch (Exception exc)
                   //    {

                   //    }




                   //}
                   else { locked = false; }
                   //clearwindow.Dispose();
               
           } //ゴールに重なった時のイベント


        public void directionchange ()
        {
            bmppaint.playerdirectionchange(controlobj.objectDirection,fore,controlobj.objectPositionX,controlobj.objectPositionY);
            if (tutorial == 6 && tutorialcount == 1)
            {
                if (controlobj.objectDirection == 1)
                {
                    RefreshPictureBox1 refreshPic2 = new RefreshPictureBox1(() =>
                    {
                        richTextBox1.Text = "ゴールを向けたよ！\n次へを押してね！";
                        tabControl1.SelectedIndex = 1;
                    });
                    this.Invoke(refreshPic2);
                }
            }
            refreshPictureBox1();
        }

        /// <summary>
        /// 統一的に使える画面更新のメソッドを作る必要がある(未完成)
        /// </summary>
        /// <param name="obj"></param>
        public void refreshObject(BoardObject obj)
        {

            bmppaint.ObjectMovePaint(controlobj.ObjectPositionX, controlobj.ObjectPositionY, fore,back, controlobj.ObjectSelectNum, ref CanPutObjectOnBoard, 2, 1);
            pictureBox1.Refresh();
        }


        public void ObjectSet(int x, int y, int ObjectSelectNum)       //ブロックスクリプト用配置命令
        {
            if ((controlobj.ObjectPositionX == x && controlobj.ObjectPositionY == y) || (ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionX == x && ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionY == y)) { return; }
            ListObjectBoard.RemoveAll(p => p.ObjectPositionX == x && p.ObjectPositionY == y);
            CanPutObjectOnBoard[x, y] = true;
            
            BoardObject NewObject = new BoardObject();
            switch (ObjectSelectNum)
            {
                case 0: NewObject = new WallObject(x, y); ListObjectBoard.Add(NewObject);
                    break;
                case 1: if (controlobj is PlayerObject == false) CanPutObjectOnBoard[controlobj.ObjectPositionX, controlobj.ObjectPositionY] = false;
                    bmppaint.ResetObject(ref CanPutObjectOnBoard, fore, ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionX, ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionY);
                    controlobj = new PlayerObject(x, y);
                    NewObject = controlobj; ListObjectBoard[0] = controlobj;
                    break;
                case 2: NewObject = new EnemyObject(x, y); ListObjectBoard.Add(NewObject);
                    break;
                case 3: NewObject = new ItemObject(x, y); ListObjectBoard.Add(NewObject);
                    break;
                case 4: NewObject = new GoalObject(x, y); ListObjectBoard.Add(NewObject);
                    break;
            }
            if (NewObject is GoalObject || NewObject is ItemObject)
            {
                bmppaint.ResetObject(fore,x,y);
                fore.MakeTransparent(Color.White);
                bmppaint.ObjectSetPaint(NewObject.ObjectPositionX, NewObject.ObjectPositionY, back, ref CanPutObjectOnBoard, NewObject.ObjectSelectNum);
            }
            else
            {
                bmppaint.ResetObject(back, x, y,0,0);
                bmppaint.ObjectSetPaint(NewObject.ObjectPositionX, NewObject.ObjectPositionY, fore, ref CanPutObjectOnBoard, NewObject.ObjectSelectNum);
            }
            pictureBox1.Refresh();

        }

        #region  //コンテキストメニュー関連のメソッド
        private void PutPlayerToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)  //コンテキストメニューで主人公を置く
        {
            int x = -1;
            int y = -1;
            int objectselectnum = 1;

           

            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
            if (x > -1)
            {
                
                ObjectSet(x, y, objectselectnum);
            
            }
        }

        private void PutEnemyToolStripMenuItem_Click(object sender, EventArgs e)  //コンテキストメニューで敵を置く
        {
            int x = -1;
            int y = -1;
            int objectselectnum = 2;


            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
            if (x > -1)
            {
               
               // ListObjectBoard.RemoveAll(p => p.ObjectPositionX == x && p.ObjectPositionY == y);
              　 ObjectSet(x, y, objectselectnum);
      　　　　

            }
        }

        private void Object_Control_Menu_Opened(object sender, EventArgs e)//コンテキストメニューを開いた時のマウス座標を記録
        {
            sp = Control.MousePosition;
        }

        private void PutWalltoolStripMenuItem2_Click(object sender, EventArgs e)//コンテキストメニューで壁を置く
        {
            int x = -1;
            int y = -1;
            int objectselectnum = 0;


            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
            if (x > -1)
            {
                ObjectSet(x, y, objectselectnum); 
             
              
            }
        }

        void deleteobj(int x, int y) {
            
            if (x > -1)
            {
                if ((controlobj.ObjectPositionX == x && controlobj.ObjectPositionY == y) || (ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionX == x && ListObjectBoard.Find(p => p is PlayerObject).ObjectPositionY == y)) { return; }



                if (
                    //ListObjectBoard.Find(p => p.ObjectPositionX == x && p.ObjectPositionY == y) is PlayerObject
                    //||
                    ListObjectBoard.Find(p => p.ObjectPositionX == x && p.ObjectPositionY == y) is EnemyObject
                    || ListObjectBoard.Find(p => p.ObjectPositionX == x && p.ObjectPositionY == y) is WallObject
                    )
                {
                    bmppaint.ResetObject(ref CanPutObjectOnBoard, fore, x, y);

                }
                else
                {
                    bmppaint.ResetObject(ref CanPutObjectOnBoard, back, x, y);
                }
                ListObjectBoard.RemoveAll(p => p.ObjectPositionX == x && p.ObjectPositionY == y);
                pictureBox1.Refresh();
            }
        }
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)//コンテキストメニューでオブジェクトを削除
        {
            int x = -1;
            int y = -1;

            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);

            deleteobj(x,y);
           
        }

        private void PutItemToolStripMenuItem_Click(object sender, EventArgs e)//コンテキストメニューでアイテムを置く
        {
            int x = -1;
            int y = -1;
            int objectselectnum = 3;


            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
            if (x > -1)
            {
              //  bmppaint.ResetObject(fore, x, y, 0, 0);
              //  fore.MakeTransparent();
                ObjectSet(x, y, objectselectnum);
            }

        }


        private void GoaltoolStripMenuItem2_Click(object sender, EventArgs e) //Goalを置く
        {
            int x = -1;
            int y = -1;
            int objectselectnum = 4;


            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
            if (x > -1)
            {
                ObjectSet(x, y, objectselectnum);
            }

        }
        #endregion

        #region //データ管理
        private void save_button_Click(object sender, EventArgs e)
        {
            save();
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            load();
        }

       
        Dataset stateHistory;

        void save()
        {
            stateHistory = new Dataset(this);

            stateHistory.cp_ListObjectBoard = ListObjectBoard;

            System.IO.Directory.CreateDirectory(@"Userdata");
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Directory.GetCurrentDirectory() + @"\Userdata";
            sfd.FileName = "dat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = sfd.OpenFile();
                BinaryFormatter bF = new BinaryFormatter();
                bF.Serialize(fileStream, stateHistory);
                fileStream.Close();
            }

        }
        void load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + @"\Userdata";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = ofd.OpenFile();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Dataset loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                if (DialogResult.Yes == MessageBox.Show("選んだステージに移動してもいいですか?", "かくにん", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                {
                    stateHistory = loadedData;
                }


                //this.=stateHistory.cpsquarelength;
                back = stateHistory.cpback;
                fore = stateHistory.cpfore;
                this.pictureBox1.BackgroundImage = back;
                this.pictureBox1.Image = fore;
                controlobj = stateHistory.cpcontrolobj;
                CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
                ListObjectBoard = stateHistory.cp_ListObjectBoard;
            }

        }

        void load2() //quickload
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + @"\Userdata";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = ofd.OpenFile();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Dataset loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                
                    stateHistory = loadedData;
                


                //this.=stateHistory.cpsquarelength;
                back = stateHistory.cpback;
                fore = stateHistory.cpfore;
                this.pictureBox1.BackgroundImage = back;
                this.pictureBox1.Image = fore;
                controlobj = stateHistory.cpcontrolobj;
                CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
                ListObjectBoard = stateHistory.cp_ListObjectBoard;
            }

        }



        private void セーブToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void ロードToolStripMenuItem_Click(object sender, EventArgs e)
        {
            load();
        }


#endregion

		#region //loadDataset
		private void loadDataset(string path, byte[] resource)
		{


			using (Stream writeStream = new FileStream(path, FileMode.Create))
			{
				BinaryWriter bw = new BinaryWriter(writeStream);
				bw.Write(resource);
			}

			Dataset loadedData;
			using (Stream fileStream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
			}

			if (DialogResult.Yes == MessageBox.Show("選んだステージに移動してもいいですか?", "かくにん", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
			{
				stateHistory = loadedData;
			}

			//this.=stateHistory.cpsquarelength;
			back = stateHistory.cpback;
			fore = stateHistory.cpfore;
			this.pictureBox1.BackgroundImage = back;
			this.pictureBox1.Image = fore;
			controlobj = stateHistory.cpcontrolobj;
			CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
			ListObjectBoard = stateHistory.cp_ListObjectBoard;
		}

		private void demoDataset(string path, byte[] resource)
		{


			using (Stream writeStream = new FileStream(path, FileMode.Create))
			{
				BinaryWriter bw = new BinaryWriter(writeStream);
				bw.Write(resource);
			}

			Dataset loadedData;
			using (Stream fileStream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
			}
			stateHistory = loadedData;
			//this.=stateHistory.cpsquarelength;
			back = stateHistory.cpback;
			fore = stateHistory.cpfore;
			this.pictureBox1.BackgroundImage = back;
			this.pictureBox1.Image = fore;
			controlobj = stateHistory.cpcontrolobj;
			CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
			ListObjectBoard = stateHistory.cp_ListObjectBoard;
		}

		private void loadDataset2(string path, byte[] resource)  //quickload
		{


			using (Stream writeStream = new FileStream(path, FileMode.Create))
			{
				BinaryWriter bw = new BinaryWriter(writeStream);
				bw.Write(resource);
			}

			Dataset loadedData;
			using (Stream fileStream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
			}


			stateHistory = loadedData;


			//this.=stateHistory.cpsquarelength;
            controlobj = stateHistory.cpcontrolobj;
            CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
            ListObjectBoard = stateHistory.cp_ListObjectBoard;
			back = stateHistory.cpback;
			fore = stateHistory.cpfore;

			this.pictureBox1.BackgroundImage = back;
			this.pictureBox1.Image = fore;
            //controlobj = stateHistory.cpcontrolobj;
            //CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
            //ListObjectBoard = stateHistory.cp_ListObjectBoard;
		}
		#endregion


		#region //ステージ選択

		/// <summary>
        /// ステージ1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void toolStripMenuItem6_Click(object sender, EventArgs e)
		{
            if(locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage1";
			var resource = Properties.Resources.stage1;
			try
			{
				loadDataset(path, resource);
				stagecount = 1;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
		}

		/// <summary>
		/// ステージ2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage2";
			var resource = Properties.Resources.stage2;
			try
			{
				loadDataset(path, resource);
				stagecount = 2;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ3
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage3";
			var resource = Properties.Resources.stage3;
			try
			{
				loadDataset(path, resource);
				stagecount = 3;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ4
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem9_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage4";
			var resource = Properties.Resources.stage4;
			try
			{
				loadDataset(path, resource);
				stagecount = 4;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ5
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem10_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage5";
			var resource = Properties.Resources.stage5;
			try
			{
				loadDataset(path, resource);
				stagecount = 5;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ6
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem11_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage6";
			var resource = Properties.Resources.stage6;
			try
			{
				loadDataset(path, resource);
				stagecount = 6;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ7
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem12_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage7";
			var resource = Properties.Resources.stage7;
			try
			{
				loadDataset(path, resource);
				stagecount = 7;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ8
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem13_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage8";
			var resource = Properties.Resources.stage8;
			try
			{
				loadDataset(path, resource);
				stagecount = 8;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ9
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem14_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage9";
			var resource = Properties.Resources.stage9;
			try
			{
				loadDataset(path, resource);
				stagecount = 9;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ10
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem15_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage10";
			var resource = Properties.Resources.stage10;
			try
			{
				loadDataset(path, resource);
				stagecount = 10;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ11
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem16_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage11";
			var resource = Properties.Resources.stage11;
			try
			{
				loadDataset(path, resource);
				stagecount = 11;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ12
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem17_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage12";
			var resource = Properties.Resources.stage12;
			try
			{
				loadDataset(path, resource);
				stagecount = 12;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}


		/// <summary>
		/// ステージ13
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem18_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage13";
			var resource = Properties.Resources.stage13;
			try
			{
				loadDataset(path, resource);
				stagecount = 13;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ14
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem19_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage14";
			var resource = Properties.Resources.stage14;
			try
			{
				loadDataset(path, resource);
				stagecount = 14;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ15
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem20_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage15";
			var resource = Properties.Resources.stage15;
			try
			{
				loadDataset(path, resource);
				stagecount = 15;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ16
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem21_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage16";
			var resource = Properties.Resources.stage16;
			try
			{
				loadDataset(path, resource);
				stagecount = 16;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ17
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem22_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage17";
			var resource = Properties.Resources.stage17;
			try
			{
				loadDataset(path, resource);
				stagecount = 17;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}
		
		/// <summary>
		/// ステージ18
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage18";
			var resource = Properties.Resources.stage18;
			try
			{
				loadDataset(path, resource);
				stagecount = 18;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}


		/// <summary>
		/// ステージ19
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem24_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage19";
			var resource = Properties.Resources.stage19;
			try
			{
				loadDataset(path, resource);
				stagecount = 19;
				stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}

		/// <summary>
		/// ステージ20
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuItem25_Click(object sender, EventArgs e)
		{
            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            if (locked == false)
            {
                locked = true;
            }
			string path = "Userdata/stage20";
			var resource = Properties.Resources.stage20;
			try
			{
				loadDataset(path, resource);
				stagecount = 20;
				stage.Text = "現在のステージ:" + stagecount;
                if(button2.Visible==true)
                {
                    button2.Visible = false;
                }
			}
			catch (Exception exc)
			{

			}
		}
        #endregion // //

		#region //チュートリアルモード
		private void 開始ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tutorialcount == 0)
			{
                pictureBox3.Size = new Size(211, 192);
                pictureBox3.Location = new Point(468, 406);
				(tabControl1.TabPages[1] as Control).Enabled = true;
				label2.Visible = false;
				tabControl1.SelectedIndex = 1;
				comboBox1.SelectedIndex = 0;
				開始ToolStripMenuItem.Text = "終了";
				ステージ選択ToolStripMenuItem1.Enabled = false;
				ステージ編集モードToolStripMenuItem.Enabled = false;
                チュートリアルモードToolStripMenuItem.Enabled = true;
				richTextBox1.Visible = true;
				richTextBox1.Text = "はじめてのプログラミングへようこそ！";
				tutorialcount++;
                button2.Visible = false;
				button1.Visible = true;
                button3.Visible = true;
                button4.Visible = false;
                pictureBox3.Visible = false;
			}
			else
			{
				開始ToolStripMenuItem.Text = "開始";
				ステージ選択ToolStripMenuItem1.Enabled = true;
				ステージ編集モードToolStripMenuItem.Enabled = true;
                チュートリアルモードToolStripMenuItem.Enabled = true;
				label2.Visible = true;
				(tabControl1.TabPages[1] as Control).Enabled = false;
				tutorialcount = 0;
				richTextBox1.Visible = false;
				tutorial = 0;
				button1.Visible = false;
                if(pictureBox2.Visible==true)
                {
                    pictureBox2.Visible = false;
                }
                if(pictureBox3.Visible==true)
                {
                    pictureBox3.Visible = false;
                }
                button3.Visible = false;
                button4.Visible = true;
			}
		}

		#endregion

		#region //menu controls
		private void ステージ選択ToolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 0;
		}

		private void Object_Control_Menu_Opening(object sender, CancelEventArgs e)
		{
			if (!contextFlag) e.Cancel = true;
		}

		private void ステージ編集モードToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (contextFlag)
			{
				contextFlag = false;
				ステージ選択ToolStripMenuItem1.Enabled = true;
				チュートリアルモードToolStripMenuItem.Enabled = true;
				ステージ編集モードToolStripMenuItem.Text = "ステージ編集モード";
			}
			else 
			{
				contextFlag = true;
				ステージ選択ToolStripMenuItem1.Enabled = false;
				チュートリアルモードToolStripMenuItem.Enabled = false;
				ステージ編集モードToolStripMenuItem.Text = "編集モード終了";
			}
		}

		#endregion

        #region チュートリアル
        private void button1_Click(object sender, EventArgs e)
        {
            if(pictureBox3.Visible == true)
            {
                pictureBox3.Visible = false;
            }
            if(tutorial == 0)
            {
                richTextBox1.Text = "このゲームは、迷路をクリアするプログラムをみなさんに体験していただきます！\nそれでは実際に迷路を体験していただきましょう！";
                if(locked == true)
                {
                    locked = false;
                }
                tutorial++;
            }
            else if(tutorial == 1)
            {
                richTextBox1.Text = "パート1 前への進み方";
                if (locked == true)
                {
                    locked = false;
                }
                tutorial++;
            }
            else if(tutorial == 2)
            {
				comboBox1.SelectedIndex = 1;
				string path = "Userdata/tutorial1";
                var resource = Properties.Resources.tutorial1;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル1";

                }
                catch (Exception exc)
                {

                }
                richTextBox1.Text = "前へ進むには 前へ進む のブロックをドラッグし、すべて実行 をクリックします。\nそれでは実際にキャラクターを前へ動かしてみましょう！\nブロックを置くにはプログラミングを上にあるプログラミングをクリックしてね！\nゴールについたら次へを押してね！";
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }

            }
            else if (tutorial == 3)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "このように一度のすべて実行でゴールに到着するのが目標になります。";
                        tutorial++;
                        break;
                    }
                    else
                    {
                        
                    }
                }
                if(tutorial!=4)
                {
                    richTextBox1.Text = "前へ進めてないよ！！\n前へ進むには 前へ進む のブロックをドラッグし、すべて実行 をクリックします。";
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 4)
            {
                richTextBox1.Text = "パート2 向きを変える\nこの主人公はまっすぐにしか進めません…\nそれでは今のようにプレイヤーの右側にゴールがある場合はどうしましょうか？";
                
                string path = "Userdata/tutorial2";
                var resource = Properties.Resources.tutorial2;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル2";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 5)
            {
				comboBox1.SelectedIndex = 2;
				richTextBox1.Text = "そんな時には、右を向く または 左を向く のどちらかのブロックをドラッグしましょう！\n主人公から見てゴールの扉は左側にあるので、左を向く をドラッグし、すべて実行をクリックしてください。\n前へ進むのブロックが残っていたら、すべて削除もしくは前へ進むのブロックを右クリックすることで削除できます！\nクリックしたら次へを押してね！";
                tutorial++;
                pictureBox2.Visible = true;
                stage.Text = "現在のステージ:チュートリアル2";
                stagecount = 30;
                pictureBox2.Image = Properties.Resources.direction;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 6)
            {
                if(ListObjectBoard[0].objectDirection==1)
                {
                    richTextBox1.Text = "それではゴールの方向を向くことができました。次に前へ進むのブロックをドラッグして、ゴールしましょう！";
                    tutorial++;
                    pictureBox2.Visible = false;
                }
                else
                {
                    richTextBox1.Text = "ゴールの方向を向けてないよ！！\n左を向く をドラッグし、すべて実行をクリックしてね！クリックしたら次へを押してね";
                    string path = "Userdata/tutorial2";
                    var resource = Properties.Resources.tutorial2;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2";
                    }
                    catch (Exception exc)
                    {

                    }
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 7)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "ゴールできました!\nそれでは一回の実行でゴールを目指してみましょう。";
                        tutorial++;
                        break;
                    }
                    else
                    {
                        
                    }
                }
                if(tutorial!=8)
                {
                    richTextBox1.Text = "ゴールできてないよ！！\nもう一度やってみよう！";
                    string path = "Userdata/tutorial21";
                    var resource = Properties.Resources.tutorial21;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2-1";
                    }
                    catch (Exception exc)
                    {

                    }
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 8)
            {
                richTextBox1.Text = "ブロックの数は何個でも配置することができます！\n今度は、\n左を向く \n前へ進む \nの順にブロックをドラッグし、すべて実行をクリックしてみましょう！\nゴールできたら次へを押してね！";
                string path = "Userdata/tutorial2";
                var resource = Properties.Resources.tutorial2;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル2";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 9)
            {
                foreach(var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "一度の実行でクリアすることができました！\nこのように一度の実行でクリアすることが目標です。\nステージは後ろに行けばいくほど難しくなるので、頑張って挑戦してみましょう！";
                        tutorial++;
                        break;
                    }
                    else
                    {
                        
                    }
                }
                if(tutorial!=10)
                {
                    richTextBox1.Text = "失敗してしまいました…\nもう一度挑戦してみよう！";
                    string path = "Userdata/tutorial2";
                    var resource = Properties.Resources.tutorial2;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2";
                    }
                    catch (Exception exc)
                    {

                    }
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 10)
            {
                richTextBox1.Text = "パート3 繰り返し";
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 11)
            {
				comboBox1.SelectedIndex = 3;
				richTextBox1.Text = "ゴールが主人公の5マス先にあります。\nつまり 前へ進む ブロックを5つドラッグすればクリアできますね！\nしかし5つも配置するのは大変です(´・ω・`)\n何か良い方法はないのでしょうか？？";
                tutorial++;
                string path = "Userdata/tutorial3";
                var resource = Properties.Resources.tutorial3;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル3";
                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 12)
            {
                richTextBox1.Text = "そこで、繰り返し ブロックをドラッグします！\nまず 繰り返し ブロックをドラッグしましょう。\nそして 前へ進む を繰り返しブロックの間にドラッグします。\n準備ができたら、すべて実行をクリックしましょう！";
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
                pictureBox2.Visible = true;
                pictureBox2.Image = Properties.Resources.tu1;
            }
            else if (tutorial == 13)
            {
                
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "前へ進むブロックが1つだけでクリアすることができました！\nこのように繰り返しブロックは繰り返しの間にあるブロックをゴールに辿りつくまで何回も行います。";
                        tutorial++;
                        pictureBox2.Visible = false;
                    }

                }
                if(tutorial!=14)
                {
                    richTextBox1.Text = "失敗してしまいました…\nもう一度挑戦してみよう！";
                    string path = "Userdata/tutorial3";
                    var resource = Properties.Resources.tutorial3;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル3";
                    }
                    catch (Exception exc)
                    {

                    }
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 14)
            {
                richTextBox1.Text = "繰り返しブロックの復習です(｀・ω・´)\nこのステージをブロック5つでクリアしてみましょう！\nヒント 2回前へ進み、1回左を向くを繰り返しましょう！";
                string path = "Userdata/tutorial4";
                var resource = Properties.Resources.tutorial4;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル4";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial==15)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "繰り返しブロックの使い方は以上です！\nチュートリアルは次で最後になります('ω')";
                        tutorial++;
                    }

                }
                if (tutorial != 16)
                {
                    richTextBox1.Text = "繰り返しブロックの復習です(｀・ω・´)\nこのステージをブロック5つでクリアしてみましょう！\nヒント 2回前へ進み、1回左を向くを繰り返しましょう！";
                    string path = "Userdata/tutorial4";
                    var resource = Properties.Resources.tutorial4;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル4";
                    }
                    catch (Exception exc)
                    {

                    }
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 16)
            {
                richTextBox1.Text = "パート4 条件文";
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 17)
            {
				comboBox1.SelectedIndex = 4;
				richTextBox1.Text = "最後は条件文です。\nこのステージをクリアするには条件を組み合わせることが重要になります。\n次のようにブロックを配置してみましょう！\n繰り返し　の中に\n前へ進む\nもし　左が壁でないなら　を入れて\n左を向く　をもし　左が壁でないならの中に入れます。\nそれではすべて実行をクリックしてみましょう！";
                string path = "Userdata/tutorial5";
                var resource = Properties.Resources.tutorial5;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル5";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                button1.Text = "次へ";
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 18)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "ゴールできました！\nもしブロックは、その条件を満たしていた時にその中にあるブロックの行動をします。\n今どの行動をしているかは矢印マークで確認できます。";
                        tutorial++;
                        button1.Text = "次へ";
                    }

                }
                if (tutorial != 19)
                {
                    richTextBox1.Text = "次のようにブロックを配置してみましょう！\n次のようにブロックを配置してみましょう！\n繰り返し　の中に\n前へ進む\nもし　左が壁でないなら　を入れて\n左を向く　をもし　左が壁でないならの中に入れます。それではすべて実行をクリックしてみましょう！\nブロックを間違って配置してしまったときは、全削除ですべてのブロックを消せます！\nまた一つだけ消したいときは、消したいブロックの上で右クリックをして削除を選ぶと消すことができます！";
                    string path = "Userdata/tutorial5";
                    var resource = Properties.Resources.tutorial5;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル5";
                    }
                    catch (Exception exc)
                    {

                    }
                    pictureBox2.Visible = true;
                    pictureBox2.Image = Properties.Resources.tu2;
                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if(tutorial == 19)
            {
                button1.Text = "次へ";
                richTextBox1.Text = "最後にこのステージをクリアしてみましょう！";
                tutorial++;
                string path = "Userdata/tutorial6";
                var resource = Properties.Resources.tutorial6;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル6";
                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }
                pictureBox2.Image = Properties.Resources.direction;
                pictureBox2.Visible = false;
            }
            else if(tutorial==20)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "ゴールできました！\n以上でチュートリアルはおしまいです。\n全ステージクリアを目指してがんばろう！";
                        button1.Text = "次へ";
                    }

                }
                if (locked == true)
                {
                    locked = false;
                }
                tutorial++;
            }
            else if (tutorial == 21)
            {
                開始ToolStripMenuItem.Text = "開始";
                ステージ選択ToolStripMenuItem1.Enabled = true;
                ステージ編集モードToolStripMenuItem.Enabled = true;
                label2.Visible = true;
                (tabControl1.TabPages[1] as Control).Enabled = false;
                tutorialcount = 0;
                richTextBox1.Visible = false;
                tutorial = 0;
                button1.Visible = false;
                if (pictureBox2.Visible == true)
                {
                    pictureBox2.Visible = false;
                }
                if (pictureBox3.Visible == true)
                {
                    pictureBox3.Visible = false;
                }
                button3.Visible = false;
                tabControl1.SelectedIndex = 0;
                button4.Visible = true;
            }
        }

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0) 
			{
				richTextBox1.Text = "はじめてのプログラミングへようこそ！";
				tutorial = 0;
			}
			else if (comboBox1.SelectedIndex == 1)
			{
				richTextBox1.Text = "パート1 前への進み方";
				tutorial = 2;
			}
			else if (comboBox1.SelectedIndex == 2)
			{
				richTextBox1.Text = "パート2 向きを変える\nこの主人公はまっすぐにしか進めません…\nそれでは今のようにプレイヤーの右側にゴールがある場合はどうしましょう？";
				string path = "Userdata/tutorial2";
				var resource = Properties.Resources.tutorial2;
				try
				{
					demoDataset(path, resource);
					stagecount = 1;
					stage.Text = "現在のステージ:" + stagecount;
				}
				catch (Exception exc)
				{

				}
				tutorial = 5;
			}
			else if (comboBox1.SelectedIndex == 3)
			{
				richTextBox1.Text = "パート3 繰り返し";
				tutorial = 11;
			}
			else if(comboBox1.SelectedIndex == 4)
			{
				richTextBox1.Text = "パート4 条件文";
				tutorial = 17;
			}
		}
        




        System.Threading.Timer dragevent;

        delegate void SetDelegate();
        public void draevent(){
            int x = -1;
            int y = -1;
            int objectselectnum = 0;



            Point sp = System.Windows.Forms.Cursor.Position;
            System.Drawing.Point cp = pictureBox1.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);
           
            if (x > -1)
            {
                switch (setswitch) { 
                    case (int)set.Wall :    ObjectSet(x, y,(int) BoardObject.objectselect.Wall);
                        break;
                    case (int)set.Enemy:  ObjectSet(x, y, (int)BoardObject.objectselect.Enemy);
                        break;
                    case (int)set.Item: ObjectSet(x, y, (int)BoardObject.objectselect.Item);
                        break;
                    case (int)set.Goal: ObjectSet(x, y, (int)BoardObject.objectselect.Goal);
                        break;
                    case (int)set.Del: deleteobj(x,y);
                        break;
                    case (int)set.None:
                        break;
            }
            }

        

        }
        void called(Object obj)
        {
            Invoke(new SetDelegate(draevent));

        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
            {
                return;
            }
            //dragevent = new System.Threading.Timer(new System.Threading.TimerCallback(called),null,System.Threading.Timeout.Infinite,50);
            dragevent.Change(100, 100);

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
            {
                return;
            }
            dragevent.Change(System.Threading.Timeout.Infinite,100);
        }

        private void EnemysettoolStripMenuItem4_Click(object sender, EventArgs e)
        {
           setswitch=(int) set.Enemy;
           settingobj.Text = "配置:敵";
        }

        private void 壁を置くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setswitch = (int)set.Wall;
            settingobj.Text = "配置:壁";
        }

        private void アイテムを置くToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setswitch = (int)set.Item;
            settingobj.Text = "配置:アイテム";
        }

        private void ゴールを作るToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setswitch = (int)set.Goal;
            settingobj.Text = "配置:ゴール";
        }

        private void 削除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setswitch = (int)set.Del;
            settingobj.Text = "削除";
        }

        private void 設定なしToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setswitch = (int)set.None;
            settingobj.Text = "";
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            constructer();
        }
		#endregion

        #region 不要
		public static void writer(int stagecount)
		{


			int[] cl = new int[20];

			//using (StreamReader sr = new StreamReader("Userdata/clear.csv"))
			//{
			//	for (int i = 0; i < 20; i++)
			//	{
			//		cl[i] = int.Parse(sr.ReadLine());
			//	}
			//}
			//using (StreamWriter sw = new StreamWriter("Userdata/clear.csv"))
			//{
			//	for (int i = 0; i < 20; i++)
			//	{
			//		sw.WriteLine(cl[i]);
			//	}
			//}
			if (cl[stagecount] != 1)
			{
				cl[stagecount] = 1;
			}

		}

		public static int reader(int stagecount)
		{
			int[] cl = new int[stagecount];
			int count = 0;
			using (StreamReader sr = new StreamReader("Userdata/clear.csv"))
			{
				for (int i = 0; i < stagecount; i++)
				{
					cl[i] = int.Parse(sr.ReadLine());
				}
			}
			foreach (var n in cl)
			{
				if (n == 0)
				{
					count++;
				}
			}
			return count;

		}
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {

            pictureBox3.Size = new Size(211, 192);
            pictureBox3.Location = new Point(468, 406);
            pictureBox3.Visible = false;
            tabControl1.SelectedIndex = 0;
            stagecount++;
            if (this.stage.InvokeRequired)
            {
                stagepresent stapre = new stagepresent(() => stage.Text = "現在のステージ:" + stagecount);
                this.Invoke(stapre);
            }
            else
            {
                stage.Text = "現在のステージ:" + stagecount;
            }
            string pathnext = "stage" + stagecount.ToString();
            byte[] da = (byte[])Properties.Resources.ResourceManager.GetObject(pathnext);
            try
            {
                locked = false;

                loadDataset2(pathnext, da);

            }
            catch (Exception exc)
            {

            }
            button2.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Visible == true)
            {
                pictureBox3.Visible = false;
            }
            
            if (tutorial == 3)
            {
                richTextBox1.Text = "前へ進むには 前へ進む のブロックをドラッグし、すべて実行 をクリックします。\nそれでは実際にキャラクターを前へ動かしてみましょう！\nブロックを置くにはプログラミングを上にあるプログラミングをクリックしてね！\nゴールについたら次へを押してね！";
                string path = "Userdata/tutorial1";
                var resource = Properties.Resources.tutorial1;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル1";

                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }

            }
            else if (tutorial == 6)
            {
                
                string path = "Userdata/tutorial2";
                var resource = Properties.Resources.tutorial2;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル2";
                }
                catch (Exception exc)
                {

                }
                pictureBox2.Visible = true;
                pictureBox2.Image = Properties.Resources.direction;
                if (locked == true)
                {
                    locked = false;
                }
                richTextBox1.Text = "そんな時には、右を向く または 左を向く のどちらかのブロックをドラッグしましょう！\n主人公から見てゴールの扉は左側にあるので、左を向く をドラッグし、すべて実行をクリックしてください。\n前へ進むのブロックが残っていたら、すべて削除もしくは前へ進むのブロックを右クリックすることで削除できます！\nクリックしたら次へを押してね！";
                
            }
            else if (tutorial == 7)
            {
                
                    string path = "Userdata/tutorial21";
                    var resource = Properties.Resources.tutorial21;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2-1";
                    }
                    catch (Exception exc)
                    {

                    }
                
                if (locked == true)
                {
                    locked = false;
                }
                richTextBox1.Text = "それではゴールの方向を向くことができました。次に前へ進むのブロックをドラッグして、ゴールしましょう！";
                    
            }
            else if (tutorial == 9)
            {
                string path = "Userdata/tutorial2";
                var resource = Properties.Resources.tutorial2;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル2";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 10)
            {
                    string path = "Userdata/tutorial2";
                    var resource = Properties.Resources.tutorial2;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2";
                    }
                    catch (Exception exc)
                    {

                    }
                
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 12)
            {
                string path = "Userdata/tutorial3";
                var resource = Properties.Resources.tutorial3;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル3";
                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 14)
            {

                string path = "Userdata/tutorial3";
                    var resource = Properties.Resources.tutorial3;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル3";
                    }
                    catch (Exception exc)
                    {

                    }
                
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 15)
            {
                string path = "Userdata/tutorial4";
                var resource = Properties.Resources.tutorial4;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル4";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 16)
            {
                 string path = "Userdata/tutorial4";
                    var resource = Properties.Resources.tutorial4;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル4";
                    }
                    catch (Exception exc)
                    {

                    }
                
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 18)
            {
                string path = "Userdata/tutorial5";
                var resource = Properties.Resources.tutorial5;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル5";
                }
                catch (Exception exc)
                {

                }
                tutorial++;
                button1.Text = "やり直し";
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 19)
            {
                 string path = "Userdata/tutorial5";
                    var resource = Properties.Resources.tutorial5;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル5";
                    }
                    catch (Exception exc)
                    {

                    }
                
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 20)
            {
                string path = "Userdata/tutorial6";
                var resource = Properties.Resources.tutorial6;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル6";
                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }
            }
            else if (tutorial == 21)
            {
                string path = "Userdata/tutorial6";
                var resource = Properties.Resources.tutorial6;
                try
                {
                    demoDataset(path, resource);
                    stagecount = 30;
                    stage.Text = "現在のステージ:チュートリアル6";
                }
                catch (Exception exc)
                {

                }
                if (locked == true)
                {
                    locked = false;
                }
                tutorial++;
            }
            
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==1)
            {
                tabControl1.SelectedIndex = 0;
            }

            locked = true;
            string path = "stage" + stagecount.ToString();
            byte[] resource = (byte[])Properties.Resources.ResourceManager.GetObject(path);
            try
            {
                using (Stream writeStream = new FileStream(path, FileMode.Create))
                {
                    BinaryWriter bw = new BinaryWriter(writeStream);
                    bw.Write(resource);
                }

                Dataset loadedData;
                using (Stream fileStream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    loadedData = (Dataset)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                }

                    stateHistory = loadedData;
                //this.=stateHistory.cpsquarelength;
                back = stateHistory.cpback;
                fore = stateHistory.cpfore;
                this.pictureBox1.BackgroundImage = back;
                this.pictureBox1.Image = fore;
                controlobj = stateHistory.cpcontrolobj;
                CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
                ListObjectBoard = stateHistory.cp_ListObjectBoard;
                stage.Text = "現在のステージ:" + stagecount;
                if (button2.Visible == true)
                {
                    button2.Visible = false;
                }
            }
            catch (Exception exc)
            {

            }
        }

        private void ステージ選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

		public class PanelEventArgs : EventArgs
		{
			public bool showFlag;
		}

		public delegate void PanelEventHandler(object sender, PanelEventArgs e);
		public event PanelEventHandler PanelEvent;

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			PanelEventArgs ee = new PanelEventArgs();
			if (tabControl1.SelectedIndex == 0) ee.showFlag = true;
			else ee.showFlag = false;
			this.PanelEvent(this, ee);
		}


		

		




		

    }

        

    interface BoardPosition //ボードの仕様はBoardDataクラス以外で変更不可，外部からのアクセスを制限
    {
        int BoardPositionXmax { get; }
        int BoardPositionYmax { get; }
        bool[,] BoardObjectCanMove { get; set; }
        List<BoardObject> ListObjectBoard { get; set; }
    }

}
