using Scripts.Data;
using Scripts.Injector;
using Scripts.Injector.Managers;
using Scripts.UI.Widget;
using UnityEngine;

namespace Scripts.Level.Controller
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] TimerController _timerController;
        [SerializeField] ScoreController _scoreController;
        [SerializeField] TaskController _taskController;
        [SerializeField] GameWidget _gameWidget;
        
        bool _isPause;
        DifficultyData _difficultyData;
        BoosterController _boosterController;

        PopupManager _popupManager;
        DifficultyManager _difficultyManager;
        LeaderBoardManager _leaderBoardManager;

        void Start()
        {
            _popupManager = ServiceLocator.Get<PopupManager>();
            _difficultyManager = ServiceLocator.Get<DifficultyManager>();
            _leaderBoardManager = ServiceLocator.Get<LeaderBoardManager>();
            
            _difficultyData = _difficultyManager.GetCurrentData();
            _boosterController = new BoosterController();
            
            SetRoundSettings();
            StartNewRound();
        }

        void OnEnable()
        {
            _taskController.OnCorrectAnswer += CorrectAnswerHandler;
            _taskController.OnWrongAnswer += WrongAnswerHandler;
            _gameWidget.OnPauseClick += PauseClickHandler;
            _gameWidget.OnBoosterClick += BoosterClickHandler;
            _timerController.OnTimeIsOut += EndGame;
        }

        void OnDisable()
        {
            _taskController.OnCorrectAnswer -= CorrectAnswerHandler;
            _taskController.OnWrongAnswer -= WrongAnswerHandler;
            _gameWidget.OnPauseClick -= PauseClickHandler;
            _gameWidget.OnBoosterClick -= BoosterClickHandler;
            _timerController.OnTimeIsOut -= EndGame;
        }
        
        void SetRoundSettings()
        {
            _timerController.SetData(_difficultyData.TimerData);
            _scoreController.SetData(_difficultyData.PointsPerAnswer);
            _boosterController.SetData(_difficultyData.BoosterData);
            _taskController.SetData(_difficultyData.TaskData);
        }
      
        void StartNewRound()
        {
            _timerController.StartTimer();
            _scoreController.ResetScore();
            _taskController.GenerateTask();
        }
        
        void CorrectAnswerHandler()
        {
            _scoreController.AddScore();
            _timerController.AddTime();
            _boosterController.AddCorrectAnswer();
            _taskController.GenerateTask();
        }
        
        void WrongAnswerHandler()
        {
            EndGame();
        }

        void EndGame()
        {
            _leaderBoardManager.RoundResult = _scoreController.CurrScore;
            _timerController.StopTimer();
            _popupManager.ShowPopup(PopUp.GameOver);
        }

        void PauseClickHandler()
        {
            if (_isPause)
            {
                _taskController.ShowTask();
                _timerController.ResumeTimer();
            }
            else
            {
                _taskController.HideTask();
                _timerController.StopTimer();
            }
            
            _isPause = !_isPause;
        }

        void BoosterClickHandler()
        {
            if (_boosterController.IsBoosterAvailable && !_boosterController.IsUseForThisTask)
            {
                var result = _boosterController.UseFiftyFifty(_taskController.WrongAnswers);
                _taskController.DisableWrongAnswers(result);
            }
        }
    }
}
