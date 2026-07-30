using Core;

namespace Bot.Interfaces
{
    public interface IBot
    {
        int GetNextMove(MoveType[] board);
    }
}