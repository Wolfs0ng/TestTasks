using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Scripts.Level.Task.Operation
{
    public class Division : BaseOperation
    {
        List<int> _divisors = new List<int>();
        
        public override string GenerateTask(int minValue, int maxValue, out int answer)
        {
            _divisors.Clear();
            
            var dividend = Random.Range(minValue, maxValue);
            var divisor = 0;
            
            if (dividend == 0)
            {
                while (divisor == 0)
                    divisor = Random.Range(minValue, maxValue);
            } 
            else if (dividend == 1)
            {
                divisor = 1;
            }
            else
            {
                if (dividend % 2 == 0)
                    _divisors.Add(2);

                for (var i = 3; i <= dividend; i ++)
                {
                    if (dividend % i == 0)
                        _divisors.Add(i);
                }

                divisor = _divisors[Random.Range(0, _divisors.Count)];
            }
            
            answer = dividend / divisor;
            
            return $"{dividend} / {divisor} = ?";
        }
    }
}