using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FP
{
    public partial class HomeForm : Form
    {
        private bool isFullScreen = true;
        private Size windowedSize = new Size(745, 450); 
        private Point windowedLocation;

        public HomeForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true; 
            this.KeyPreview = true; 

            this.KeyDown += HomeForm_KeyDown;
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            string relativePath = "../../../../images/Main Menu/bg.jpg";
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

        private void HomeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
            }
        }
        private void EnterFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            isFullScreen = true;
        }
        private void ExitFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(745, 450);
            isFullScreen = false;
        }
        private void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                ExitFullScreen();
            }
            else
            {
                EnterFullScreen();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
