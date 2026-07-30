using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class BaseGameView : MonoBehaviour
    {
        [SerializeField] protected GridLayoutGroup _grid;
        [SerializeField] protected CellView _cellPrefab;
        [SerializeField] protected List<CellView> _cells;
        [SerializeField] protected Button _restartButton;
        
        protected MoveType _currentMove;

        public event Action<int> OnMoveMade;
        public event Action OnResetClick;

        public void Initialize(int fieldSize)
        {
            _grid.constraintCount = fieldSize;
            CheckCellsCount();
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }
        
        protected virtual void Enable()
        {
            foreach (var cell in _cells)
                cell.OnCellChoose += MoveMade;

            _restartButton.onClick.AddListener(ResetClicked);
        }
        
        protected virtual void Disable()
        {
            foreach (var cell in _cells)
                cell.OnCellChoose -= MoveMade;

            _restartButton.onClick.RemoveAllListeners();
        }

        public void MoveMade(int index)
        {
            _cells[index].SetMark(_currentMove);
            OnMoveMade?.Invoke(index);
        }

        public virtual void ShowWinner(MoveType winner, int[] winLine)
        {
            _restartButton.gameObject.SetActive(true);

            for (var i = 0; i < _cells.Count; i++)
            {
                if (winLine.Any(l => l == i))
                    _cells[i].Highlight();
                else
                    _cells[i].Disable();
            }
        }

        public virtual void ShowDraw()
        {
            _restartButton.gameObject.SetActive(true);

            foreach (var cell in _cells)
                cell.Disable();
        }

        public virtual void NextPlayer(MoveType currentMove)
        {
            _currentMove = currentMove;
        }

        public void ResetField()
        {
            for (var i = 0; i < _cells.Count; i++)
                _cells[i].Reset(i);
        }
        
        private void ResetClicked()
        {
            OnResetClick?.Invoke();
            _restartButton.gameObject.SetActive(false);
        }
        
        private void CheckCellsCount()
        {
            var targetCellsCount = (int)Math.Pow(_grid.constraintCount, 2);

            if (_cells.Count < targetCellsCount)
            {
                var currentCellsCount = _cells.Count;

                for (var i = 0; i < targetCellsCount - currentCellsCount; i++)
                {
                    var newCell = Instantiate(_cellPrefab, _grid.transform);
                    _cells.Add(newCell);
                }
            }

            if (_cells.Count > targetCellsCount)
            {
                for (var i = targetCellsCount; i < _cells.Count; i++)
                    _cells[i].gameObject.SetActive(false);
            }

            ResetField();
        }
    }
}