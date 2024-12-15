using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FP
{
    public class GameClock
    {
        private TimeSpan gameTime;
        public TimeSpan GameTime { get { return gameTime; } }
        private System.Windows.Forms.Timer gameTimer;
        private Font clockFont;
        private Brush backgroundBrush;
        private Brush textBrush;
        private int padding;
        private int marginRight;
        private int marginTop;
        private int cornerRadius;

        public event EventHandler TimeUpdated;

        public GameClock()
        {
            // Inisialisasi waktu game mulai dari 12:00
            gameTime = new TimeSpan(12, 0, 0);

            gameTimer = new System.Windows.Forms.Timer(); 
            gameTimer.Interval = 1000; // 1 detik
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            clockFont = new Font("Segoe UI", 16, FontStyle.Bold);
            backgroundBrush = new SolidBrush(Color.LightGray);
            textBrush = new SolidBrush(Color.Black);
            padding = 10;
            marginRight = 20;
            marginTop = 20;
            cornerRadius = 10;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // 20 detik setiap 1 detik
            gameTime = gameTime.Add(new TimeSpan(0, 0, 20));

            TimeUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(Graphics g, int formWidth)
        {
            string timeText = gameTime.ToString(@"hh\:mm");

            SizeF textSize = g.MeasureString(timeText, clockFont);

            // Menentukan ukuran kotak dengan padding
            Rectangle rect = new Rectangle(
                formWidth - (int)textSize.Width - 2 * padding - marginRight,
                marginTop,
                (int)textSize.Width + 2 * padding,
                (int)textSize.Height + 2 * padding
            );

            // Membuat path untuk kotak dengan sudut tumpul
            using (GraphicsPath path = GetRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(backgroundBrush, path);

                // Menggambar teks jam
                float textX = rect.X + (rect.Width - textSize.Width) / 2;
                float textY = rect.Y + (rect.Height - textSize.Height) / 2;
                g.DrawString(timeText, clockFont, textBrush, textX, textY);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Menghentikan dan membersihkan resource yang digunakan oleh GameClock.
        /// </summary>
        public void Dispose()
        {
            // Hentikan timer jika masih berjalan
            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Tick -= GameTimer_Tick;
                gameTimer.Dispose();
                gameTimer = null;
            }

            // Dispose resource lainnya
            if (clockFont != null)
            {
                clockFont.Dispose();
                clockFont = null;
            }

            if (backgroundBrush != null)
            {
                backgroundBrush.Dispose();
                backgroundBrush = null;
            }

            if (textBrush != null)
            {
                textBrush.Dispose();
                textBrush = null;
            }
        }

        public void ResetTime()
        {
            gameTime = new TimeSpan(12, 0, 0);
            TimeUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
