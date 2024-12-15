using Accessibility;
using System.Drawing;

namespace FP
{
    public class Benda
    {
        private string _name, _imagePath;
        private Point _position;
        private Size _imageSize;
        public string Name { get { return _name; } }

        public Point Position { get { return _position; } set { _position = (value.X > 0 && value.Y > 0) ? value : _position; } }

        public string ImagePath { get { return _imagePath; } set { _imagePath = value ?? _imagePath; } }

        public Size ImageSize { get { return _imageSize; } set { _imageSize = (value.Width > 0 && value.Height > 0) ? value : _imageSize; }}

        public Benda(string name, Point position, string imagePath, Size? imageSize = null)
        {
            _name = name;
            _position = position;
            _imagePath = imagePath;

            _imageSize = imageSize ?? new Size(100, 100);
        }
    }
}
