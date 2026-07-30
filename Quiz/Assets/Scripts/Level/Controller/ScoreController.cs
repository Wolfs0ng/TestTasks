using TMPro;
using UnityEngine;

namespace Scripts.Level.Controller
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        
        int _pointsPerAnswer;
        int _currentScore;

        public int CurrScore => _currentScore;
        
        void Start()
        {
            _currentScore = 0;
            UpdateScoreText();
        }
        
        public void SetData(int pointsPerAnswer)
        {
            _pointsPerAnswer = pointsPerAnswer;
        }

        public void AddScore()
        {
            _currentScore += _pointsPerAnswer;
            UpdateScoreText();
        }

        public void ResetScore()
        {
            _currentScore = 0;
            UpdateScoreText();
        }

        void UpdateScoreText()
        {
            if (_scoreText != null)
                _scoreText.text = _currentScore.ToString();
        }
    }
}