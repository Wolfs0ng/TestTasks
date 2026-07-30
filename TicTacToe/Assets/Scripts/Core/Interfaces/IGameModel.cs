using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IGameModel 
    {
        MoveType[] Board { get; set; }
        MoveType CurrentMoveType { get; set; }
        Dictionary<int, MoveType> MoveOrder { get; set; }
        int[] GameResult { get; set; }
    }
}
