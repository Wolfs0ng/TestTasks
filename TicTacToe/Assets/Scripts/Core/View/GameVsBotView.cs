using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class GameVsBotView : BaseGameView
    {
        [SerializeField] private TextMeshProUGUI _gameInformation;
        [SerializeField] private Button _undoButton;

        public event Action OnUndoButtonClicked;
        
        protected override void Enable()
        {
            base.Enable();
            _undoButton.onClick.AddListener(UndoButtonClicked);
        }

        protected override void Disable()
        {
            base.Disable();
            _undoButton.onClick.RemoveAllListeners();
        }
        
        public override void ShowWinner(MoveType winner, int[] winLine)
        {
            base.ShowWinner(winner, winLine);
            _gameInformation.text = $"Winner: {winner}";
        }

        public override void ShowDraw()
        {
            base.ShowDraw();
            _gameInformation.text = "Draw";
        }

        public override void NextPlayer(MoveType currentMove)
        {
            base.NextPlayer(currentMove);
            _gameInformation.text = $"Turn: {currentMove}";
        }
        
        public void UndoStep(KeyValuePair<int, MoveType> lastMove)
        {
            _cells[lastMove.Key].SetMark(MoveType.None);
        }

        public void SetUndoButtonState(bool isActive)
        {
            _undoButton.interactable = isActive;
        }

        private void UndoButtonClicked()
        {
            OnUndoButtonClicked?.Invoke();
        }
    }
}