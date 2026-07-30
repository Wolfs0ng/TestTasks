using System.Collections.Generic;
using System.Linq;
using Core.Rules.Interfaces;

namespace Core.Rules
{
    public class Diagonals : IRule
    {
        readonly List<int> _filledLine = new List<int>();

        public int[] CheckRule(MoveType currentMove, MoveType[] board, int edgeLength)
        {
            CheckDiagonalLeft(currentMove, board, edgeLength);

            if (!_filledLine.Any())
                CheckDiagonalRight(currentMove, board, edgeLength);

            return _filledLine.ToArray();
        }

        void CheckDiagonalLeft(MoveType currentMove, MoveType[] board, int edgeLength)
        {
            _filledLine.Clear();
            var step = edgeLength + 1;

            for (var i = 0; i < board.Length; i += step)
            {
                if (board[i] == currentMove)
                {
                    _filledLine.Add(i);
                    continue;
                }
                
                _filledLine.Clear();
                break;
            }
        }

        void CheckDiagonalRight(MoveType currentMove, MoveType[] board, int edgeLength)
        {
            _filledLine.Clear();
            var step = edgeLength - 1;

            for (var i = step; i < board.Length - 1; i += step)
            {
                if (board[i] == currentMove)
                {
                    _filledLine.Add(i);
                    continue;
                }
                
                _filledLine.Clear();
                break;
            }
        }
    }
}