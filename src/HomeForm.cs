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
            EnterFullScreen();
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
            gameForm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
