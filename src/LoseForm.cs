using System;
using System.Drawing;
using System.Windows.Forms;

namespace FP
{
    public class LoseForm : MainForm
    {
        public LoseForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "You Lose!";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.LightCoral;

            Label loseLabel = new Label
            {
                Text = "Game Over! Better Luck Next Time!",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

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
            buttonPanel.Controls.Add(exitButton);

            this.Controls.Add(loseLabel);
            this.Controls.Add(buttonPanel);
        }


    }
}
