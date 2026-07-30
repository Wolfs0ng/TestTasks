using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Items
{
    public class AnswerView : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] Image _colorButton;
        
        int _answer;
        public int Answer => _answer;
        
        public Action<int> OnAswerChoose;

        void OnEnable()
        {
            _button.onClick.AddListener(AnswerChooseHandler);
        }
        
        void OnDisable()
        {
            _button.onClick.RemoveListener(AnswerChooseHandler);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _colorButton.color = Color.white;
            Interactable(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void Interactable(bool isActive)
        {
            _button.interactable = isActive;
        }

        public void Highlight(bool isCorrect)
        {
            _colorButton.color = isCorrect? Color.green: Color.red;
        }
        
        public void SetData(int answer)
        {
            _answer = answer;
            _text.text = _answer.ToString();
        }
        void AnswerChooseHandler()
        {
            OnAswerChoose?.Invoke(_answer);
        }
    }
}