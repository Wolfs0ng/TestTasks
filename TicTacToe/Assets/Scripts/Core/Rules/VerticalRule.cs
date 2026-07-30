using System.Collections.Generic;
using Core.Rules.Interfaces;

namespace Core.Rules
{
    public class VerticalRule : IRule
    {
        readonly List<int> _filledLine = new List<int>();

        public int[] CheckRule(MoveType currentMove, MoveType[] board, int edgeLength)
        {
            _filledLine.Clear();

            for (var i = 0; i < board.Length; i += edgeLength)
            {
                for (var j = i; j < i + edgeLength; j++)
                {
                    if (board[j] == currentMove)
                    {
                        _filledLine.Add(j);
                        continue;
                    }

                    _filledLine.Clear();
                    break;
                }

                if (_filledLine.Count == edgeLength)
                    return _filledLine.ToArray();
            }

            return _filledLine.ToArray();
        }
    }
}