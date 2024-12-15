using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace FP
{
    public class Logic
    {
        public static int countChanges = 0;
        private List<Ruangan> _rooms;
        private List<Benda> _changeables;
        public Logic(List<Ruangan> rooms) 
        {
            _rooms = rooms;
            _changeables = new List<Benda>();
            foreach (Ruangan room in _rooms)
            {
                foreach (var item in room.Items)
                {
                    if (item.ImagePath.Contains("_awal"))
                    {
                        _changeables.Add(item);
                    }
                }
            }
        }

        public void Transform()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, _changeables.Count);
            _changeables[num].ImagePath = _changeables[num].ImagePath.Replace("awal", "akhir");
            countChanges++;
        }
    }
}
