using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class PlayerResultData
    {
        public string Name;
        public int Score;
    }
    
    [CreateAssetMenu(fileName = "LeaderBoardData", menuName = "LeaderBoard")]
    public class LeaderBoardData : ScriptableObject
    {
        public List<PlayerResultData> PlayersResultData;
    }
}