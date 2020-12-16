using System;
using Xunit;

namespace XamarinLife.Engine.Tests
{
	public static class UniverseAssert
	{
		public static void Equal(int[] expected, Universe actual)
		{
			if (actual.Size != expected.Length)
				throw new ArgumentOutOfRangeException(nameof(expected));

			for (var x = 0; x < actual.Width; x++)
			{
				for (var y = 0; y < actual.Height; y++)
				{
					var cellState = expected[x + (y * actual.Width)] == 0
						? CellState.Dead
						: CellState.Alive;

					Assert.Equal(cellState, actual[x, y]);
				}
			}
		}
	}
}
