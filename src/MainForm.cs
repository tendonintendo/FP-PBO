using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace FP
{
    public class MainForm : Form
    {
        protected bool isFullScreen = false;
        protected Size windowedSize = new Size(745, 450);

        public MainForm()
        {
            this.FormClosing += MainForm_FormClosing;
        }

        protected void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                ExitFullScreen();
            }
            else
            {
                EnterFullScreen();
            }
            this.Invalidate();
        }

        protected void EnterFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            isFullScreen = true;
        }

        protected void ExitFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Normal;
            this.Size = windowedSize;
            isFullScreen = false;
        }

        protected void LoadBackgroundImage(string relativePath)
        {
            string bgPath = Path.GetFullPath(Path.Combine(Application.StartupPath, relativePath));
            if (File.Exists(bgPath))
            {
                this.BackgroundImage = Image.FromFile(bgPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                MessageBox.Show($"Background image not found: {bgPath}");
                Console.WriteLine($"Invalid background path: {bgPath}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
