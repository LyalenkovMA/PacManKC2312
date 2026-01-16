using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacManKC2312
{
    public class Game
    {
        private int _countPoint;
        private int _timerBonus;
        private int _timyBonus;
        private int _delay;

        private List<GameObject> _gameObjects;
        private Level _level;
        private Player _player;

        public Game()
        {
            _gameObjects = new List<GameObject>();
            _level = new Level("map.txt");
            _player = new Player(new Vector(1,1));

            _gameObjects.Add(_player);
            _gameObjects.Add(new Enemie(_level, _player));
            _gameObjects.Add(new Enemie(_level, _player));
            _gameObjects.Add(new Enemie(_level, _player));
            _gameObjects.Add(new Enemie(_level, _player));
            _gameObjects.Add(new Bonus(_level, _player));
            _gameObjects.Add(new Bonus(_level, _player));
            _gameObjects.Add(new Bonus(_level, _player));

            _countPoint = 0;
            _delay = 200;
            _timerBonus = 0;
            _timyBonus = 40;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (IsStart())
                {
                    _player.Control();
                }
            });

            Task.Run(() =>
            {
                while (IsStart())
                {
                    Update();
                    Thread.Sleep(_delay);
                }
            });

            while (IsStart())
            {
                Draw();
                Thread.Sleep(_delay);
            }
        }

        private bool IsStart()
        {
            return _player.IsLive && _countPoint != _level.CountFood;
        }

        private void Update()
        {
            foreach (GameObject gameObject in _gameObjects)
                gameObject.Update(_level);

            if (_level.IsFood(_player))
                _countPoint++;

            if (_player.IsBonus)
            {
                if(_timerBonus == _timyBonus)
                {
                    _player.DiactivateBonus();
                    _timerBonus = 0;
                }
                else
                {
                    _timerBonus++;
                }
            }
        }

        private void Draw()
        {
            string lineSymbolPlayer = "";
            string info = "";

            Console.Clear();
            _level.Draw();
            PrintInformLine();

            foreach (GameObject gameObject in _gameObjects)
                gameObject.Draw();
        }

        private void PrintInformLine()
        {
            int width = _level.GetMap().GetLength(0);
            string livesPlayer = "";
            string infoBals = $"Балы {_countPoint}";
            string text = "";
            char emptySpace = ' ';

            for (int i = 0; i < _player.CountLive; i++)
                livesPlayer += _player.Symbol;

            text += '|' + livesPlayer;

            for (int i = 0; i < (width - (livesPlayer.Length + infoBals.Length))-2; i++)
                text += emptySpace;
            text += infoBals + '|';

            Console.WriteLine(text);
        }
    }
}
