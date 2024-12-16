using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FP
{
    public partial class HomeForm : MainForm
    {
        public HomeForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.KeyPreview = true;
            this.KeyDown += HomeForm_KeyDown;
            LoadBackgroundImage("../../../../images/Main Menu/bg.jpg");
        }

        private void HomeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm();
            gameForm.FormClosed += (s, args) => this.Show(); // Tampilkan kembali HomeForm saat GameForm ditutup
            gameForm.Show();
            this.Hide();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
