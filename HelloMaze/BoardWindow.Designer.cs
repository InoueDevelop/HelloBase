namespace HelloMaze
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
			this.Object_Control_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.主人公を置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.敵を置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.アイテムを置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.壁を置くtoolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.左ドラッグtoolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.EnemysettoolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.壁を置くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.アイテムを置くToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ゴールを作るToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.削除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.設定なしToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.squareX = new System.Windows.Forms.Label();
			this.squareY = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.ロードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.セーブToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.stage = new System.Windows.Forms.Label();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.settingobj = new System.Windows.Forms.Label();
			this.Object_Control_Menu.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.SuspendLayout();
			// 
			// Object_Control_Menu
			// 
			this.Object_Control_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主人公を置くToolStripMenuItem,
            this.敵を置くToolStripMenuItem,
            this.アイテムを置くToolStripMenuItem,
            this.壁を置くtoolStripMenuItem2,
            this.toolStripMenuItem2,
            this.左ドラッグtoolStripMenuItem4,
            this.toolStripMenuItem1,
            this.削除ToolStripMenuItem});
			this.Object_Control_Menu.Name = "Object_Control_Menu";
			this.Object_Control_Menu.Size = new System.Drawing.Size(186, 206);
			this.Object_Control_Menu.Opened += new System.EventHandler(this.Object_Control_Menu_Opened);
			// 
			// 主人公を置くToolStripMenuItem
			// 
			this.主人公を置くToolStripMenuItem.Name = "主人公を置くToolStripMenuItem";
			this.主人公を置くToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.主人公を置くToolStripMenuItem.Text = "主人公を置く";
			this.主人公を置くToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PutPlayerToolStripMenuItem_MouseDown);
			// 
			// 敵を置くToolStripMenuItem
			// 
			this.敵を置くToolStripMenuItem.Name = "敵を置くToolStripMenuItem";
			this.敵を置くToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.敵を置くToolStripMenuItem.Text = "敵を置く";
			this.敵を置くToolStripMenuItem.Click += new System.EventHandler(this.PutEnemyToolStripMenuItem_Click);
			// 
			// アイテムを置くToolStripMenuItem
			// 
			this.アイテムを置くToolStripMenuItem.Name = "アイテムを置くToolStripMenuItem";
			this.アイテムを置くToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.アイテムを置くToolStripMenuItem.Text = "アイテムを置く";
			this.アイテムを置くToolStripMenuItem.Click += new System.EventHandler(this.PutItemToolStripMenuItem_Click);
			// 
			// 壁を置くtoolStripMenuItem2
			// 
			this.壁を置くtoolStripMenuItem2.Name = "壁を置くtoolStripMenuItem2";
			this.壁を置くtoolStripMenuItem2.Size = new System.Drawing.Size(185, 28);
			this.壁を置くtoolStripMenuItem2.Text = "壁を置く";
			this.壁を置くtoolStripMenuItem2.Click += new System.EventHandler(this.PutWalltoolStripMenuItem2_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 28);
			this.toolStripMenuItem2.Text = "ゴールを作る";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.GoaltoolStripMenuItem2_Click);
			// 
			// 左ドラッグtoolStripMenuItem4
			// 
			this.左ドラッグtoolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EnemysettoolStripMenuItem4,
            this.壁を置くToolStripMenuItem,
            this.アイテムを置くToolStripMenuItem1,
            this.ゴールを作るToolStripMenuItem,
            this.削除ToolStripMenuItem1,
            this.設定なしToolStripMenuItem});
			this.左ドラッグtoolStripMenuItem4.Name = "左ドラッグtoolStripMenuItem4";
			this.左ドラッグtoolStripMenuItem4.Size = new System.Drawing.Size(185, 28);
			this.左ドラッグtoolStripMenuItem4.Text = "左ドラッグ設定";
			// 
			// EnemysettoolStripMenuItem4
			// 
			this.EnemysettoolStripMenuItem4.Name = "EnemysettoolStripMenuItem4";
			this.EnemysettoolStripMenuItem4.Size = new System.Drawing.Size(185, 28);
			this.EnemysettoolStripMenuItem4.Text = "敵を置く";
			this.EnemysettoolStripMenuItem4.Click += new System.EventHandler(this.EnemysettoolStripMenuItem4_Click);
			// 
			// 壁を置くToolStripMenuItem
			// 
			this.壁を置くToolStripMenuItem.Name = "壁を置くToolStripMenuItem";
			this.壁を置くToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.壁を置くToolStripMenuItem.Text = "壁を置く";
			this.壁を置くToolStripMenuItem.Click += new System.EventHandler(this.壁を置くToolStripMenuItem_Click);
			// 
			// アイテムを置くToolStripMenuItem1
			// 
			this.アイテムを置くToolStripMenuItem1.Name = "アイテムを置くToolStripMenuItem1";
			this.アイテムを置くToolStripMenuItem1.Size = new System.Drawing.Size(185, 28);
			this.アイテムを置くToolStripMenuItem1.Text = "アイテムを置く";
			this.アイテムを置くToolStripMenuItem1.Click += new System.EventHandler(this.アイテムを置くToolStripMenuItem1_Click);
			// 
			// ゴールを作るToolStripMenuItem
			// 
			this.ゴールを作るToolStripMenuItem.Name = "ゴールを作るToolStripMenuItem";
			this.ゴールを作るToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.ゴールを作るToolStripMenuItem.Text = "ゴールを作る";
			this.ゴールを作るToolStripMenuItem.Click += new System.EventHandler(this.ゴールを作るToolStripMenuItem_Click);
			// 
			// 削除ToolStripMenuItem1
			// 
			this.削除ToolStripMenuItem1.Name = "削除ToolStripMenuItem1";
			this.削除ToolStripMenuItem1.Size = new System.Drawing.Size(185, 28);
			this.削除ToolStripMenuItem1.Text = "削除";
			this.削除ToolStripMenuItem1.Click += new System.EventHandler(this.削除ToolStripMenuItem1_Click);
			// 
			// 設定なしToolStripMenuItem
			// 
			this.設定なしToolStripMenuItem.Name = "設定なしToolStripMenuItem";
			this.設定なしToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.設定なしToolStripMenuItem.Text = "設定なし";
			this.設定なしToolStripMenuItem.Click += new System.EventHandler(this.設定なしToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
			// 
			// 削除ToolStripMenuItem
			// 
			this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
			this.削除ToolStripMenuItem.Size = new System.Drawing.Size(185, 28);
			this.削除ToolStripMenuItem.Text = "削除";
			this.削除ToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
			// 
			// squareX
			// 
			this.squareX.AutoSize = true;
			this.squareX.Location = new System.Drawing.Point(84, 49);
			this.squareX.Name = "squareX";
			this.squareX.Size = new System.Drawing.Size(61, 15);
			this.squareX.TabIndex = 1;
			this.squareX.Text = "squareX:";
			this.squareX.Visible = false;
			// 
			// squareY
			// 
			this.squareY.AutoSize = true;
			this.squareY.Location = new System.Drawing.Point(84, 78);
			this.squareY.Name = "squareY";
			this.squareY.Size = new System.Drawing.Size(61, 15);
			this.squareY.TabIndex = 2;
			this.squareY.Text = "squareY:";
			this.squareY.Visible = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(1051, 31);
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
			this.toolStripMenuItem3.Size = new System.Drawing.Size(82, 27);
			this.toolStripMenuItem3.Text = "ファイル";
			// 
			// ロードToolStripMenuItem
			// 
			this.ロードToolStripMenuItem.Name = "ロードToolStripMenuItem";
			this.ロードToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
			this.ロードToolStripMenuItem.Text = "ロード";
			this.ロードToolStripMenuItem.Click += new System.EventHandler(this.ロードToolStripMenuItem_Click);
			// 
			// セーブToolStripMenuItem
			// 
			this.セーブToolStripMenuItem.Name = "セーブToolStripMenuItem";
			this.セーブToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
			this.セーブToolStripMenuItem.Text = "セーブ";
			this.セーブToolStripMenuItem.Click += new System.EventHandler(this.セーブToolStripMenuItem_Click);
			// 
			// 終了ToolStripMenuItem
			// 
			this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
			this.終了ToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
			this.終了ToolStripMenuItem.Text = "終了";
			this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label25);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.label23);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("メイリオ", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox1.Location = new System.Drawing.Point(16, 49);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Size = new System.Drawing.Size(325, 624);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ステージ選択";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(131, 521);
			this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(58, 45);
			this.label25.TabIndex = 24;
			this.label25.Text = "20";
			this.label25.Click += new System.EventHandler(this.label25_Click);
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(131, 476);
			this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(58, 45);
			this.label24.TabIndex = 23;
			this.label24.Text = "19";
			this.label24.Click += new System.EventHandler(this.label24_Click);
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(131, 431);
			this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(58, 45);
			this.label23.TabIndex = 22;
			this.label23.Text = "18";
			this.label23.Click += new System.EventHandler(this.label23_Click);
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(131, 386);
			this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(58, 45);
			this.label22.TabIndex = 21;
			this.label22.Text = "17";
			this.label22.Click += new System.EventHandler(this.label22_Click);
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(131, 341);
			this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(58, 45);
			this.label21.TabIndex = 20;
			this.label21.Text = "16";
			this.label21.Click += new System.EventHandler(this.label21_Click);
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(131, 296);
			this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(58, 45);
			this.label20.TabIndex = 19;
			this.label20.Text = "15";
			this.label20.Click += new System.EventHandler(this.label20_Click);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(131, 254);
			this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(58, 45);
			this.label19.TabIndex = 18;
			this.label19.Text = "14";
			this.label19.Click += new System.EventHandler(this.label19_Click);
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(131, 209);
			this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(58, 45);
			this.label18.TabIndex = 17;
			this.label18.Text = "13";
			this.label18.Click += new System.EventHandler(this.label18_Click);
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(131, 159);
			this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(58, 45);
			this.label17.TabIndex = 16;
			this.label17.Text = "12";
			this.label17.Click += new System.EventHandler(this.label17_Click);
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(131, 112);
			this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(58, 45);
			this.label16.TabIndex = 15;
			this.label16.Text = "11";
			this.label16.Click += new System.EventHandler(this.label16_Click);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(19, 261);
			this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(150, 45);
			this.label15.TabIndex = 14;
			this.label15.Text = "④ 条件文";
			this.label15.Visible = false;
			this.label15.Click += new System.EventHandler(this.label15_Click);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(19, 215);
			this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(180, 45);
			this.label14.TabIndex = 13;
			this.label14.Text = "③ 繰り返し";
			this.label14.Visible = false;
			this.label14.Click += new System.EventHandler(this.label14_Click);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(19, 166);
			this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(240, 45);
			this.label13.TabIndex = 12;
			this.label13.Text = "② 向きを変える";
			this.label13.Visible = false;
			this.label13.Click += new System.EventHandler(this.label13_Click);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(20, 116);
			this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(180, 45);
			this.label12.TabIndex = 11;
			this.label12.Text = "① 前へ進む";
			this.label12.Visible = false;
			this.label12.Click += new System.EventHandler(this.label12_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(12, 49);
			this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(290, 45);
			this.label11.TabIndex = 10;
			this.label11.Text = "チュートリアル開始";
			this.label11.Click += new System.EventHandler(this.label11_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(43, 521);
			this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(58, 45);
			this.label10.TabIndex = 9;
			this.label10.Text = "10";
			this.label10.Click += new System.EventHandler(this.label10_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(63, 476);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(39, 45);
			this.label9.TabIndex = 8;
			this.label9.Text = "9";
			this.label9.Click += new System.EventHandler(this.label9_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(63, 431);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(39, 45);
			this.label8.TabIndex = 7;
			this.label8.Text = "8";
			this.label8.Click += new System.EventHandler(this.label8_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(63, 386);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(39, 45);
			this.label7.TabIndex = 6;
			this.label7.Text = "7";
			this.label7.Click += new System.EventHandler(this.label7_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(63, 341);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 45);
			this.label6.TabIndex = 5;
			this.label6.Text = "6";
			this.label6.Click += new System.EventHandler(this.label6_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(63, 296);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 45);
			this.label5.TabIndex = 4;
			this.label5.Text = "5";
			this.label5.Click += new System.EventHandler(this.label5_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(63, 251);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 45);
			this.label4.TabIndex = 3;
			this.label4.Text = "4";
			this.label4.Click += new System.EventHandler(this.label4_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(63, 206);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 45);
			this.label3.TabIndex = 2;
			this.label3.Text = "3";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(63, 161);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 45);
			this.label2.TabIndex = 1;
			this.label2.Text = "2";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(63, 116);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 45);
			this.label1.TabIndex = 0;
			this.label1.Text = "1";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// stage
			// 
			this.stage.AutoSize = true;
			this.stage.Location = new System.Drawing.Point(849, 49);
			this.stage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.stage.Name = "stage";
			this.stage.Size = new System.Drawing.Size(106, 15);
			this.stage.TabIndex = 7;
			this.stage.Text = "現在のステージ:0";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(16, 680);
			this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(324, 208);
			this.richTextBox1.TabIndex = 9;
			this.richTextBox1.Text = "";
			this.richTextBox1.Visible = false;
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(236, 896);
			this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 29);
			this.button1.TabIndex = 10;
			this.button1.Text = "次へ";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(363, 685);
			this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(600, 240);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 11;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Visible = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.ContextMenuStrip = this.Object_Control_Menu;
			this.pictureBox1.Location = new System.Drawing.Point(363, 78);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(600, 600);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragOver);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::HelloMaze.Properties.Resources.goal;
			this.pictureBox3.InitialImage = null;
			this.pictureBox3.Location = new System.Drawing.Point(385, 685);
			this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(281, 240);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox3.TabIndex = 12;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Visible = false;
			// 
			// settingobj
			// 
			this.settingobj.AutoSize = true;
			this.settingobj.Location = new System.Drawing.Point(383, 49);
			this.settingobj.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.settingobj.Name = "settingobj";
			this.settingobj.Size = new System.Drawing.Size(0, 15);
			this.settingobj.TabIndex = 13;
			// 
			// BoardData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.Bisque;
			this.ClientSize = new System.Drawing.Size(1051, 976);
			this.ControlBox = false;
			this.Controls.Add(this.settingobj);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.stage);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.squareY);
			this.Controls.Add(this.squareX);
			this.Controls.Add(this.pictureBox1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BoardData";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "迷路を解こう";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoardData_KeyDown);
			this.Object_Control_Menu.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem セーブToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ロードToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label stage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label settingobj;
        private System.Windows.Forms.ToolStripMenuItem 左ドラッグtoolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem EnemysettoolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 壁を置くToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem アイテムを置くToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ゴールを作るToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 削除ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 設定なしToolStripMenuItem;
    }
}

