using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitmapPaint;

namespace HelloMaze
{
    /// <summary>
    /// Boardに配置されるオブジェクト操作を行うクラス
    /// </summary>
    [Serializable]
    public class BoardObject 
    {
        /// <summary>
        /// オブジェクトの横マス座標
        /// </summary>
        internal int ObjectPositionX;
        public int objectPositionX
        {
            get {return ObjectPositionX;}
            protected set { ObjectPositionX = value; }
        }
        /// <summary>
        /// オブジェクトの縦マス座標
        /// </summary>
        internal int ObjectPositionY;
        public int objectPositionY
        {
            get {return ObjectPositionY;}
            protected set { ObjectPositionY=value; }
        }

        /// <summary>
        /// オブジェクトの向きを示す列挙型
        /// </summary>
        public enum ObjectDirection { init, Right, Down ,Left,Up };
        /// <summary>
        /// オブジェクトの現在の向き
        /// </summary>
        public int objectDirection = (int)ObjectDirection.Up;

         /// <summary>
         /// オブジェクトの種類を識別する番号
         /// <remarks>
         /// 0:壁,1:プレイヤー,2:敵,3:アイテム.4:ゴール
         /// </remarks>
         /// </summary>
        internal int ObjectSelectNum;  //0:Wall,1:Player,2:Enemy,3:Item,4:Goal

        enum objectselect
        {
            Wall,
            Player,
            Enemy,
            Item,
            Goal
        }

        internal bool OperateMove=false; //ターンの間にAIまたはプレイヤーが動いたかどうかの判定

        public BoardObject()
        {

            ObjectPositionX = 0;
            ObjectPositionY = 0;
            ObjectSelectNum = -1;
        }

       public BoardObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
        }

        /// <summary>
        /// オブジェクトを上に移動させる
        /// </summary>
       internal void moveUp() { ObjectPositionY--; }
       internal void moveDown() { ObjectPositionY++; }
       internal void moveRight() {ObjectPositionX++; }
       internal void moveLeft() { ObjectPositionX--; }

        /// <summary>
        /// 向きの方向へ進む
        /// </summary>
       internal void moveStraight() { 
           switch (objectDirection){
               case (int)ObjectDirection.Up:
                   moveUp();
                   break;
               case (int)ObjectDirection.Down:
                   moveDown();
                   break;
               case (int)ObjectDirection.Right:
                   moveRight();
                   break;
               case (int)ObjectDirection.Left:
                   moveLeft();
                   break;
           }
       
       }
        /// <summary>
        /// オブジェクトの向きを変える
        /// </summary>
       internal void turnRight() { objectDirection = getRight(); }
       internal void turnLeft() { objectDirection = getLeft(); }

        /// <summary>
        /// 現在の向きに対して右に回転した方向を得る
        /// </summary>
        /// <returns></returns>
       public int getRight()
       {
           int res = 0;
           switch (objectDirection)
           {
               case (int)ObjectDirection.Left:
                   res = (int)ObjectDirection.Up;
                   break;
               case (int)ObjectDirection.Right:
                   res = (int)ObjectDirection.Down;
                   break;
               case (int)ObjectDirection.Up:
                   res = (int)ObjectDirection.Right;
                   break;
               case (int)ObjectDirection.Down:
                   res = (int)ObjectDirection.Left;
                   break;
           }
           return res;
       }
        /// <summary>
       /// 現在の向きに対して左に回転した方向を得る
        /// </summary>
        /// <returns></returns>
       public int getLeft()
       {
           int res=0;
           switch (objectDirection)
           {
               case (int)ObjectDirection.Up:
                   res = (int)ObjectDirection.Left;
                   break;
               case (int)ObjectDirection.Down:
                   res = (int)ObjectDirection.Right;
                   break;
               case (int)ObjectDirection.Right:
                   res = (int)ObjectDirection.Up;
                   break;
               case (int)ObjectDirection.Left:
                   res = (int)ObjectDirection.Down;
                   break;
           }
           return res;
       }


    

    }
    [Serializable]
    class PlayerObject : BoardObject
    {
       

        public PlayerObject() {
            //ObjectPositionX=1;
            //ObjectPositionY=1;
        
            //if (BoardData.BoardPosition.BoardPositionCanMove[ObjectPositionX, ObjectPositionY])
            //{ }
            //else {}
        }

        public PlayerObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
            ObjectSelectNum=1;
        }


    }
    [Serializable]
    class WallObject :BoardObject
    { 
     public WallObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
            ObjectSelectNum = 0;
         }
    }
    [Serializable]
    class EnemyObject : BoardObject
    { 
     public EnemyObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
            ObjectSelectNum = 2;
         }
    }
    [Serializable]
    class ItemObject : BoardObject
    {
        public ItemObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
            ObjectSelectNum = 3;
        }
        }
    [Serializable]
    class GoalObject : BoardObject
    { 
    public GoalObject(int X,int Y)
        {
            ObjectPositionX = X;
            ObjectPositionY = Y;
            ObjectSelectNum = 4;
        }
    }


}
