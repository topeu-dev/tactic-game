namespace Turn
{
    public class Initiative
    {
        public int Priority;
        public int Value;

        public Initiative(int initiativeValue, int priority)
        {
            Value = initiativeValue;
            Priority = priority;
        }
    }
}