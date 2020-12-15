using System;
using Xunit;

namespace XamarinLife.Engine.Tests
{
	public class UniverseTests
	{
		public class Constructor
		{

			[Theory]
			[InlineData(1, 1)]
			[InlineData(1, 10)]
			[InlineData(10, 10)]
			[InlineData(10, 1)]
			[InlineData(100, 100)]
			public void NewUniverseIsValid(int width, int height)
			{
				var universe = new Universe(width, height);

				Assert.Equal(width, universe.Width);
				Assert.Equal(height, universe.Height);

				Assert.Equal(width * height, universe.Size);
				Assert.All(universe, c => Assert.Equal(CellState.Dead, c));
			}

			[Theory]
			[InlineData(-1, 1)]
			[InlineData(1, -1)]
			[InlineData(-10, -10)]
			[InlineData(0, 0)]
			[InlineData(0, 1)]
			[InlineData(1, 0)]
			public void InvalidUniverseThrows(int width, int height)
			{
				Assert.Throws<ArgumentOutOfRangeException>(() => new Universe(width, height));
			}
		}

		public class Tick
		{
			[Fact]
			public void AllDeadStaysAllDead()
			{
				var universe = new Universe(10, 10);

				var changed = universe.Tick();

				Assert.False(changed);
				Assert.All(universe, c => Assert.Equal(CellState.Dead, c));
			}

			[Fact]
			public void CellWithNoNeighborsDies()
			{
				var universe = new Universe(5, 5);

				universe[2, 2] = CellState.Alive;

				var changed = universe.Tick();

				Assert.True(changed);
				Assert.All(universe, c => Assert.Equal(CellState.Dead, c));
			}

			[Fact]
			public void CellWithOneNeighborDies()
			{
				var universe = new Universe(5, 5);

				universe[2, 2] = CellState.Alive;
				universe[2, 3] = CellState.Alive;

				var changed = universe.Tick();

				Assert.True(changed);
				Assert.All(universe, c => Assert.Equal(CellState.Dead, c));
			}

			[Fact]
			public void CellWithTwoNeighborsStaysAlive()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var changed = universe.Tick();

				Assert.True(changed);
				var expected = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 1, 1, 1, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};
				CompareUniverse(expected, universe);
			}

			[Fact]
			public void CellWithOnlyThreeNeighborsStaysAlive()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 0, 0,
					0, 1, 1, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var changed = universe.Tick();

				Assert.False(changed);
				CompareUniverse(initial, universe);
			}

			[Fact]
			public void CellWithThreeNeighborsStaysAlive()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 0, 0,
					0, 1, 1, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 1,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var changed = universe.Tick();

				Assert.True(changed);
				var expected = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 0, 0,
					0, 1, 1, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};
				CompareUniverse(expected, universe);
			}

			[Fact]
			public void CellWithFourNeighborsStaysAlive()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 1, 0, 0,
					0, 1, 1, 1, 0,
					0, 0, 1, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var changed = universe.Tick();

				Assert.True(changed);
				var expected = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 1, 0,
					0, 1, 0, 1, 0,
					0, 1, 1, 1, 0,
					0, 0, 0, 0, 0,
				};
				CompareUniverse(expected, universe);
			}

			[Fact]
			public void DiamondPatternStaysTheSame()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0, 0,
					0, 0, 1, 1, 0, 0,
					0, 1, 0, 0, 1, 0,
					0, 0, 1, 1, 0, 0,
					0, 0, 0, 0, 0, 0,
				};

				var universe = new Universe(6, 5);
				SetUniverse(universe, initial);

				var changed = universe.Tick();

				Assert.False(changed);
				CompareUniverse(initial, universe);
			}

			[Fact]
			public void BarOscillates()
			{
				var step1 = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 0, 0, 0,
				};
				var step2 = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 1, 1, 1, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, step1);

				var changed = universe.Tick();
				Assert.True(changed);

				CompareUniverse(step2, universe);

				changed = universe.Tick();
				Assert.True(changed);

				CompareUniverse(step1, universe);
			}
		}

		public class CountNeighbors
		{
			[Fact]
			public void DeadUniverseHasNoNeighbors()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(0, neighbors);
			}

			[Fact]
			public void SingleCellUniverseHasNoNeighbors()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 1, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(0, neighbors);
			}

			[Fact]
			public void OneRightHasOneNeighbor()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 1, 1, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(1, neighbors);
			}

			[Fact]
			public void TwoRightHasOneNeighbor()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 1, 1, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(1, neighbors);
			}

			[Fact]
			public void TreeByThreeBoxHasEightNeighbors()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 1, 0,
					0, 1, 0, 1, 0,
					0, 1, 1, 1, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(8, neighbors);
			}

			[Fact]
			public void TreeByThreeSolidHasEightNeighbors()
			{
				var initial = new[]
				{
					0, 0, 0, 0, 0,
					0, 1, 1, 1, 0,
					0, 1, 1, 1, 0,
					0, 1, 1, 1, 0,
					0, 0, 0, 0, 0,
				};

				var universe = new Universe(5, 5);
				SetUniverse(universe, initial);

				var neighbors = universe.CountNeighbors(2, 2);
				Assert.Equal(8, neighbors);
			}
		}

		private static void CompareUniverse(int[] expected, Universe actual)
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

		private static void SetUniverse(Universe universe, int[] state)
		{
			if (universe.Size != state.Length)
				throw new ArgumentOutOfRangeException(nameof(state));

			for (var x = 0; x < universe.Width; x++)
			{
				for (var y = 0; y < universe.Height; y++)
				{
					var cellState = state[x + (y * universe.Width)] == 0
						? CellState.Dead
						: CellState.Alive;

					universe[x, y] = cellState;
				}
			}
		}
	}
}
