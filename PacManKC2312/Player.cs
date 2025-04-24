using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManKC2312
{
    public class Player: GameObject
    {
        private int _direction;
       
        public Player(Vector position) : base(position, '@', ConsoleColor.Yellow)
        {
            _direction = KeyMoveRigth;
            CountLive = 3;
            IsBonus = false;
        }

        public bool IsBonus { get; private set; }

        public int CountLive { get; private set; }

        public bool IsLive => CountLive > 0;

        public override void Update(Level level)
        {
            switch (_direction)
            {
                case KeyMoveUp:
                    SetPosition(new Vector(MoveStop, MoveBack), level);
                    break;
                case KeyMoveDown:
                    SetPosition(new Vector(MoveStop, MoveForward), level);
                    break;
                case KeyMoveLeft:
                    SetPosition(new Vector(MoveBack, MoveStop), level);
                    break;
                case KeyMoveRigth:
                    SetPosition(new Vector(MoveForward, MoveStop), level);
                    break;
            }
        }

        public void Control()
        {
            const ConsoleKey KeyUp = ConsoleKey.UpArrow;
            const ConsoleKey KeyDown = ConsoleKey.DownArrow;
            const ConsoleKey KeyRight = ConsoleKey.RightArrow;
            const ConsoleKey KeyLeft = ConsoleKey.LeftArrow;

            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case KeyUp:
                    _direction = KeyMoveUp;
                    break;
                case KeyDown:
                    _direction = KeyMoveDown;
                    break;
                case KeyLeft:
                    _direction = KeyMoveLeft;
                    break;
                case KeyRight:
                    _direction = KeyMoveRigth;
                    break;
            }
        }

        public void TakeDamage()
        {
            CountLive--;
            GetStartPosition();
        }

        public void ActivateBonus()
        {
            IsBonus = true;
        }

        public void DiactivateBonus()
        {
            IsBonus = false;
        }
    }
}
