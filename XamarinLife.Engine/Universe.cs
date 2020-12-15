using System;
using System.Collections;
using System.Collections.Generic;

namespace XamarinLife.Engine
{
	public class Universe : IEnumerable<CellState>
	{
		private CellState[,] cells;
		private CellState[,] tempCells;

		public Universe(int width, int height)
		{
			if (width <= 0)
				throw new ArgumentOutOfRangeException(nameof(width));
			if (height <= 0)
				throw new ArgumentOutOfRangeException(nameof(height));

			Width = width;
			Height = height;

			cells = new CellState[width, height];
		}

		public int Width { get; }

		public int Height { get; }

		public int Size => Width * Height;

		public CellState this[int x, int y]
		{
			get => cells[x, y];
			set => cells[x, y] = value;
		}

		public bool Tick()
		{
			var hasChanged = false;

			// lazy create the temp cells
			tempCells ??= new CellState[Width, Height];

			for (var x = 0; x < Width; x++)
			{
				for (var y = 0; y < Height; y++)
				{
					// read the current state
					var currentState = cells[x, y];
					var previousState = currentState;
					var neighbors = CountNeighbors(x, y);

					// step
					if (currentState == CellState.Alive)
					{
						if (neighbors < 2)
						{
							// Rule 1: dies (under pop)
							currentState = CellState.Dead;
						}
						else if (neighbors >= 2 && neighbors <= 3)
						{
							// Rule 2: lives (unchanged)
						}
						else if (neighbors > 3)
						{
							// Rule 3: dies (over pop)
							currentState = CellState.Dead;
						}
					}
					else
					{
						if (neighbors == 3)
						{
							// Rule 4: birth
							currentState = CellState.Alive;
						}
					}

					// update the temp board
					tempCells[x, y] = currentState;

					// track changes
					if (previousState != currentState)
						hasChanged = true;
				}
			}

			// swap all cells by swapping fields
			(cells, tempCells) = (tempCells, cells);

			return hasChanged;
		}

		public int CountNeighbors(int x, int y)
		{
			var count = 0;

			var minX = Math.Max(x - 1, 0);
			var maxX = Math.Min(x + 1, Width - 1);
			var minY = Math.Max(y - 1, 0);
			var maxY = Math.Min(y + 1, Height - 1);

			for (var checkX = minX; checkX <= maxX; checkX++)
			{
				for (var checkY = minY; checkY <= maxY; checkY++)
				{
					if (checkX == x && checkY == y)
						continue;

					if (cells[checkX, checkY] == CellState.Alive)
						count++;
				}
			}

			return count;
		}

		// IEnumerable<T>

		public IEnumerator<CellState> GetEnumerator()
		{
			foreach (var cell in cells)
				yield return cell;
		}

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();
	}
}
