using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 
using System.IO; 
using System.Windows.Forms; 
using WinTimer = System.Windows.Forms.Timer; 

namespace FP
{
    public class GameForm : MainForm
    {
        private Ruangan currentRoom;
        private Size originalSize;
        private string selectedItemName;
        private int roomIndex = 0;
        private Ruangan[] rooms;
        private GameClock gameClock;
        private BackgroundMusic backgroundMusic;
        private GameLogic logic;
        private Panel popupPanel;
        private Label itemNameLabel;
        private Button reportButton;
        private bool isPopupVisible = false;
        private string reportItemName;

        //animasi report
        private WinTimer animationTimer; 
        private bool isAnimating = false;
        private int animationStep = 0;
        private const int maxAnimationSteps = 10; // Jumlah langkah animasi
        private Size finalSize; // Ukuran akhir tombol
        private Point finalLocation; // Lokasi akhir tombol


        public GameForm()
        {
            InitializeRooms();
            LoadGame();
            EnterFullScreen();
            reportButton = new Button();
            reportButton.Text = "Report";
            reportButton.Visible = false;

            reportButton.BackColor = Color.Black;
            reportButton.ForeColor = Color.White;
            reportButton.FlatStyle = FlatStyle.Flat;
            reportButton.FlatAppearance.BorderSize = 0;
            reportButton.AutoSize = false;
            reportButton.Size = new Size(100, 40);
            reportButton.Padding = new Padding(10, 5, 10, 5);
            reportButton.Font = new Font("Arial", 12, FontStyle.Bold);

            reportButton.Click += ReportButton_Click;
            reportButton.MouseEnter += ReportButton_MouseEnter;
            reportButton.MouseLeave += ReportButton_MouseLeave;

            this.Controls.Add(reportButton);

            animationTimer = new WinTimer();
            animationTimer.Interval = 15; 
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void ReportButton_MouseEnter(object sender, EventArgs e)
        {
            reportButton.BackColor = Color.Gray;
        }

        private void ReportButton_MouseLeave(object sender, EventArgs e)
        {
            reportButton.BackColor = Color.Black;
        }

        private void LoadGame()
        {
            this.Text = currentRoom.Name;
            LoadBackgroundImage(currentRoom.BackgroundImagePath);
            originalSize = this.BackgroundImage.Size;

            this.DoubleBuffered = true;
            this.MouseClick += GameForm_MouseClick;
            this.MouseMove += GameForm_MouseMove;

            gameClock = new GameClock();
            gameClock.TimeUpdated += (s, e) => Invalidate();

            logic = new GameLogic(rooms, gameClock);

            string musicPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "../../../../music/music1.mp3"));
            if (File.Exists(musicPath))
            {
                backgroundMusic = new BackgroundMusic(musicPath);
                backgroundMusic.Play();
            }
            else
            {
                MessageBox.Show($"Music file not found: {musicPath}");
                Application.Exit();
            }
        }

