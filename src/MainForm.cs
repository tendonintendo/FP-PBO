using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FP
{
    public class MainForm : Form
    {
        private Ruangan currentRoom;
        private Size originalSize;
        private string selectedItemName;
        private bool isFullScreen = false;

        private int roomIndex = 0;
        private Ruangan[] rooms;

        public MainForm()
        {
            InitializeRooms();
            this.Text = this.currentRoom.Name;
            this.Size = new Size(745, 450);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            if (!string.IsNullOrEmpty(currentRoom.BackgroundImagePath) && File.Exists(currentRoom.BackgroundImagePath))
            {
                Image bgImage = Image.FromFile(currentRoom.BackgroundImagePath);
                this.BackgroundImage = bgImage;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                originalSize = bgImage.Size;
            }
            else
            {
                MessageBox.Show($"Background image not found: {currentRoom.BackgroundImagePath}");
                Console.WriteLine($"Invalid background path: {currentRoom.BackgroundImagePath}");
            }

            this.DoubleBuffered = true;
            this.MouseClick += MainForm_MouseClick;
        }

        private void InitializeRooms()
        {
            rooms = new Ruangan[]
            {
                new Ruangan("Ruangan 1", "../../../../images/Room 1/bg.png"),
                new Ruangan("Ruangan 2", "../../../../images/Room 2/bg.png"),
                new Ruangan("Ruangan 3", "../../../../images/Room 3/bg.png")
            };

            // Ruangan 1
            rooms[0].AddItem(new Benda("Table", new Point(150, 600), "../../../../images/Room 1/meja_pc_awal.png", new Size(420, 220)));
            rooms[0].AddItem(new Benda("Tabletop Speaker", new Point(450, 540), "../../../../images/Room 1/speaker.png", new Size(45, 60)));
            rooms[0].AddItem(new Benda("Lamp", new Point(450, 465), "../../../../images/Room 1/lampu_mati.png", new Size(100, 140)));
            rooms[0].AddItem(new Benda("PC", new Point(190, 375), "../../../../images/Room 1/pc_awal.png", new Size(250, 225)));
            rooms[0].AddItem(new Benda("Chair", new Point(0, 420), "../../../../images/Room 1/kursi_gaming.png", new Size(500, 470)));

            // Ruangan 2
            

            // Ruangan 3
            

            // Set ruangan pertama yang aktif
            currentRoom = rooms[roomIndex];
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (originalSize == null) return;

            float scaleX = (float)this.ClientSize.Width / originalSize.Width;
            float scaleY = (float)this.ClientSize.Height / originalSize.Height;

            float scale = Math.Min(scaleX, scaleY);

            float scaledBgWidth = originalSize.Width * scale;
            float scaledBgHeight = originalSize.Height * scale;

            float offsetX = (this.ClientSize.Width - scaledBgWidth) / 2;
            float offsetY = (this.ClientSize.Height - scaledBgHeight) / 2;

            foreach (var item in currentRoom.Items)
            {
                float scaledX = offsetX + item.Position.X * scale;
                float scaledY = offsetY + item.Position.Y * scale;

                float scaledWidth = item.ImageSize.Width * scale;
                float scaledHeight = item.ImageSize.Height * scale;

                RectangleF itemRect = new RectangleF(scaledX, scaledY, scaledWidth, scaledHeight);

                if (itemRect.Contains(e.Location))
                {
                    selectedItemName = item.Name;
                    Invalidate();
                    return;
                }
            }

            selectedItemName = null;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (originalSize == null) return;

            float scaleX = (float)this.ClientSize.Width / originalSize.Width;
            float scaleY = (float)this.ClientSize.Height / originalSize.Height;

            float scale = Math.Min(scaleX, scaleY);

            float scaledBgWidth = originalSize.Width * scale;
            float scaledBgHeight = originalSize.Height * scale;

            float offsetX = (this.ClientSize.Width - scaledBgWidth) / 2;
            float offsetY = (this.ClientSize.Height - scaledBgHeight) / 2;

            using (Image bgImage = Image.FromFile(currentRoom.BackgroundImagePath))
            {
                e.Graphics.DrawImage(bgImage, offsetX, offsetY, scaledBgWidth, scaledBgHeight);
            }

            foreach (var item in currentRoom.Items)
            {
                if (!string.IsNullOrEmpty(item.ImagePath) && File.Exists(item.ImagePath))
                {
                    using (Image itemImage = Image.FromFile(item.ImagePath))
                    {
                        float scaledX = offsetX + item.Position.X * scale;
                        float scaledY = offsetY + item.Position.Y * scale;

                        float scaledWidth = item.ImageSize.Width * scale;
                        float scaledHeight = item.ImageSize.Height * scale;

                        e.Graphics.DrawImage(itemImage, scaledX, scaledY, scaledWidth, scaledHeight);
                    }
                }
                else
                {
                    MessageBox.Show($"Item image not found: {item.ImagePath}");
                    Console.WriteLine($"Invalid item path: {item.ImagePath}");
                }
            }

            if (!string.IsNullOrEmpty(selectedItemName))
            {
                using (Font font = new Font("Arial", 16, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    SizeF textSize = e.Graphics.MeasureString(selectedItemName, font);
                    float textX = (this.ClientSize.Width - textSize.Width) / 2;
                    float textY = 10;
                    e.Graphics.DrawString(selectedItemName, font, brush, textX, textY);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Right)
            {
                roomIndex = (roomIndex + 1) % rooms.Length; // Pindah ke ruangan berikutnya (sirkuler)
                currentRoom = rooms[roomIndex];
                this.Text = currentRoom.Name; // Update judul jendela dengan nama ruangan baru
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                roomIndex = (roomIndex - 1 + rooms.Length) % rooms.Length; // Pindah ke ruangan sebelumnya (sirkuler)
                currentRoom = rooms[roomIndex];
                this.Text = currentRoom.Name; // Update judul jendela dengan nama ruangan baru
                Invalidate();
                return true;
            }

            if (keyData == Keys.F11)
            {
                ToggleFullScreen();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                ExitFullScreen();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
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

        private void EnterFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            isFullScreen = true;
        }

        private void ExitFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(745, 450);
            isFullScreen = false;
        }
    }
}
