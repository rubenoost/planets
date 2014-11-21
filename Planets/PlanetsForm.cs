using System;
using System.Diagnostics;
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
            this.DoubleBuffered = true;
            engine = new MainEngine(this);

            // Event handlers
            Closed += (sender, args) => Process.GetCurrentProcess().Kill();
        }

        private void PlanetsForm_Load(object sender, EventArgs e)
        {

        }

        //#region Logo test stuff
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    pictureBox1.Visible = true;
        //    this.pictureBox1.Image = global::Planets.Properties.Resources.LogoFinal;
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    pictureBox1.Visible = true;
        //    this.pictureBox1.Image = global::Planets.Properties.Resources.LogoFinal_Inv;
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    pictureBox1.Visible = false;
        //}

        //#endregion
    }
}
