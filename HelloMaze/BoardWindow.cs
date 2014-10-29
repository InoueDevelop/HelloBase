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

namespace HelloMaze
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

        #endregion

        public BoardData() //コンストラクタ
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
                if (reader(10) == 0 && tutorialcount == 0)
                {
                    label16.Visible = true;
                    label17.Visible = true;
                    label18.Visible = true;
                    label19.Visible = true;
                    label20.Visible = true;
                    label21.Visible = true;
                    label22.Visible = true;
                    label23.Visible = true;
                    label24.Visible = true;
                    label25.Visible = true;
                }
                else
                {
                    label16.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    label20.Visible = false;
                    label21.Visible = false;
                    label22.Visible = false;
                    label23.Visible = false;
                    label24.Visible = false;
                    label25.Visible = false;
                }
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
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
                label19.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                label25.Visible = false;
            }
        }


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
                        locked = true;
                        Task goalevent = new Task(() => { Goalevent(); });
                        goalevent.Start();
                        //Goalevent();
                    }
                    else
                    {
                        RefreshPictureBox1 refreshPic1 = new RefreshPictureBox1(() =>
                        {
                            pictureBox3.Visible = true;
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

               writer(stagecount-1);

                   stagecount++;

                   locked = true;


                   ClearForm clearwindow = new ClearForm(stagecount-1);


                   if (stagecount == 21)
                   {
                       clearwindow.Loadtext = "全クリア！！";
                       stagecount = 1;
                   }
                   clearwindow.ShowDialog();

                   if (clearwindow.newgamestart == true)
                   {
                       if (this.pictureBox1.InvokeRequired)
                       {
                           RefreshPictureBox1 constr = new RefreshPictureBox1(() => constructer());
                           this.Invoke(constr);
                       }
                       else {constructer(); }
                   }
                   else if (clearwindow.Loaddatastart == true)
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

                       if (!(reader(10) > 0) && tutorialcount == 0)
                       {
                           RefreshPictureBox1 co = new RefreshPictureBox1(() =>
                           {
                               label16.Visible = true;
                               label17.Visible = true;
                               label18.Visible = true;
                               label19.Visible = true;
                               label20.Visible = true;
                               label21.Visible = true;
                               label22.Visible = true;
                               label23.Visible = true;
                               label24.Visible = true;
                               label25.Visible = true;
                           });
                           this.Invoke(co);
                       }



                   }
                   else { locked = false; }
                   clearwindow.Dispose();
               
           } //ゴールに重なった時のイベント


        public void directionchange ()
        {
            bmppaint.playerdirectionchange(controlobj.objectDirection,fore,controlobj.objectPositionX,controlobj.objectPositionY);
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
                if (DialogResult.Yes == MessageBox.Show("読み込んだファイルで現在のプレイに上書きしてもよいですか?", "上書きの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
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

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
#endregion


        #region //ステージ選択

        /// <summary>
        /// labelset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage1";
            var resource = Properties.Resources.stage1;
            try
            {
                loadDataset(path,resource);
                stagecount = 1;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void loadDataset(string path,byte[] resource)
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
           
            if (DialogResult.Yes == MessageBox.Show("読み込んだファイルで現在のプレイに上書きしてもよいですか?", "上書きの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
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
            back = stateHistory.cpback;
            fore = stateHistory.cpfore;
            this.pictureBox1.BackgroundImage = back;
            this.pictureBox1.Image = fore;
            controlobj = stateHistory.cpcontrolobj;
            CanPutObjectOnBoard = stateHistory.cpCanPutObjectOnBoard;
            ListObjectBoard = stateHistory.cp_ListObjectBoard;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage2";
            var resource = Properties.Resources.stage2;
            try
            {
                loadDataset(path, resource);
                stagecount = 2;
                stage.Text = "現在のステージ:"+stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage3";
            var resource = Properties.Resources.stage3;
            try
            {
                loadDataset(path, resource);
                stagecount = 3;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage4";
            var resource = Properties.Resources.stage4;
            try
            {
                loadDataset(path, resource);
                stagecount = 4;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage5";
            var resource = Properties.Resources.stage5;
            try
            {
                loadDataset(path, resource);
                stagecount = 5;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage6";
            var resource = Properties.Resources.stage6;
            try
            {
                loadDataset(path, resource);
                stagecount = 6;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage7";
            var resource = Properties.Resources.stage7;
            try
            {
                loadDataset(path, resource);
                stagecount = 7;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage8";
            var resource = Properties.Resources.stage8;
            try
            {
                loadDataset(path, resource);
                stagecount = 8;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage9";
            var resource = Properties.Resources.stage9;
            try
            {
                loadDataset(path, resource);
                stagecount = 9;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage10";
            var resource = Properties.Resources.stage10;
            try
            {
                loadDataset(path, resource);
                stagecount = 10;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }
        #endregion // //

        private void label11_Click(object sender, EventArgs e)
        {
            if(tutorialcount==0)
            {

                label11.Text = "チュートリアル終了";
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
                label19.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                label25.Visible = false;
                richTextBox1.Visible = true;
                richTextBox1.Text = "はじめてのプログラミングへようこそ！";
                tutorialcount++;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                button1.Visible = true;
            }
            else
            {
                label11.Text = "チュートリアル開始";
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                if(reader(10)==0)
                {
                    label16.Visible = true;
                    label17.Visible = true;
                    label18.Visible = true;
                    label19.Visible = true;
                    label20.Visible = true;
                    label21.Visible = true;
                    label22.Visible = true;
                    label23.Visible = true;
                    label24.Visible = true;
                    label25.Visible = true;
                    tutorialcount = 0;
                }

                
                richTextBox1.Visible = false;

                tutorial = 0;
                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                button1.Visible = false;

            }
            
        }

        public static void writer(int stagecount)
        {
            int[] cl = new int[20]; 
            using (StreamReader sr = new StreamReader("Userdata/clear.csv"))
            {
                for (int i = 0; i < 20; i++)
                {
                    cl[i] = int.Parse(sr.ReadLine());
                }
            }
            if(cl[stagecount]!=1)
            {
                cl[stagecount] = 1;
            }
            using(StreamWriter sw = new StreamWriter("Userdata/clear.csv"))
            {
                for(int i=0;i<20;i++)
                {
                    sw.WriteLine(cl[i]);
                }
            }
        }

        public static int reader(int stagecount)
        {
            int[] cl = new int[stagecount];
            int count=0;
            using (StreamReader sr = new StreamReader("Userdata/clear.csv"))
            {
                for (int i = 0; i < stagecount; i++)
                {
                    cl[i] = int.Parse(sr.ReadLine());
                }
            }
            foreach(var n in cl)
            {
                if(n==0)
                {
                    count++;
                }
            }
            return count;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

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
                tutorial++;
            }
            else if(tutorial == 1)
            {
                richTextBox1.Text = "パート1 前への進み方";
                tutorial++;
            }
            else if(tutorial == 2)
            {
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
                richTextBox1.Text = "前へ進むには命令セットから前へ進むを選び、配置をクリックしてください。\nその後、連続実行をクリックします。\nそれでは実際にキャラクターを前へ動かしてみましょう！\nゴールについたら次へを押してね！";
                tutorial++;

            }
            else if (tutorial == 3)
            {
                if (ListObjectBoard[0].objectPositionX == ListObjectBoard[ListObjectBoard.Count()-2].objectPositionX && ListObjectBoard[0].objectPositionY == ListObjectBoard[ListObjectBoard.Count()-2].objectPositionY)
                {
                    richTextBox1.Text = "このように一度の連続実行でゴールに到着するのが目標になります。";
                    tutorial++;
                }
                else
                {
                    richTextBox1.Text = "前へ進めてないよ！！\n前へ進むには命令セットから前へ進むを選び、配置をクリックしてください。\nその後、連続実行をクリックします。";
                }
            }
            else if(tutorial == 4)
            {
                richTextBox1.Text = "パート2 向きを変える\nこの主人公はまっすぐにしか進めません…\nそれでは今のようにプレイヤーの右側にゴールがある場合はどうしましょう？";
                
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
            }
            else if(tutorial == 5)
            {
                richTextBox1.Text = "そんな時には、右を向く または 左を向く のどちらかのブロックを配置してあげましょう！\n主人公から見てゴールの扉は左側にあるので、左を向く を配置して連続実行をクリックしてください。\n前へ進むのブロックが残っていたら、すべて削除もしくは前へ進むのブロックを右クリックすることで削除できます！\nクリックしたら次へを押してね！";
                tutorial++;
                pictureBox2.Visible = true;
                pictureBox2.Image = Properties.Resources.direction;
            }
            else if (tutorial == 6)
            {
                if(ListObjectBoard[0].objectDirection==1)
                {
                    richTextBox1.Text = "それではゴールの方向を向くことができました。次に前へ進むのブロックを配置して、ゴールしましょう！";
                    tutorial++;
                    pictureBox2.Visible = false;
                }
                else
                {
                    richTextBox1.Text = "ゴールの方向を向けてないよ！！\nもう一度やってみよう！";
                    string path = "Userdata/tutorial2";
                    var resource = Properties.Resources.tutorial2;
                    try
                    {
                        demoDataset(path, resource);
                        stagecount = 30;
                        stage.Text = "現在のステージ:チュートリアル2" + stagecount;
                    }
                    catch (Exception exc)
                    {

                    }
                }
            }
            else if (tutorial == 7)
            {
                if (ListObjectBoard[0].objectPositionX == 6 && ListObjectBoard[0].objectPositionY==5)
                {
                    richTextBox1.Text = "ゴールできました!\nそれでは一回の実行でゴールを目指してみましょう。";
                    tutorial++;
                }
                else
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
            }
            else if(tutorial == 8)
            {
                richTextBox1.Text = "ブロックの数は何個でも配置することができます！\n今度は、\n左を向く \n前へ進む \nの順にブロックを配置し、連続実行をクリックしてみましょう！\nゴールできたら次へを押してね！";
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
            }
            else if (tutorial == 9)
            {
                foreach(var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "一度の実行でクリアすることができました！\nこのように一度の実行でクリアすることが目標です。\nステージは後ろに行けばいくほど難しくなるので、頑張って挑戦してみましょう！";
                        tutorial++;
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
            }
            else if(tutorial == 10)
            {
                richTextBox1.Text = "パート3 繰り返し";
                tutorial++;
            }
            else if(tutorial == 11)
            {
                richTextBox1.Text = "ゴールが主人公の5マス先にあります。\nつまり 前へ進む ブロックを5つ配置すればクリアできますね！\nしかし5つも配置するのは大変です(´・ω・`)\n何か良い方法はないのでしょうか！！";
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
            }
            else if(tutorial == 12)
            {
                richTextBox1.Text = "そこで、繰り返し ブロックを配置します！\nまず 繰り返し ブロックを選択、その横にある条件セットから ずっと を選択し配置しましょう。\n次に前へ進むを1つだけ配置します。\n最後に ここまで ブロックを配置します。\n準備ができたら、連続実行をクリックしましょう！";
                tutorial++;
            }
            else if (tutorial == 13)
            {
                
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "前へ進むブロックが1つだけでクリアすることができました！\nこのように繰り返しブロックは繰り返しとここまでの間にあるブロックをゴールに辿りつくまで何回も行います。";
                        tutorial++;
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
            }
            else if(tutorial == 14)
            {
                richTextBox1.Text = "繰り返しブロックの復習です(｀・ω・´)\nこのステージをブロック5つでクリアしてみましょう！\nヒント 2回前へ進み、1回左を向くを繰り返しましょう！\n失敗してしまった場合、次へをクリックすれば元の状態に戻ります！";
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
                    richTextBox1.Text = "繰り返しブロックの復習です(｀・ω・´)\nこのステージをブロック5つでクリアしてみましょう！\nヒント 2回前へ進み、1回左を向くを繰り返しましょう！\n失敗してしまった場合、次へをクリックすれば元の状態に戻ります！";
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
            }
            else if(tutorial == 16)
            {
                richTextBox1.Text = "パート4 条件文";
                tutorial++;
            }
            else if(tutorial == 17)
            {
                richTextBox1.Text = "最後は条件文です。\nこのステージをクリアするには命令セットと条件を組み合わせることが重要になります。\n次のようにブロックを配置してみましょう！\n繰り返し　ずっと\n前へ進む\nもし　左が壁でないなら\n左を向く\nここまで\nここまで\nそれでは連続実行をクリックしてみましょう！";
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
            }
            else if(tutorial == 18)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "ゴールできました！\nもしブロックは、その条件を満たしていた時にここまでブロックまでにある行動をします。\n今どの行動をしているかは矢印マークで確認できます。";
                        tutorial++;
                        button1.Text = "次へ";
                    }

                }
                if (tutorial != 19)
                {
                    richTextBox1.Text = "次のようにブロックを配置してみましょう！\n繰り返し　ずっと\n前へ進む\nもし　左が壁でないなら\n左を向く\nここまで\nここまで\nそれでは連続実行をクリックしてみましょう！\nブロックを間違って配置してしまったときは、全削除ですべてのブロックを消せます！\nまた一つだけ消したいときは、消したいブロックの上で右クリックをして削除を選ぶと消すことができます！";
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
                }
            }
            else if(tutorial == 19)
            {
                button1.Text = "やり直し";
                richTextBox1.Text = "最後にこのステージをクリアしてみましょう！\nわからない時は、やり直しボタンを押すとヒントが出るよ！";
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
            }
            else if(tutorial==20)
            {
                foreach (var n in ListObjectBoard)
                {
                    if (n is GoalObject && controlobj is PlayerObject && (controlobj.ObjectPositionX == n.ObjectPositionX && controlobj.ObjectPositionY == n.ObjectPositionY))
                    {
                        richTextBox1.Text = "ゴールできました！\n以上でチュートリアルはおしまいです。\n全ステージクリアを目指してがんばろう！";
                        tutorial++;
                        button1.Text = "次へ";
                        tutorial++;
                    }

                }
                richTextBox1.Text = "ヒント\nもしブロックを2回使います。順番は、\nもし左が壁でないなら\nもし右が壁でないなら\nの順番です。";
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
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "パート1 前への進み方";
            tutorial = 2;
        }

        private void label13_Click(object sender, EventArgs e)
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

        private void label14_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "パート3 繰り返し";
            tutorial = 11;
        }

        private void label15_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "パート4 条件文";
            tutorial = 17;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage11";
            var resource = Properties.Resources.stage11;
            try
            {
                loadDataset(path, resource);
                stagecount = 11;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage12";
            var resource = Properties.Resources.stage12;
            try
            {
                loadDataset(path, resource);
                stagecount = 12;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage13";
            var resource = Properties.Resources.stage13;
            try
            {
                loadDataset(path, resource);
                stagecount = 13;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage14";
            var resource = Properties.Resources.stage14;
            try
            {
                loadDataset(path, resource);
                stagecount = 14;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label20_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage15";
            var resource = Properties.Resources.stage15;
            try
            {
                loadDataset(path, resource);
                stagecount = 15;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label21_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage16";
            var resource = Properties.Resources.stage16;
            try
            {
                loadDataset(path, resource);
                stagecount = 16;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label22_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage17";
            var resource = Properties.Resources.stage17;
            try
            {
                loadDataset(path, resource);
                stagecount = 17;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label23_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage18";
            var resource = Properties.Resources.stage18;
            try
            {
                loadDataset(path, resource);
                stagecount = 18;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label24_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage19";
            var resource = Properties.Resources.stage19;
            try
            {
                loadDataset(path, resource);
                stagecount = 19;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void label25_Click(object sender, EventArgs e)
        {
            string path = "Userdata/stage20";
            var resource = Properties.Resources.stage20;
            try
            {
                loadDataset(path, resource);
                stagecount = 20;
                stage.Text = "現在のステージ:" + stagecount;
            }
            catch (Exception exc)
            {

            }
        }

        private void pictureBox1_DragOver(object sender, DragEventArgs e)
        {
            
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

    }

        #endregion







    interface BoardPosition //ボードの仕様はBoardDataクラス以外で変更不可，外部からのアクセスを制限
    {
        int BoardPositionXmax { get; }
        int BoardPositionYmax { get; }
        bool[,] BoardObjectCanMove { get; set; }
        List<BoardObject> ListObjectBoard { get; set; }
    }

}
