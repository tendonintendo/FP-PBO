using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FP
{
    public class GameLogic
    {
        public static int countChanges = 0;
        private Ruangan[] _rooms;
        private List<Benda> _changeables;
        private Random _rnd;
        private CancellationTokenSource _cts;

        public GameLogic(Ruangan[] rooms)
        {
            _rooms = rooms;
            _changeables = new List<Benda>();
            _rnd = new Random();
            _cts = new CancellationTokenSource();

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
            StartRandomTransform();
        }

        public void Transform()
        {
            if (_changeables.Count == 0) return;

            int num = _rnd.Next(0, _changeables.Count);
            _changeables[num].ImagePath = _changeables[num].ImagePath.Replace("awal", "akhir");
            MessageBox.Show($"Transformed: {_changeables[num].Name}");
            countChanges++;
        }

        public void ResetLogic()
        {
            if (_cts != null)
            {
                _cts.Dispose();
            }
            _cts = new CancellationTokenSource();
            countChanges = 0;
            StartRandomTransform();
        }

        private async void StartRandomTransform()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                int delay = _rnd.Next(10000, 20000); //ini transform tiap 10-20 detik, masih testing
                await Task.Delay(delay, _cts.Token);
                Transform();
                if (countChanges == 3) { StopTransforming(); }
            }
        }

        public void StopTransforming()
        {
            _cts.Cancel();
        }
    }
}
