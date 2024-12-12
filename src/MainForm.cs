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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.WindowState = FormWindowState.Normal;
            EnterFullScreen();

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
            this.MouseMove += MainForm_MouseMove;
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
            rooms[0].AddItem(new Benda("Kursi gimang", new Point(0, 420), "../../../../images/Room 1/kursi_gaming.png", new Size(500, 470)));
            rooms[0].AddItem(new Benda("Sofa Kiri", new Point(850, 619), "../../../../images/Room 1/sofa_kiri.png", new Size(231, 198)));
            rooms[0].AddItem(new Benda("Speaker", new Point(1630, 477), "../../../../images/Room 1/speaker.png", new Size(230, 340)));
            rooms[0].AddItem(new Benda("Sofa kanan", new Point(1450, 619), "../../../../images/Room 1/sofa_kanan.png", new Size(231, 198)));
            rooms[0].AddItem(new Benda("Meja TV", new Point(1057, 598), "../../../../images/Room 1/meja_TV.png", new Size(408, 48)));
            rooms[0].AddItem(new Benda("TV awal", new Point(1010, 295), "../../../../images/Room 1/TV_awal.png", new Size(494, 304))); 
            rooms[0].AddItem(new Benda("Stick awal", new Point(1360, 559), "../../../../images/Room 1/stick_awal.png", new Size(84, 49)));
            rooms[0].AddItem(new Benda("Headset kanan", new Point(1260, 620), "../../../../images/Room 1/headset.png", new Size(80, 90)));
            rooms[0].AddItem(new Benda("Headset kiri", new Point(1160, 620), "../../../../images/Room 1/headset.png", new Size(80, 90)));

            // Ruangan 2
            rooms[1].AddItem(new Benda("Bathtub shelf", new Point(583, 315), "../../../../images/Room 2/rak_bathup_awal.png", new Size(220, 153)));
            rooms[1].AddItem(new Benda ("Bathtub", new Point(220,215), "../../../../images/Room 2/bathup.png", new Size(600,630)));
            rooms[1].AddItem(new Benda("Trash can", new Point(27, 625), "../../../../images/Room 2/tempat_sampah_awal.png", new Size(200, 220)));
            rooms[1].AddItem(new Benda("Shelf", new Point(15, 175), "../../../../images/Room 2/rak_kiri_awal.png", new Size(200, 420)));
            rooms[1].AddItem(new Benda("Towel", new Point(860, 412), "../../../../images/Room 2/serbet.png", new Size(98, 253)));
            rooms[1].AddItem(new Benda("Sink", new Point(1008, 420), "../../../../images/Room 2/lemari.png", new Size(313, 413)));
            rooms[1].AddItem(new Benda("Mirror", new Point(1035, 139), "../../../../images/Room 2/cermin_awal.png", new Size(263, 263)));
            rooms[1].AddItem(new Benda("Toilet", new Point(1430, 404), "../../../../images/Room 2/WC.png", new Size(224, 427)));
            rooms[1].AddItem(new Benda("Tissue", new Point(1680, 500), "../../../../images/Room 2/Tisu.png", new Size(190, 200)));

            // Ruangan 3
            rooms[2].AddItem(new Benda("kasur", new Point(335, 485), "../../../../images/Room 3/kasur.png", new Size(696, 348)));
            rooms[2].AddItem(new Benda("Rak kasur", new Point(65, 503), "../../../../images/Room 3/rakkasur_awal.png", new Size(240, 340)));
            rooms[2].AddItem(new Benda("Rak atas", new Point(45, 100), "../../../../images/Room 3/rakatas_awal.png", new Size(290, 320)));
            rooms[2].AddItem(new Benda("Lukisan", new Point(370, 220), "../../../../images/Room 3/lukisan.png", new Size(240, 170)));
            rooms[2].AddItem(new Benda("Lampu atas", new Point(885, -34), "../../../../images/Room 3/lampuatas.png", new Size(140, 350)));
            rooms[2].AddItem(new Benda("Perapian", new Point(1055, 403), "../../../../images/Room 3/perapian.png", new Size(420, 430)));
            rooms[2].AddItem(new Benda("Banteng", new Point(1100, 200), "../../../../images/Room 3/banteng.png", new Size(340, 180)));
            rooms[2].AddItem(new Benda("Lemari", new Point(1470, 184), "../../../../images/Room 3/lemari.png", new Size(390, 650)));

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

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            bool cursorOverItem = false;

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
                    cursorOverItem = true;
                    break;
                }
            }

            if (cursorOverItem)
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
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
                roomIndex = (roomIndex + 1) % rooms.Length;
                currentRoom = rooms[roomIndex];
                this.Text = currentRoom.Name; // Update judul jendela dengan nama ruangan baru
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                roomIndex = (roomIndex - 1 + rooms.Length) % rooms.Length;
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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(745, 450);
            isFullScreen = false;
        }
    }
}
