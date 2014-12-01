using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Planets
{
    public static class Debug
    {
        public static bool Enabled = true;

        private static readonly Form DebugForm;
        private static readonly RichTextBox TextBox;

        static Debug()
        {
            DebugForm = new Form();
            DebugForm.Closing += delegate(object sender, CancelEventArgs args) { args.Cancel = true; DebugForm.Hide(); };
            DebugForm.Size = new Size(800, 600);
            DebugForm.TopMost = true;
            DebugForm.LostFocus += (sender, args) => DebugForm.Hide();

            TextBox = new RichTextBox
            {
                Size = DebugForm.ClientSize,
                BackColor = Color.Black,
                ForeColor = Color.Red,
                BorderStyle = BorderStyle.None,
                Font = new Font(FontFamily.GenericMonospace, (float) 14.0, FontStyle.Bold)
            };
            TextBox.TextChanged += delegate
            {
                TextBox.SelectionStart = TextBox.Text.Length;
                TextBox.ScrollToCaret();
            };
            DebugForm.Controls.Add(TextBox);
        }

        public static void AddMessage(string message)
        {
            lock (TextBox)
            {
                if (TextBox.InvokeRequired)
                    TextBox.Invoke(new Action(() => TextBox.Text += message + "\n"));
                else
                    TextBox.Text += message + "\n";
            }
        }

        public static void ShowWindow()
        {
            PlanetsLauncher.HostForm.BeginInvoke(new Action(DebugForm.Show));
        }
    }
}
