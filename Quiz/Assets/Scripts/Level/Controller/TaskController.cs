using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Data;
using Scripts.Level.Task.Operation;
using Scripts.UI.Items;
using Scripts.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Level.Controller
{
    public class TaskController : MonoBehaviour
    {
        [SerializeField] GameObject _holder;
        [SerializeField] Transform _root;
        [SerializeField] TextMeshProUGUI _taskText;
        [SerializeField] AnswerView _answerPrefab;
        [SerializeField] List<AnswerView> _answerViews;
        
        int _minNumber;
        int _maxNumber;
        int _answersCount;
        int _correctAnswer;
        List<Operation> _operations;
        List<int> _answerOptions = new List<int>();
        
        Addition _additionOperation = new Addition();
        Subtraction _subtractionOperation = new Subtraction();
        Multiplication _multiplicationOperation = new Multiplication();
        Division _divisionOperation = new Division();

        public List<int> WrongAnswers => _answerOptions.Where(s => s != _correctAnswer).ToList();

        public Action OnCorrectAnswer;
        public Action OnWrongAnswer;
        
        void OnEnable()
        {
            foreach (var view in _answerViews)
                view.OnAswerChoose += CheckAnswer;
        }

        void OnDisable()
        {
            foreach (var view in _answerViews)
                view.OnAswerChoose -= CheckAnswer;
        }
        
        public void SetData(TaskData data)
        {
            _minNumber = data.MinValue;
            _maxNumber = data.MaxValue;
            _answersCount = data.AnswerCount;
            _operations = data.Operations;
            
            CheckAnswersCount();
        }

        public void GenerateTask()
        {
            _answerOptions.Clear();
            var operation = (Operation)Random.Range(0, _operations.Count);
            
            switch (operation)
            {
                case Operation.Addition:
                    _taskText.text = _additionOperation.GenerateTask(_minNumber, _maxNumber, out _correctAnswer);
                    break;
                case Operation.Division:
                    _taskText.text = _subtractionOperation.GenerateTask(_minNumber, _maxNumber, out _correctAnswer);
                    break;
                case Operation.Multiplication:
                    _taskText.text =_multiplicationOperation.GenerateTask(_minNumber, _maxNumber, out _correctAnswer);
                    break;
                case Operation.Subtraction:
                    _taskText.text = _divisionOperation.GenerateTask(_minNumber, _maxNumber, out _correctAnswer);
                    break;
            }

            _answerOptions.Add(_correctAnswer);

            for (var i = 0; i < _answersCount - 1; i++)
            {
                var option = Random.Range(_minNumber, _maxNumber + 1);

                while (_answerOptions.Contains(option))
                    option = Random.Range(_minNumber, _maxNumber + 1);

                _answerOptions.Add(option);
            }

            
            ShuffleArray(_answerOptions);

            for (var i = 0; i < _answerViews.Count; i++)
                _answerViews[i].SetData(_answerOptions[i]);
            
            foreach (var view in _answerViews)
                view.Show();
        }
        
        public void ShowTask()
        {
            _holder.SetActive(true);
        }

        public void HideTask()
        {
            _holder.SetActive(false);
        }

        public void DisableWrongAnswers(int[] wrongAnswers)
        {
            foreach (var answerView in _answerViews)
            {
                if (wrongAnswers.Any(wrongAnswer => wrongAnswer == answerView.Answer))
                    answerView.Hide();
            }
        }

        void CheckAnswer(int answerIndex)
        {
            StartCoroutine(HighlightAnswer(answerIndex));
        }

        void ShuffleArray(List<int> array)
        {
            for (var i = 0; i < array.Count; i++)
            {
                var j = Random.Range(i, array.Count);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        
        void CheckAnswersCount()
        {
            for (var i = 0; i < _answersCount; i++)
            {
                var currItemView = _answerViews.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
                
                if (currItemView == null)
                {
                    currItemView = Instantiate(_answerPrefab, _root);
                    _answerViews.Add(currItemView);
                    currItemView.OnAswerChoose += CheckAnswer;
                }
                
                currItemView.Show();
            }
        }

        IEnumerator HighlightAnswer(int answerValue)
        {
            foreach (var view in _answerViews)
                view.Interactable(false);
            
            _answerViews.First(a => a.Answer == answerValue).Highlight(answerValue == _correctAnswer);
            
            if (answerValue == _correctAnswer)
            {
                yield return Awaiters.Seconds(1f);
                
                foreach (var view in _answerViews)
                {
                    if(view.Answer != _correctAnswer)
                        view.Hide();
                }
                
                yield return Awaiters.Seconds(1f);
                
                OnCorrectAnswer?.Invoke();
            }
            else
            {
                _answerViews.First(a => a.Answer == _correctAnswer).Highlight(true);
                
                yield return Awaiters.Seconds(1f);
                OnWrongAnswer?.Invoke();
            }
        }
    }
}