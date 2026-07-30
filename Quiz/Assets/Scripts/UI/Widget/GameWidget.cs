using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Widget
{
    public class GameWidget : MonoBehaviour
    {
        [SerializeField] Button _pauseButton;
        [SerializeField] Button _boosterButton;

        public Action OnPauseClick;
        public Action OnBoosterClick;

        void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseButtonHandler);
            _boosterButton.onClick.AddListener(BoosterButtonHandler);
        }

        void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(PauseButtonHandler);
            _boosterButton.onClick.RemoveListener(BoosterButtonHandler);
        }

        void PauseButtonHandler()
        {
            OnPauseClick?.Invoke();
        }

        void BoosterButtonHandler()
        {
            OnBoosterClick?.Invoke();
        }
    }
}

