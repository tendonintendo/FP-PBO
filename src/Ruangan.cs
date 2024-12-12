using System.Collections.Generic;
using System.Drawing;

namespace FP
{
    public class Ruangan
    {
        private string _backgroundImagePath;
        private string _name;
        private List<Benda> _items;

        public string BackgroundImagePath { get { return _backgroundImagePath; } }
        public string Name { get { return _name; } }
        public List<Benda> Items { get { return _items; } }

        public Ruangan(string name, string backgroundImagePath)
        {
            _backgroundImagePath = backgroundImagePath;
            _items = new List<Benda>();
            _name = name;
        }

        public void AddItem(Benda item)
        {
            Items.Add(item);
        }
    }
}
