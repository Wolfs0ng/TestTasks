using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerResultData
{
    public Vector2Int MazeSize;
    public int Time;
    public float Distance;
}

[CreateAssetMenu(fileName = "LeaderboardData", menuName = "SO/Leaderboard")]
public class LeaderboardData : ScriptableObject
{
    public List<PlayerResultData> PlayersResultData;
}