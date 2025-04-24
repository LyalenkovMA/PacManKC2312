using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManKC2312
{
    public class Enemie : GameObject
    {
        private char[,] _map;
        private Level _level;
        private Player _player;
        private int _direction;
        private int _oldDirection;

        private Vector _moveUp;
        private Vector _moveDown;
        private Vector _moveLeft;
        private Vector _moveRigth;

        public Enemie(Level level, Player player) : base(level.PositionEnemis, 'S', ConsoleColor.Red)
        {
            _player = player;
            _level = level;
            _map = level.GetMap();
            _direction = KeyMoveUp;
            _oldDirection = KeyMoveDown;
            _moveUp = new Vector(MoveStop, MoveBack);
            _moveDown = new Vector(MoveStop, MoveForward);
            _moveLeft = new Vector(MoveBack, MoveStop);
            _moveRigth = new Vector(MoveForward, MoveStop);
        }

        public override void Update(Level level)
        {
            if(Position == _player.Position)
            {
                if (_player.IsBonus)
                    GetStartPosition();
                else
                    _player.TakeDamage();
            }

            switch (_direction)
            {
                case KeyMoveUp:
                    SetPosition(_moveUp, _level);
                    break;
                case KeyMoveDown:
                    SetPosition(_moveDown, _level);
                    break;
                case KeyMoveLeft:
                    SetPosition(_moveLeft, _level);
                    break;
                case KeyMoveRigth:
                    SetPosition(_moveRigth, _level);
                    break;
            }

            UpdateOldDirection();
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            int countFreePaths = GetCountFreePaths();

            if (countFreePaths == 2)
            {
                switch (_oldDirection)
                {
                    case KeyMoveDown:
                        GetMoveDirection(_moveUp, _moveRigth, KeyMoveLeft, KeyMoveRigth, KeyMoveUp);
                        break;
                    case KeyMoveUp:
                        GetMoveDirection(_moveDown, _moveRigth, KeyMoveLeft, KeyMoveRigth, KeyMoveDown);
                        break;
                    case KeyMoveLeft:
                        GetMoveDirection(_moveRigth, _moveUp, KeyMoveDown, KeyMoveUp, KeyMoveRigth);
                        break;
                    case KeyMoveRigth:
                        GetMoveDirection(_moveLeft, _moveUp, KeyMoveDown, KeyMoveUp, KeyMoveLeft);
                        break;
                }
            }
            else
            {
                int direction = _oldDirection;

                while (direction == _oldDirection)
                    direction = GameSeting.GetRandomNumber(4);

                _direction = direction;
            }
        }

        private int GetCountFreePaths()
        {
            int countFreePaths = 4;

            CountingPaths(_level.IsWall(new Vector(Position.X, Position.Y - 1)), ref countFreePaths);
            CountingPaths(_level.IsWall(new Vector(Position.X, Position.Y + 1)), ref countFreePaths);
            CountingPaths(_level.IsWall(new Vector(Position.X - 1, Position.Y)), ref countFreePaths);
            CountingPaths(_level.IsWall(new Vector(Position.X + 1, Position.Y)), ref countFreePaths);
            return countFreePaths;
        }

        private void GetMoveDirection(Vector vectorOne, Vector vectorTwo,int keyOne, int keyTwo,int keyFre)
        {
            if (_level.IsWall(Position + vectorOne))
                if (_level.IsWall(Position + vectorTwo))
                    _direction = keyOne;
                else
                    _direction = keyTwo;
            else
                _direction = keyFre;
        }

        private void UpdateOldDirection()
        {

            switch (_direction)
            {
                case KeyMoveUp:
                    _oldDirection = KeyMoveDown;
                    break;
                case KeyMoveDown:
                    _oldDirection = KeyMoveUp;
                    break;
                case KeyMoveLeft:
                    _oldDirection = KeyMoveRigth;
                    break;
                case KeyMoveRigth:
                    _oldDirection = KeyMoveLeft;
                    break;
            }
            
            int countFreePaths = GetCountFreePaths();

            if (countFreePaths == 1)
            {
                int direction = _direction;
                _direction = _oldDirection;
                _oldDirection = direction;
            }
        }

        private void CountingPaths(bool isWall, ref int countFreePaths)
        {
            if (isWall)
                countFreePaths--;
        }
    }
}
