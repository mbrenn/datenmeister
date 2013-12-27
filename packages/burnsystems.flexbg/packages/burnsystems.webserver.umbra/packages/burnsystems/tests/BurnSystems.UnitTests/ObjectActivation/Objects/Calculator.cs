namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
	/// <summary>
	/// Just a test class implementing ICalculator
	/// </summary>
	public class Calculator : ICalculator
	{
		public int InternalAddOffset
		{
			get;
			set;
		}
		
		public int Add(int a, int b)
		{
			return a + b + this.InternalAddOffset;
		}

		public int Subtract(int a, int b)
		{
			return a - b;
		}

		public int Multiply(int a, int b)
		{
			return a * b;
		}
	}
    /// <summary>
    /// Just a test class implementing ICalculator
    /// </summary>
    public class CalculatorAddByTwo : ICalculator
    {
        public int Add(int a, int b)
        {
            return a + b + 2;
        }

        public int Subtract(int a, int b)
        {
            return a - b + 2;
        }

        public int Multiply(int a, int b)
        {
            return a * b + 2;
        }
    }
}
