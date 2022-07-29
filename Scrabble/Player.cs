using System.Collections.Generic;

namespace Scrabble
{
    public class Player
    {
        public List<char> Letters;
        public string _name;
        public int point;

        public Player(string name)
        {
            _name = name;
            Letters = new List<char>();
            point = 0;
        }
    }
}