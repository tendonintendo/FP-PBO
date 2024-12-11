using System.Collections.Generic;
using System.Drawing;

namespace FP
{
    public class Ruangan
    {

        public string BackgroundImagePath { get; set; }
        public string Name { get; set; }
        public List<Benda> Items { get; set; }

        public Ruangan(string name, string backgroundImagePath)
        {
            BackgroundImagePath = backgroundImagePath;
            Items = new List<Benda>();
            Name = name;
        }

        public void AddItem(Benda item)
        {
            Items.Add(item);
        }
    }
}
