using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Level.Controller
{
    public class BoosterController
    {
        int _initialBoosterCount;
        int _appearancePeriod;
        int _removedAnswersCount;

        int _currentCorrectAnswersCount;
        int _currentBoostersCount;

        public bool IsBoosterAvailable => _currentBoostersCount > 0;
        public bool IsUseForThisTask { get; private set; }

        public void SetData(BoosterData data)
        {
            _initialBoosterCount = data.InitialBoosterCount;
            _appearancePeriod = data.AppearancePeriod;
            _removedAnswersCount = data.RemovedAnswersCount;

            _currentBoostersCount = _initialBoosterCount;
            _currentCorrectAnswersCount = 0;
        }

        public void AddCorrectAnswer()
        {
            IsUseForThisTask = false;
            _currentCorrectAnswersCount++;

            if (_currentCorrectAnswersCount == _appearancePeriod)
            {
                _currentCorrectAnswersCount = 0;
                _currentBoostersCount++;
            }
        }

        public int[] UseFiftyFifty(List<int> answerOptions)
        {
            IsUseForThisTask = true;
            _currentBoostersCount--;

            var removedAnswers = GetRandomNumbers(answerOptions, _removedAnswersCount);

            return removedAnswers;
        }

        int[] GetRandomNumbers(List<int> numbers, int count)
        {
            if (numbers.Count < count)
            {
                Debug.LogError("Not enough elements in Array!");
                return Array.Empty<int>();
            }

            var indexes = new List<int>();
            
            for (var i = 0; i < numbers.Count; i++)
                indexes.Add(i);

            var selectedIndexes = new List<int>();

            for (var i = 0; i < count; i++)
            {
                while (selectedIndexes.Contains(indexes[i]))
                    indexes.RemoveAt(i);

                selectedIndexes.Add(indexes[i]);
            }

            var result = new int[count];

            for (var i = 0; i < count; i++)
                result[i] = numbers[selectedIndexes[i]];

            return result;
        }
    }
}
