namespace Core.Rules.Interfaces
{
    public interface IRule
    {
        int[] CheckRule(MoveType currentMove, MoveType[] board, int edgeLength);
    }
}