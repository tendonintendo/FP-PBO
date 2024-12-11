using System.Drawing;

namespace FP
{
    public class Benda
    {
        public string Name { get; set; }

        public Point Position { get; set; }

        public string ImagePath { get; set; }

        public Size ImageSize { get; set; }

        public Benda(string name, Point position, string imagePath, Size? imageSize = null)
        {
            Name = name;
            Position = position;
            ImagePath = imagePath;

            ImageSize = imageSize ?? new Size(100, 100);
        }
    }
}
