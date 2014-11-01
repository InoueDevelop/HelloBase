using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Plock
{
	public partial class Credits : Form
	{
		public Credits(bool OK)
		{
			InitializeComponent();
			pictureBox1.Image = Properties.Resources.hidarite_answer;
			if (OK) pictureBox2.Image = Properties.Resources.ex_answer;
		}
	}
}
