using System;
using System.Collections.Generic;
using Bot.Interfaces;
using Core;

namespace Bot
{
    public class RandomBot : IBot
    {
        private Random _random = new Random();

        public int GetNextMove(MoveType[] board)
        {
            var availableMoves = new List<int>();
            
            for (var i = 0; i < board.Length; i++)
            {
                if (board[i] == MoveType.None)
                    availableMoves.Add(i);
            }

            return availableMoves[_random.Next(availableMoves.Count)];
        }
    }
}