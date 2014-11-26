using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;

namespace Planets
{
    public static class Debug
    {
        public static bool Enabled = true;

        public static Form DebugForm;
        private static RichTextBox TextBox;

        static Debug()
        {
            DebugForm = new Form();
            DebugForm.Closing += delegate(object sender, CancelEventArgs args) { args.Cancel = true; DebugForm.Hide(); };
            DebugForm.Size = new Size(800, 600);
            DebugForm.TopMost = true;
            DebugForm.LostFocus += (sender, args) => DebugForm.Hide();

            TextBox = new RichTextBox();
            TextBox.Size = DebugForm.ClientSize;
            TextBox.BackColor = Color.Black;
            TextBox.ForeColor = Color.Red;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.Font = new Font(FontFamily.GenericMonospace, (float)14.0, FontStyle.Bold);

            DebugForm.Controls.Add(TextBox);
        }

        public static void AddMessage(string message)
        {
            lock (TextBox)
            {
                TextBox.Text += message + "\n";
            }
        }

        public static void ShowWindow()
        {
            PlanetsLauncher.HostForm.BeginInvoke(new Action(DebugForm.Show)); 
        }
    }
}
