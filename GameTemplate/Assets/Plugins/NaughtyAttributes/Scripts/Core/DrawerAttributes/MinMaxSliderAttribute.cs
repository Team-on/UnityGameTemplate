using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinMaxSliderAttribute : DrawerAttribute
	{
		public float MinValue { get; private set; }
		public float MaxValue { get; private set; }
		public bool IsInt { get; private set; }

		public MinMaxSliderAttribute(float minValue, float maxValue, bool isInt)
		{
			MinValue = minValue;
			MaxValue = maxValue;
			IsInt = isInt;
		}
	}
}
