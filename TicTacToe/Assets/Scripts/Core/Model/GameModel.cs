using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Model
{
    public class GameModel : IGameModel
    {
        public MoveType[] Board { get; set; }
        public MoveType CurrentMoveType { get; set; }
        public Dictionary<int, MoveType> MoveOrder { get; set; }
        public int[] GameResult { get; set; }

        public GameModel(int boardSize)
        {
            Board = new MoveType[boardSize];
            MoveOrder = new Dictionary<int, MoveType>(boardSize);
            CurrentMoveType = MoveType.X;
            GameResult = null;
        }
    }
}