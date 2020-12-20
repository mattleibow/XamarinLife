using System;
using System.Collections;
using System.Collections.Generic;

namespace XamarinLife.Engine
{
	public class Universe : IEnumerable<CellState>
	{
		private CellStates cells = new CellStates();
		private CellStates tempCells = new CellStates();

		public Universe(int initialWidth, int initialHeight)
		{
			if (initialWidth <= 0)
				throw new ArgumentOutOfRangeException(nameof(initialWidth));
			if (initialHeight <= 0)
				throw new ArgumentOutOfRangeException(nameof(initialHeight));

			InitialWidth = initialWidth;
			InitialHeight = initialHeight;

			Reset();
		}

		public int InitialWidth { get; }

		public int InitialHeight { get; }

		public int MinimumX { get; private set; }

		public int MinimumY { get; private set; }

		public int MaximumX { get; private set; }

		public int MaximumY { get; private set; }

		public int Width => MaximumX - MinimumX + 1;

		public int Height => MaximumY - MinimumY + 1;

		public int Size => Width * Height;

		public int AliveCells => cells.Count;

		public CellState this[int x, int y]
		{
			get => cells.TryGetValue((x, y), out var state) ? state : CellState.Dead;
			set
			{
				cells.TryGetValue((x, y), out var oldState);
				if (oldState != value)
				{
					if (value == CellState.Alive)
					{
						cells[(x, y)] = value;

						// expand the bounds
						if (x < MinimumX)
							MinimumX = x;
						if (x > MaximumX)
							MaximumX = x;
						if (y < MinimumY)
							MinimumY = y;
						if (y > MaximumY)
							MaximumY = y;
					}
					else
					{
						cells.Remove((x, y));
					}

					OnCellsChanged();
				}
			}
		}

		public bool Tick()
		{
			var hasChanged = false;

			for (var y = MinimumY - 1; y <= MaximumY + 1; y++)
			{
				for (var x = MinimumX - 1; x <= MaximumX + 1; x++)
				{
					// read the current state
					cells.TryGetValue((x, y), out var currentState);
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
					if (currentState == CellState.Alive)
					{
						tempCells[(x, y)] = currentState;

						// expand the bounds
						if (x < MinimumX)
							MinimumX = x;
						if (x > MaximumX)
							MaximumX = x;
						if (y < MinimumY)
							MinimumY = y;
						if (y > MaximumY)
							MaximumY = y;
					}
					else
					{
						tempCells.Remove((x, y));
					}

					// track changes
					if (previousState != currentState)
						hasChanged = true;
				}
			}

			// swap all cells by swapping fields
			(cells, tempCells) = (tempCells, cells);

			if (hasChanged)
				OnCellsChanged();

			return hasChanged;
		}

		public void Clear()
		{
			cells.Clear();
			OnCellsChanged();
		}

		public void Reset()
		{
			cells.Clear();
			MinimumX = InitialWidth / -2;
			MinimumY = InitialHeight / -2;
			MaximumX = MinimumX + InitialWidth - 1;
			MaximumY = MinimumY + InitialHeight - 1;
			OnCellsChanged();
		}

		public int CountNeighbors(int x, int y)
		{
			var count = 0;

			var minX = Math.Max(x - 1, MinimumX);
			var maxX = Math.Min(x + 1, MaximumX);
			var minY = Math.Max(y - 1, MinimumY);
			var maxY = Math.Min(y + 1, MaximumY);

			for (var checkY = minY; checkY <= maxY; checkY++)
			{
				for (var checkX = minX; checkX <= maxX; checkX++)
				{
					if (checkX == x && checkY == y)
						continue;

					if (cells.TryGetValue((checkX, checkY), out var state) && state == CellState.Alive)
						count++;
				}
			}

			return count;
		}

		public void Randomize()
		{
			var rnd = new Random();

			for (var y = MinimumY; y <= MaximumY; y++)
			{
				for (var x = MinimumX; x <= MaximumX; x++)
				{
					if (rnd.Next(2) == 1)
						cells[(x, y)] = CellState.Alive;
					else
						cells.Remove((x, y));
				}
			}

			OnCellsChanged();
		}

		public event EventHandler CellsChanged;

		protected virtual void OnCellsChanged() =>
			CellsChanged?.Invoke(this, EventArgs.Empty);

		// IEnumerable<T>

		public IEnumerator<CellState> GetEnumerator()
		{
			for (var y = MinimumY; y <= MaximumY; y++)
			{
				for (var x = MinimumX; x <= MaximumX; x++)
				{
					yield return this[x, y];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();
	}
}
