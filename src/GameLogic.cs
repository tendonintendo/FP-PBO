using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FP
{
    public class GameLogic
    {
        public static int countChanges = 0;
        private Ruangan[] _rooms;
        private List<Benda> _changeables;
        private Random _rnd;
        private CancellationTokenSource _cts;
        private GameClock _gameClock;
        private HashSet<int> _history;

        public GameLogic(Ruangan[] rooms, GameClock gameClock)
        {
            _rooms = rooms;
            _changeables = new List<Benda>();
            _rnd = new Random();
            _cts = new CancellationTokenSource();
            _gameClock = gameClock;

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
            _history = new HashSet<int>();
            StartRandomTransform();
        }

        public void Transform()
        {
            if (_changeables.Count == 0) return;

            int num = _rnd.Next(0, _changeables.Count);
            while (_history.Contains(num)) { num = _rnd.Next(0, _changeables.Count); }
            _changeables[num].ImagePath = _changeables[num].ImagePath.Replace("awal", "akhir");
            if (_changeables[num].Name == "Tissue")
            {
                _changeables[num].ImageSize = new Size(130, 25);
                _changeables[num].Position = new Point(1680, 570);
            } else if(_changeables[num].Name == "Lamp")
            {
                _changeables[num].ImageSize = new Size(147, 140);
                _changeables[num].Position = new Point(358, 465);
            } else if(_changeables[num].Name == "Trash can")
            {
                _changeables[num].ImageSize = new Size(172, 193);
                _changeables[num].Position = new Point(30, 625);
            }
            MessageBox.Show($"Transformed: {_changeables[num].Name}");
            _history.Add(num);
            countChanges++;
        }

        public void ResetLogic()
        {
            if (_cts != null)
            {
                _cts.Dispose();
            }
            _cts = new CancellationTokenSource();
            _history.Clear();
            countChanges = 0;
            StartRandomTransform();
        }

        private async void StartRandomTransform()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                int delayMinutes = _rnd.Next(20, 31);

                TimeSpan targetTime = _gameClock.GameTime.Add(new TimeSpan(0, delayMinutes, 0));

                while (_gameClock.GameTime < targetTime)
                {
                    await Task.Delay(100);
                }

                Transform();

                if (countChanges == 3)
                {
                    await StopTransforming();
                }
            }
        }

        public bool IsItemChanged(string itemName)
        {
            for (int i = 0; i < _changeables.Count; i++)
            {
                if (_changeables[i].Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return _history.Contains(i);
                }
            }
            return false;
        }


        public void LoseMessage()
        {
            MessageBox.Show("You Lose!");
        }


        public async Task StopTransforming()
        {
            _cts.Cancel();

            TimeSpan loseClock = _gameClock.GameTime.Add(new TimeSpan(0, 30, 0)); // set timer 30 menit
            while (_gameClock.GameTime < loseClock)
            {
                await Task.Delay(100);
            }

            LoseMessage();
        }

    }
}
