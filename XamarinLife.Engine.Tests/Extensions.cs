using System;

namespace XamarinLife.Engine.Tests
{
	public static class Extensions
	{
		public static void SetUniverse(this Universe universe, int[] state)
		{
			if (universe.Size != state.Length)
				throw new ArgumentOutOfRangeException(nameof(state));

			for (var y = 0; y < universe.Height; y++)
			{
				for (var x = 0; x < universe.Width; x++)
				{
					var cellState = state[x + (y * universe.Width)] == 0
						? CellState.Dead
						: CellState.Alive;

					universe[x + universe.MinimumX, y + universe.MinimumY] = cellState;
				}
			}
		}
	}
}
