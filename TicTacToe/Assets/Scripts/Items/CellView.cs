using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _mark;
        [SerializeField] private Sprite _cross;
        [SerializeField] private Sprite _circle;
        [SerializeField] private Button _button;

        private int _index;

        public Action<int> OnCellChoose;

        private void OnEnable()
        {
            _button.onClick.AddListener(CellChoose);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void CellChoose()
        {
            OnCellChoose?.Invoke(_index);
            _button.interactable = false;
        }

        public void Reset(int index)
        {
            _index = index;
            _button.interactable = true;
            _mark.color = Color.white;
            _mark.enabled = false;
        }

        public void SetMark(MoveType moveType)
        {
            if (moveType == MoveType.None)
            {
                _mark.enabled = false;
                _button.interactable = true;
            }
            else
            {
                _mark.enabled = true;
                _button.interactable = false;
                _mark.sprite = moveType == MoveType.O ? _circle : _cross;
            }
        }

        public void Highlight()
        {
            _mark.color = Color.green;
        }

        public void Disable()
        {
            _button.interactable = false;
        }
    }
}