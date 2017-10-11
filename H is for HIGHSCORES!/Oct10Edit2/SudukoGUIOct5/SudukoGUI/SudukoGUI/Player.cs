using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudukoGUI
{
    class Player
    {
        public string Name { get; private set; }

        public double Score = 0;

        public char Token { get; private set; }

        public override string ToString()
        {
            return $"{Name}, Token: {Token}, Score: {Score}";
        }

        public Player(string name, char token)
        {
            Name = name;
            Token = token;
        }

        public Player(string name, char token, double score)
        {
            Name = name;
            Token = token;
            Score = score;
        }
    }
}
