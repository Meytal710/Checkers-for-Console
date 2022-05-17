using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_EX02
{
    class Player
    {
        private readonly string r_PlayerName;
        private readonly char r_PlayerSign;
        private readonly char r_PlayerKing;
        private int m_PlayerScore;
        
        public Player(string i_PlayerName, char i_PlayerSign, char i_PlayerKing, int i_PlayerScore)
        {
            this.r_PlayerName = i_PlayerName;
            this.r_PlayerSign = i_PlayerSign;
            this.r_PlayerKing = i_PlayerKing;
            this.m_PlayerScore = 0;
        }

        public string Name
        {
            get { return r_PlayerName; }
        }

        public char Sign
        {
            get { return r_PlayerSign; }
        }

        public char King
        {
            get { return r_PlayerKing; }
        }

        internal int Score
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }
    }
}
