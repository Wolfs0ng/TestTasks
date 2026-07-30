using Random = UnityEngine.Random;

namespace Scripts.Level.Task.Operation
{
    public class Addition : BaseOperation
    {
        public override string GenerateTask(int minValue, int maxValue, out int answer)
        {
            var firstValue = Random.Range(minValue, maxValue);
            var secondValue = Random.Range(minValue, maxValue);
            
            answer = firstValue + secondValue;
            
            return $"{firstValue} + {secondValue} = ?";
        }
    }
}