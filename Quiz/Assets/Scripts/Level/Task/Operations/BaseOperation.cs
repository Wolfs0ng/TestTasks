namespace Scripts.Level.Task.Operation
{
    public abstract class BaseOperation
    {
        public abstract string GenerateTask(int minValue, int maxValue, out int answer);
    }
}