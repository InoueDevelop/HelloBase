using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HelloMaze
{
    public partial class ClearForm : Form
    {
       public bool newgamestart = false;
       public bool Loaddatastart = false;
       public string Loadtext{
           set { Loada.Text = value; }
       }

        public ClearForm(int stagecount)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if(stagecount==20)
            {
                this.pictureBox1.Image = Properties.Resources.perfectgoal;
            }
            else
            {
                this.pictureBox1.Image = Properties.Resources.evgoal;
            }

            pictureBox1.Refresh();
     
        }

        private void newgame_Click(object sender, EventArgs e)
        {
            newgamestart = true;
            this.Close();
        }

        private void Loada_Click(object sender, EventArgs e)
        {
            Loaddatastart = true;
            this.Close();
        }


    }
}
