using System.Collections.Generic;
using System.Linq;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Injector.Managers
{
    public class LeaderBoardManager : MonoBehaviour
    {
        [SerializeField] LeaderBoardData _leaderBoardData;
        
        public int RoundResult { get; set; }

        public void AddNewResult(string name)
        {
            var newResult = new PlayerResultData
            {
                Name = name, Score = RoundResult
            };

            var samePlayer = _leaderBoardData.PlayersResultData.FirstOrDefault(d => d.Name == newResult.Name);
            
            if(samePlayer == null)
                _leaderBoardData.PlayersResultData.Add(newResult);
            else if(newResult.Score > samePlayer.Score)
                _leaderBoardData.PlayersResultData.Add(newResult);
        }

        public List<PlayerResultData> GetResultsData()
        {
            return _leaderBoardData.PlayersResultData.OrderByDescending(s => s.Score).ToList();
        }
    }
}