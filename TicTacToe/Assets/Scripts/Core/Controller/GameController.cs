using System;
using System.Linq;
using Core.Interfaces;
using Core.Model;
using Core.Rules;
using Core.Rules.Interfaces;
using Core.View;
using Scripts.Level.Controller;
using UnityEngine;

namespace Core.Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private int _edgeSize;
        [SerializeField] private StopwatchController _stopwatchController;

        private IGameModel _model;
        private GameView _view;

        private int _fieldSize;
        private IRule[] _rules;

        private void Awake()
        {
            _fieldSize = (int)Math.Pow(_edgeSize, 2);

            _model = new GameModel(_fieldSize);

            _rules = new IRule[]
            {
                new HorizontalRule(),
                new VerticalRule(),
                new Diagonals()
            };

            _view = GetComponent<GameView>();
            _view.Initialize(_edgeSize);
        }

        private void Start()
        {
            StartNewGame();
        }

        private void OnEnable()
        {
            _view.OnMoveMade += CheckMove;
            _view.OnResetClick += StartNewGame;
        }

        private void OnDisable()
        {
            _view.OnMoveMade -= CheckMove;
            _view.OnResetClick -= StartNewGame;
        }

        private void CheckMove(int index)
        {
            _model.Board[index] = _model.CurrentMoveType;
            _model.MoveOrder.Add(index, _model.CurrentMoveType);
            CheckRules(_model.Board);

            if (_model.GameResult != null && _model.GameResult.Length > 0)
            {
                _stopwatchController.StopTimer();
                _view.ShowWinner(_model.CurrentMoveType, _model.GameResult);
            }
            else if (_model.Board.All(s => s != 0))
            {
                _stopwatchController.StopTimer();
                _view.ShowDraw();
            }

            _model.CurrentMoveType = _model.CurrentMoveType == MoveType.O ? MoveType.X : MoveType.O;
            _view.NextPlayer(_model.CurrentMoveType);
        }

        private void CheckRules(MoveType[] getBoard)
        {
            foreach (var rule in _rules)
            {
                _model.GameResult = rule.CheckRule(_model.CurrentMoveType, getBoard, _edgeSize);

                if (_model.GameResult.Length > 0)
                    break;
            }
        }

        private void StartNewGame()
        {
            _model = new GameModel(_fieldSize);
            _view.NextPlayer(_model.CurrentMoveType);
            _view.ResetField();

            _stopwatchController.ResetTimer();
            _stopwatchController.StartTimer();
        }
    }
}