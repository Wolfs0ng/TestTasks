using System.Collections.Generic;
using System.Linq;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Injector.Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField] List<DifficultyData> _difficultyData;

        Difficulty _currentDifficulty;

        public void SetDifficultyType(Difficulty type)
        {
            _currentDifficulty = type;
        }
        
        public DifficultyData GetCurrentData()
        {
            var data = _difficultyData.FirstOrDefault(d => d.DifficultyType == _currentDifficulty);

            if (data == null)
            {
                Debug.LogError("Difficulty type not found!");
                return null;
            }

            return data;
        }
    }
}
