using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
    public interface IGameFactory
    {
        Game CreateGame(Map gameMap, Ai ai);
    }

    class GameFactory : IGameFactory
    {
        public Game CreateGame(Map gameMap, Ai ai)
        {
            return new Game(gameMap, ai);
        }
    }
}
