using Scripts.Data;
using Scripts.Injector;
using Scripts.Injector.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Widget
{
    public class MenuWidget : MonoBehaviour
    {
        [SerializeField] GameObject _menuButtonsHolder;
        [SerializeField] Button _newGameButton;
        [SerializeField] Button _leaderBoardButton;
        [SerializeField] Button _backgroundButton;
        [SerializeField] GameObject _difficultyButtonsHolder;
        [SerializeField] Button _easyButton;
        [SerializeField] Button _mediumButton;
        [SerializeField] Button _hardButton;

        PopupManager _popupManager;
        SceneManager _sceneManager;
        DifficultyManager _difficultyManager;

        void Start()
        {
            _popupManager = ServiceLocator.Get<PopupManager>();
            _sceneManager = ServiceLocator.Get<SceneManager>();
            _difficultyManager = ServiceLocator.Get<DifficultyManager>();
        }

        void OnEnable()
        {
            _newGameButton.onClick.AddListener(NewGameButtonHandler);
            _leaderBoardButton.onClick.AddListener(LeaderBoardButtonHandler);
            _backgroundButton.onClick.AddListener(BackgroundButtonHandler);
            _easyButton.onClick.AddListener(EasyButtonHandler);
            _mediumButton.onClick.AddListener(MediumButtonHandler);
            _hardButton.onClick.AddListener(HardButtonHandler);
        }

        void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(NewGameButtonHandler);
            _leaderBoardButton.onClick.RemoveListener(LeaderBoardButtonHandler);
            _backgroundButton.onClick.RemoveListener(BackgroundButtonHandler);
            _easyButton.onClick.AddListener(EasyButtonHandler);
            _mediumButton.onClick.AddListener(MediumButtonHandler);
            _hardButton.onClick.AddListener(HardButtonHandler);
        }

        void NewGameButtonHandler()
        {
            _menuButtonsHolder.SetActive(false);
            _difficultyButtonsHolder.SetActive(true);
        }

        void LeaderBoardButtonHandler()
        {
            _popupManager.ShowPopup(PopUp.LeaderBoard);
        }

        void BackgroundButtonHandler()
        {
            Debug.LogError("show background settings");
        }
        
        void EasyButtonHandler()
        {
            SelectDifficulty(Difficulty.Easy);
        }
        
        void MediumButtonHandler()
        {
            SelectDifficulty(Difficulty.Medium);
        }
        
        void HardButtonHandler()
        {
            SelectDifficulty(Difficulty.Hard);
        }
        
        void SelectDifficulty(Difficulty difficulty)
        {
            _difficultyManager.SetDifficultyType(difficulty);
            _sceneManager.LoadGameScene();
        }
    }
}