        private void InitializeRooms()
        {
            rooms = new Ruangan[]
            {
                new Ruangan("Gaming Room", "../../../../images/Room 1/bg.png"),
                new Ruangan("Bathroom", "../../../../images/Room 2/bg.png"),
                new Ruangan("Bedroom", "../../../../images/Room 3/bg.png")
            };

            // Ruangan 1
            rooms[0].AddItem(new Benda("Table", new Point(150, 600), "../../../../images/Room 1/meja_pc_awal.png", new Size(420, 220)));
            rooms[0].AddItem(new Benda("Tabletop Speaker", new Point(450, 540), "../../../../images/Room 1/speaker.png", new Size(45, 60)));
            rooms[0].AddItem(new Benda("PC", new Point(190, 375), "../../../../images/Room 1/pc_awal.png", new Size(250, 225)));
            rooms[0].AddItem(new Benda("Lamp", new Point(450, 465), "../../../../images/Room 1/lampu_awal.png", new Size(100, 140))); //point lampu akhir = 358,465; size = 147,140
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
            rooms[1].AddItem(new Benda("Bathtub", new Point(220, 215), "../../../../images/Room 2/bathup.png", new Size(600, 630)));
            rooms[1].AddItem(new Benda("Trash can", new Point(27, 625), "../../../../images/Room 2/tempat_sampah_awal.png", new Size(200, 220))); //172,193 = size tempat sampah akhir, position = 30, 625
            rooms[1].AddItem(new Benda("Shelf", new Point(15, 175), "../../../../images/Room 2/rak_kiri_awal.png", new Size(200, 420)));
            rooms[1].AddItem(new Benda("Towel", new Point(860, 412), "../../../../images/Room 2/serbet.png", new Size(98, 253)));
            rooms[1].AddItem(new Benda("Sink", new Point(1008, 420), "../../../../images/Room 2/lemari.png", new Size(313, 413)));
            rooms[1].AddItem(new Benda("Mirror", new Point(1035, 139), "../../../../images/Room 2/cermin_awal.png", new Size(263, 263)));
            rooms[1].AddItem(new Benda("Toilet", new Point(1430, 404), "../../../../images/Room 2/WC.png", new Size(224, 427)));
            rooms[1].AddItem(new Benda("Tissue", new Point(1680, 500), "../../../../images/Room 2/Tisu_awal.png", new Size(150, 200))); //130, 25 = size tisu akhir; point = 1680, 570

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

        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (originalSize == null) return;

            float scaleX = (float)this.ClientSize.Width / originalSize.Width;
            float scaleY = (float)this.ClientSize.Height / originalSize.Height;
            float scale = Math.Min(scaleX, scaleY);

            float scaledBgWidth = originalSize.Width * scale;
            float scaledBgHeight = originalSize.Height * scale;

            float offsetX = (this.ClientSize.Width - scaledBgWidth) / 2;
            float offsetY = (this.ClientSize.Height - scaledBgHeight) / 2;

            bool clickedOnItem = false;

            foreach (var item in currentRoom.Items)
            {
                float scaledX = offsetX + item.Position.X * scale;
                float scaledY = offsetY + item.Position.Y * scale;

                float scaledWidth = item.ImageSize.Width * scale;
                float scaledHeight = item.ImageSize.Height * scale;

                RectangleF itemRect = new RectangleF(scaledX, scaledY, scaledWidth, scaledHeight);

                if (itemRect.Contains(e.Location))
                {
                    if (IsPixelOpaque(item.ImagePath, e.Location, itemRect))
                    {
                        clickedOnItem = true;
                        if (e.Button == MouseButtons.Left)
                        {
                            selectedItemName = item.Name;
                            reportButton.Visible = false; 
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            selectedItemName = null;
                            reportItemName = item.Name;

                            int buttonX = e.X;
                            int buttonY = e.Y;

                            finalSize = new Size(100, 40); 

                            reportButton.Size = new Size(0, 0);

                            finalLocation = new Point(buttonX, buttonY);

                            if (finalLocation.X + finalSize.Width > this.ClientSize.Width)
                            {
                                finalLocation.X = this.ClientSize.Width - finalSize.Width - 10; // Padding 10 px
                            }

                            if (finalLocation.Y + finalSize.Height > this.ClientSize.Height)
                            {
                                finalLocation.Y = this.ClientSize.Height - finalSize.Height - 10; // Padding 10 px
                            }

                            reportButton.Location = new Point(buttonX, buttonY);

                            reportButton.Visible = true;
                            isAnimating = true;
                            animationStep = 0;
                            animationTimer.Start();
                        }

                        Invalidate();
                        break;
                    }
                }
            }

            if (!clickedOnItem)
            {
                if (!reportButton.Bounds.Contains(e.Location))
                {
                    HideReportButton();
                }
                selectedItemName = null;
                reportItemName = null; 
                Invalidate();
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    HideReportButton();
                }
            }
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(reportItemName))
            {
                bool isChanged = logic.IsItemChanged(reportItemName);

                if (isChanged)
                {
                    MessageBox.Show("Berhasil di report!", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Belum ada perubahan!", "Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                HideReportButton();
            }
            else
            {
                MessageBox.Show("Belum ada perubahan!", "Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsPixelOpaque(string imagePath, Point mousePoint, RectangleF itemRect)
        {
            if (!File.Exists(imagePath)) return false;

            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                float scaleX = bitmap.Width / itemRect.Width;
                float scaleY = bitmap.Height / itemRect.Height;

                int pixelX = (int)((mousePoint.X - itemRect.X) * scaleX);
                int pixelY = (int)((mousePoint.Y - itemRect.Y) * scaleY);

                if (pixelX >= 0 && pixelX < bitmap.Width && pixelY >= 0 && pixelY < bitmap.Height)
                {
                    Color color = bitmap.GetPixel(pixelX, pixelY);
                    return color.A != 0;
                }
            }
            return false;
        }

        private void GameForm_MouseMove(object sender, MouseEventArgs e)
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

            using (Font font = new Font("Comic Sans MS", 20, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string teksRuangan = $"{currentRoom.Name}";
                e.Graphics.DrawString(teksRuangan, font, brush, 10, 10);
            }

            gameClock.Draw(e.Graphics, this.ClientSize.Width);

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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Left)
            {
                roomIndex = keyData == Keys.Right ? (roomIndex + 1) % rooms.Length : (roomIndex - 1 + rooms.Length) % rooms.Length;
                currentRoom = rooms[roomIndex];
                ClearState();
                this.Text = currentRoom.Name;
                Invalidate();
                return true;
            }
            if (keyData == Keys.F11)
            {
                ToggleFullScreen();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (animationStep < maxAnimationSteps)
            {
                int newWidth = reportButton.Width + finalSize.Width / maxAnimationSteps;
                int newHeight = reportButton.Height + finalSize.Height / maxAnimationSteps;

                if (newWidth > finalSize.Width) newWidth = finalSize.Width;
                if (newHeight > finalSize.Height) newHeight = finalSize.Height;

                reportButton.Size = new Size(newWidth, newHeight);

                animationStep++;
            }
            else
            {
                animationTimer.Stop();
                isAnimating = false;
                reportButton.Size = finalSize; 
            }
        }

        private void HideReportButton()
        {
            if (isAnimating)
            {
                animationTimer.Stop();
                isAnimating = false;
            }
            reportButton.Visible = false;
            reportItemName = null; 
            Invalidate();
        }

        private void ClearState()
        {
            selectedItemName = null;
            reportItemName = null; 
            HideReportButton();
        }

    }
}
