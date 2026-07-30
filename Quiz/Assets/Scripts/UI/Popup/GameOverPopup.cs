using Scripts.Injector;
using Scripts.Injector.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popup
{
    public class GameOverPopup : BasePopup
    {
        [SerializeField] TMP_InputField _inputFieldButton;
        [SerializeField] Button _cancelButton;
        [SerializeField] Button _submitButton;

        SceneManager _sceneManager;
        LeaderBoardManager _leaderBoardManager;

        void Awake()
        {
            _sceneManager = ServiceLocator.Get<SceneManager>();
            _leaderBoardManager = ServiceLocator.Get<LeaderBoardManager>();
        }

        void OnEnable()
        {
            _cancelButton.onClick.AddListener(CancelButtonHandler);
            _submitButton.onClick.AddListener(SubmitButtonHandler);
        }
        
        void OnDisable()
        {
            _cancelButton.onClick.RemoveListener(CancelButtonHandler);
            _submitButton.onClick.RemoveListener(SubmitButtonHandler);
        }
        
        void CancelButtonHandler()
        {
            _sceneManager.LoadMenuScene();
            Hide();
        }
        
        void SubmitButtonHandler()
        {
            _leaderBoardManager.AddNewResult(_inputFieldButton.text);
            _sceneManager.LoadMenuScene();
            Hide();
        }
    }
}