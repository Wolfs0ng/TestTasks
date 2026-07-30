using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private LeaderboardData leaderboardData;

    public void AddNewResult(Vector2Int mazeSize, int timer, float distance)
    {
        leaderboardData.PlayersResultData.Add(new PlayerResultData
            { MazeSize = mazeSize, Time = timer, Distance = distance });
    }
}
