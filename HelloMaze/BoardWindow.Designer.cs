﻿namespace HelloMaze
{
    partial class BoardData
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Object_Control_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.主人公を置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.敵を置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アイテムを置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.壁を置くtoolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareX = new System.Windows.Forms.Label();
            this.squareY = new System.Windows.Forms.Label();
            this.save_button = new System.Windows.Forms.Button();
            this.load_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ロードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.セーブToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bGMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.再生ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ナレーションToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Object_Control_Menu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.Object_Control_Menu;
            this.pictureBox1.Location = new System.Drawing.Point(570, 66);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Object_Control_Menu
            // 
            this.Object_Control_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主人公を置くToolStripMenuItem,
            this.敵を置くToolStripMenuItem,
            this.アイテムを置くToolStripMenuItem,
            this.壁を置くtoolStripMenuItem2,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.削除ToolStripMenuItem});
            this.Object_Control_Menu.Name = "Object_Control_Menu";
            this.Object_Control_Menu.Size = new System.Drawing.Size(158, 154);
            this.Object_Control_Menu.Opened += new System.EventHandler(this.Object_Control_Menu_Opened);
            // 
            // 主人公を置くToolStripMenuItem
            // 
            this.主人公を置くToolStripMenuItem.Name = "主人公を置くToolStripMenuItem";
            this.主人公を置くToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.主人公を置くToolStripMenuItem.Text = "主人公を置く";
            this.主人公を置くToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PutPlayerToolStripMenuItem_MouseDown);
            // 
            // 敵を置くToolStripMenuItem
            // 
            this.敵を置くToolStripMenuItem.Name = "敵を置くToolStripMenuItem";
            this.敵を置くToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.敵を置くToolStripMenuItem.Text = "敵を置く";
            this.敵を置くToolStripMenuItem.Click += new System.EventHandler(this.PutEnemyToolStripMenuItem_Click);
            // 
            // アイテムを置くToolStripMenuItem
            // 
            this.アイテムを置くToolStripMenuItem.Name = "アイテムを置くToolStripMenuItem";
            this.アイテムを置くToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.アイテムを置くToolStripMenuItem.Text = "アイテムを置く";
            this.アイテムを置くToolStripMenuItem.Click += new System.EventHandler(this.PutItemToolStripMenuItem_Click);
            // 
            // 壁を置くtoolStripMenuItem2
            // 
            this.壁を置くtoolStripMenuItem2.Name = "壁を置くtoolStripMenuItem2";
            this.壁を置くtoolStripMenuItem2.Size = new System.Drawing.Size(157, 24);
            this.壁を置くtoolStripMenuItem2.Text = "壁を置く";
            this.壁を置くtoolStripMenuItem2.Click += new System.EventHandler(this.PutWalltoolStripMenuItem2_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(157, 24);
            this.toolStripMenuItem2.Text = "ゴールを作る";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.GoaltoolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 6);
            // 
            // 削除ToolStripMenuItem
            // 
            this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
            this.削除ToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.削除ToolStripMenuItem.Text = "削除";
            this.削除ToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // squareX
            // 
            this.squareX.AutoSize = true;
            this.squareX.Location = new System.Drawing.Point(439, 41);
            this.squareX.Name = "squareX";
            this.squareX.Size = new System.Drawing.Size(61, 15);
            this.squareX.TabIndex = 1;
            this.squareX.Text = "squareX:";
            // 
            // squareY
            // 
            this.squareY.AutoSize = true;
            this.squareY.Location = new System.Drawing.Point(439, 66);
            this.squareY.Name = "squareY";
            this.squareY.Size = new System.Drawing.Size(61, 15);
            this.squareY.TabIndex = 2;
            this.squareY.Text = "squareY:";
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(308, 381);
            this.save_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(125, 38);
            this.save_button.TabIndex = 3;
            this.save_button.Text = "save";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // load_button
            // 
            this.load_button.Location = new System.Drawing.Point(308, 441);
            this.load_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.load_button.Name = "load_button";
            this.load_button.Size = new System.Drawing.Size(125, 38);
            this.load_button.TabIndex = 4;
            this.load_button.Text = "load";
            this.load_button.UseVisualStyleBackColor = true;
            this.load_button.Click += new System.EventHandler(this.load_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.ヘルプToolStripMenuItem,
            this.bGMToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1209, 27);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ロードToolStripMenuItem,
            this.セーブToolStripMenuItem,
            this.終了ToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(63, 23);
            this.toolStripMenuItem3.Text = "ファイル";
            // 
            // ロードToolStripMenuItem
            // 
            this.ロードToolStripMenuItem.Name = "ロードToolStripMenuItem";
            this.ロードToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.ロードToolStripMenuItem.Text = "ロード";
            this.ロードToolStripMenuItem.Click += new System.EventHandler(this.ロードToolStripMenuItem_Click);
            // 
            // セーブToolStripMenuItem
            // 
            this.セーブToolStripMenuItem.Name = "セーブToolStripMenuItem";
            this.セーブToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.セーブToolStripMenuItem.Text = "セーブ";
            this.セーブToolStripMenuItem.Click += new System.EventHandler(this.セーブToolStripMenuItem_Click);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.終了ToolStripMenuItem.Text = "終了";
            this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(56, 23);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            // 
            // bGMToolStripMenuItem
            // 
            this.bGMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.再生ToolStripMenuItem,
            this.ナレーションToolStripMenuItem});
            this.bGMToolStripMenuItem.Name = "bGMToolStripMenuItem";
            this.bGMToolStripMenuItem.Size = new System.Drawing.Size(54, 23);
            this.bGMToolStripMenuItem.Text = "BGM";
            // 
            // 再生ToolStripMenuItem
            // 
            this.再生ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oNToolStripMenuItem,
            this.oFFToolStripMenuItem});
            this.再生ToolStripMenuItem.Name = "再生ToolStripMenuItem";
            this.再生ToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.再生ToolStripMenuItem.Text = "再生";
            // 
            // oNToolStripMenuItem
            // 
            this.oNToolStripMenuItem.Name = "oNToolStripMenuItem";
            this.oNToolStripMenuItem.Size = new System.Drawing.Size(107, 24);
            this.oNToolStripMenuItem.Text = "ON";
            // 
            // oFFToolStripMenuItem
            // 
            this.oFFToolStripMenuItem.Name = "oFFToolStripMenuItem";
            this.oFFToolStripMenuItem.Size = new System.Drawing.Size(107, 24);
            this.oFFToolStripMenuItem.Text = "OFF";
            // 
            // ナレーションToolStripMenuItem
            // 
            this.ナレーションToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oNToolStripMenuItem1,
            this.oFFToolStripMenuItem1});
            this.ナレーションToolStripMenuItem.Name = "ナレーションToolStripMenuItem";
            this.ナレーションToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.ナレーションToolStripMenuItem.Text = "ナレーション";
            // 
            // oNToolStripMenuItem1
            // 
            this.oNToolStripMenuItem1.Name = "oNToolStripMenuItem1";
            this.oNToolStripMenuItem1.Size = new System.Drawing.Size(107, 24);
            this.oNToolStripMenuItem1.Text = "ON";
            // 
            // oFFToolStripMenuItem1
            // 
            this.oFFToolStripMenuItem1.Name = "oFFToolStripMenuItem1";
            this.oFFToolStripMenuItem1.Size = new System.Drawing.Size(107, 24);
            this.oFFToolStripMenuItem1.Text = "OFF";
            // 
            // BoardData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 706);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.load_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.squareY);
            this.Controls.Add(this.squareX);
            this.Controls.Add(this.pictureBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BoardData";
            this.Text = "迷路を解こう";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoardData_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Object_Control_Menu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label squareX;
        private System.Windows.Forms.Label squareY;
        private System.Windows.Forms.ContextMenuStrip Object_Control_Menu;
        private System.Windows.Forms.ToolStripMenuItem 主人公を置くToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 敵を置くToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem アイテムを置くToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 削除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 壁を置くtoolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button load_button;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem セーブToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ロードToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bGMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 再生ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oFFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ナレーションToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem oFFToolStripMenuItem1;
    }
}

