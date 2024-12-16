using System;
using System.Drawing;
using System.Windows.Forms;

namespace FP
{
    public class WinForm : MainForm
    {
        public WinForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "You Won!";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.LightGreen;

            Label winLabel = new Label
            {
                Text = "Congratulations, You Won!",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            Button restartButton = new Button
            {
                Text = "Restart",
                Dock = DockStyle.Left,
                Width = 200,
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            restartButton.Click += RestartGame;

            Button exitButton = new Button
            {
                Text = "Exit",
                Dock = DockStyle.Right,
                Width = 200,
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            exitButton.Click += (s, e) => Application.Exit();

            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.Transparent
            };
            buttonPanel.Controls.Add(restartButton);
            buttonPanel.Controls.Add(exitButton);

            this.Controls.Add(winLabel);
            this.Controls.Add(buttonPanel);
        }

        private void RestartGame(object sender, EventArgs e)
        {
            HomeForm homeForm = new HomeForm();
            homeForm.Show();
            this.Hide();
        }
    }
}
