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
        public Bitmap fore;
        Point sp;    //イベント発生時に保持されるマウスの画面座標

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

        #endregion

        public BoardData() //コンストラクタ
        {

            InitializeComponent();
            back = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            fore = new Bitmap(pictureBox1.Width, pictureBox1.Height);


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

            controlobj = new PlayerObject(gridsizeheight / 2, gridsizeheight / 2);
            ListObjectBoard.Add(controlobj);
            bmppaint.ObjectSetPaint(controlobj.ObjectPositionX, controlobj.ObjectPositionY, fore, ref CanPutObjectOnBoard, controlobj.ObjectSelectNum);

            pictureBox1.Refresh();
            g.Dispose();
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


        private void pictureBox1_Click(object sender, EventArgs e)//マウスクリックによるオブジェクトの操作権限の移行
        {
            int x = -1;
            int y = -1;

            List<int> checkman = new List<int>();


            Point sp = System.Windows.Forms.Cursor.Position;
            System.Drawing.Point cp = pictureBox1.PointToClient(sp);

            GetCursolPosition(cp.X, cp.Y, ref x, ref y);  
            squareX.Text = "squareX:" + x;
            squareY.Text = "squareY:" + y;



            if (-1 < x)
            {
                //bmppaint.PointSquare(x,y,fore);
                //pictureBox1.Refresh();

                switch (CanPutObjectOnBoard[x, y])
                {
                    case (false):
                        {
                            if (ListObjectBoard != null)
                            {

                                controlobj = ListObjectBoard.Find(p => p.ObjectPositionX == x && p.ObjectPositionY == y&&(p is PlayerObject ||p is EnemyObject||p is WallObject));
                            }

                            break;
                        }

                    case (true):
                        {
                            //bmppaint.ObjectSetPaint(x, y, fore, ref CanPutObjectOnBoard,wall.ObjectSelectNum);
                            //pictureBox1.Refresh();
                            break;
                        }
                }
            }
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
            }
            if (e.KeyCode == Keys.A) //left
            {

                MoveOperation(controlobj, 3, 1);
            }
            if (e.KeyCode == Keys.S)//Up
            {
                MoveOperation(controlobj, 1, 1);
            }
            if (e.KeyCode == Keys.Z) //down
            {
                MoveOperation(controlobj, 4, 1);
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
                    }
                    break;
            }
        }

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

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)//コンテキストメニューでオブジェクトを削除
        {

            int x = -1;
            int y = -1;

            Point cp = Object_Control_Menu.SourceControl.PointToClient(sp);
            GetCursolPosition(cp.X, cp.Y, ref x, ref y);

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

    }







    interface BoardPosition //ボードの仕様はBoardDataクラス以外で変更不可，外部からのアクセスを制限
    {
        int BoardPositionXmax { get; }
        int BoardPositionYmax { get; }
        bool[,] BoardObjectCanMove { get; set; }
        List<BoardObject> ListObjectBoard { get; set; }
    }

}
