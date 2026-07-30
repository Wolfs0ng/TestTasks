using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Level.Task.Operation
{
    public class Multiplication : BaseOperation
    {
        public override string GenerateTask(int minValue, int maxValue, out int answer)
        {
            var firstValue = Random.Range(minValue, maxValue);
            var numDigits = (int)Mathf.Log10(firstValue) + 1;
            var randomNumber = Random.Range(0, (int)Math.Pow(10, numDigits) - 1);
            var secondValue = randomNumber % firstValue;
            
            answer = firstValue * secondValue;
            
            return $"{firstValue} * {secondValue} = ?";
        }
    }
}