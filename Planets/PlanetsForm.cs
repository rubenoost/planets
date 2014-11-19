using System;
using System.Drawing;
using System.Windows.Forms;
using Planets.Controller;

namespace Planets
{
    public partial class PlanetsForm : Form
    {

        MainEngine engine;

        public PlanetsForm()
        {
            InitializeComponent();
            engine = new MainEngine(this);
        }

        private void PlanetsForm_Load(object sender, EventArgs e)
        {

        }

        #region Logo test stuff
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            this.pictureBox1.Image = global::Planets.Properties.Resources.LogoFinal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            this.pictureBox1.Image = global::Planets.Properties.Resources.LogoFinal_Inv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
        }

        #endregion
    }
}
