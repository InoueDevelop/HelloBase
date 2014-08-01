namespace HelloMaze
{
    partial class ClearForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.newgame = new System.Windows.Forms.Button();
            this.Loada = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(260, 180);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // newgame
            // 
            this.newgame.Location = new System.Drawing.Point(30, 217);
            this.newgame.Name = "newgame";
            this.newgame.Size = new System.Drawing.Size(75, 23);
            this.newgame.TabIndex = 1;
            this.newgame.Text = "最初から";
            this.newgame.UseVisualStyleBackColor = true;
            this.newgame.Click += new System.EventHandler(this.newgame_Click);
            // 
            // Loada
            // 
            this.Loada.Location = new System.Drawing.Point(167, 217);
            this.Loada.Name = "Loada";
            this.Loada.Size = new System.Drawing.Size(92, 23);
            this.Loada.TabIndex = 2;
            this.Loada.Text = "次のステージへ";
            this.Loada.UseVisualStyleBackColor = true;
            this.Loada.Click += new System.EventHandler(this.Loada_Click);
            // 
            // ClearForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Loada);
            this.Controls.Add(this.newgame);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ClearForm";
            this.Text = "おめでとう！！";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button newgame;
        private System.Windows.Forms.Button Loada;
    }
}