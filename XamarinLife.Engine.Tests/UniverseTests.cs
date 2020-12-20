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

		public class Indexer
		{
			[Theory]
			[InlineData(-2, -2, -2, -2, 4, 4)]
			[InlineData(-2, 0, -2, -1, 4, 3)]
			[InlineData(0, -2, -1, -2, 3, 4)]
			[InlineData(0, -10, -1, -10, 3, 12)]
			[InlineData(0, 0, -1, -1, 3, 3)]
			[InlineData(1, 1, -1, -1, 3, 3)]
			[InlineData(2, 2, -1, -1, 4, 4)]
			[InlineData(3, 3, -1, -1, 5, 5)]
			[InlineData(1, 10, -1, -1, 3, 12)]
			public void IndexerChangesBounds(int x, int y, int minX, int minY, int width, int height)
			{
				var universe = new Universe(3, 3);

				universe[x, y] = CellState.Alive;

				UniverseAssert.BoundsEqual(minX, minY, width, height, universe);
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
				universe.SetUniverse(initial);

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
				UniverseAssert.Equal(expected, universe);
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
				universe.SetUniverse(initial);

				var changed = universe.Tick();

				Assert.False(changed);
				UniverseAssert.Equal(initial, universe);
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
				universe.SetUniverse(initial);

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
				UniverseAssert.Equal(expected, universe);
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
				universe.SetUniverse(initial);

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
				UniverseAssert.Equal(expected, universe);
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
				universe.SetUniverse(initial);

				var changed = universe.Tick();

				Assert.False(changed);
				UniverseAssert.Equal(initial, universe);
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
				universe.SetUniverse(step1);

				var changed = universe.Tick();
				Assert.True(changed);

				UniverseAssert.Equal(step2, universe);

				changed = universe.Tick();
				Assert.True(changed);

				UniverseAssert.Equal(step1, universe);
			}

			[Fact]
			public void GliderMovesBoundsMultipleTimes()
			{
				var initial = new[]
				{
					0, 1, 0,
					0, 0, 1,
					1, 1, 1,
				};
				var step1 = new[]
				{
					0, 0, 0,
					1, 0, 1,
					0, 1, 1,
					0, 1, 0,
				};
				var step2 = new[]
				{
					0, 0, 0,
					0, 0, 1,
					1, 0, 1,
					0, 1, 1,
				};
				var step3 = new[]
				{
					0, 0, 0, 0,
					0, 1, 0, 0,
					0, 0, 1, 1,
					0, 1, 1, 0,
				};

				var universe = new Universe(3, 3);
				universe.SetUniverse(initial);

				universe.Tick();
				UniverseAssert.Equal(step1, universe);
				UniverseAssert.BoundsEqual(-1, -1, 3, 4, universe);

				universe.Tick();
				UniverseAssert.Equal(step2, universe);
				UniverseAssert.BoundsEqual(-1, -1, 3, 4, universe);

				universe.Tick();
				UniverseAssert.Equal(step3, universe);
				UniverseAssert.BoundsEqual(-1, -1, 4, 4, universe);
			}

			[Fact]
			public void GliderMovesBoundsDown()
			{
				var initial = new[]
				{
					0, 1, 0,
					0, 0, 1,
					1, 1, 1,
				};
				var step1 = new[]
				{
					0, 0, 0,
					1, 0, 1,
					0, 1, 1,
					0, 1, 0,
				};

				var universe = new Universe(3, 3);
				universe.SetUniverse(initial);

				universe.Tick();

				UniverseAssert.Equal(step1, universe);
				UniverseAssert.BoundsEqual(-1, -1, 3, 4, universe);
			}

			[Fact]
			public void GliderMovesBoundsRight()
			{
				var initial = new[]
				{
					0, 1, 1,
					1, 0, 1,
					0, 0, 1,
				};
				var step1 = new[]
				{
					0, 1, 1, 0,
					0, 0, 1, 1,
					0, 1, 0, 0,
				};

				var universe = new Universe(3, 3);
				universe.SetUniverse(initial);

				universe.Tick();

				UniverseAssert.Equal(step1, universe);
				UniverseAssert.BoundsEqual(-1, -1, 4, 3, universe);
			}

			[Fact]
			public void GliderMovesBoundsLeft()
			{
				var initial = new[]
				{
					1, 1, 0,
					1, 0, 1,
					1, 0, 0,
				};
				var step1 = new[]
				{
					0, 1, 1, 0,
					1, 1, 0, 0,
					0, 0, 1, 0,
				};

				var universe = new Universe(3, 3);
				universe.SetUniverse(initial);

				universe.Tick();

				UniverseAssert.Equal(step1, universe);
				UniverseAssert.BoundsEqual(-2, -1, 4, 3, universe);
			}

			[Fact]
			public void GliderMovesBoundsUp()
			{
				var initial = new[]
				{
					1, 1, 1,
					0, 0, 1,
					0, 1, 0,
				};
				var step1 = new[]
				{
					0, 1, 0,
					0, 1, 1,
					1, 0, 1,
					0, 0, 0,
				};

				var universe = new Universe(3, 3);
				universe.SetUniverse(initial);

				universe.Tick();

				UniverseAssert.Equal(step1, universe);
				UniverseAssert.BoundsEqual(-1, -2, 3, 4, universe);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
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
				universe.SetUniverse(initial);

				var neighbors = universe.CountNeighbors(0, 0);
				Assert.Equal(8, neighbors);
			}
		}

		public class Randomize
		{
			[Fact]
			public void RandomUniverseIsNotEmptyAndValid()
			{
				var universe = new Universe(10, 10);
				universe.Randomize();

				var hasAlive = false;
				foreach (var cell in universe)
				{
					if (cell == CellState.Alive)
						hasAlive = true;
					else
						Assert.Equal(CellState.Dead, cell);
				}
				Assert.True(hasAlive);
			}
		}

		public class AliveCells
		{
			[Fact]
			public void EmptyUniverseIsZero()
			{
				var universe = new Universe(10, 10);

				Assert.Equal(0, universe.AliveCells);
			}

			[Fact]
			public void DeadCellIsZero()
			{
				var universe = new Universe(10, 10);

				universe[3, 3] = CellState.Dead;

				Assert.Equal(0, universe.AliveCells);
			}

			[Fact]
			public void OneCellIsOne()
			{
				var universe = new Universe(10, 10);

				universe[3, 3] = CellState.Alive;

				Assert.Equal(1, universe.AliveCells);
			}

			[Fact]
			public void OneCellIsOneEvenWhenSetAgain()
			{
				var universe = new Universe(10, 10);

				universe[3, 3] = CellState.Alive;
				universe[3, 3] = CellState.Alive;

				Assert.Equal(1, universe.AliveCells);
			}

			[Fact]
			public void TwoCellsAreTwo()
			{
				var universe = new Universe(10, 10);

				universe[3, 3] = CellState.Alive;
				universe[5, 5] = CellState.Alive;

				Assert.Equal(2, universe.AliveCells);
			}

			[Fact]
			public void OneKilledCellIsZero()
			{
				var universe = new Universe(10, 10);

				universe[3, 3] = CellState.Alive;

				Assert.Equal(1, universe.AliveCells);

				universe[3, 3] = CellState.Dead;

				Assert.Equal(0, universe.AliveCells);
			}
		}

		public class CellsChanged
		{
			[Fact]
			public void EventFiresExactlyOnceOnCellSet()
			{
				var eventCount = 0;

				var universe = new Universe(10, 10);
				universe.CellsChanged += OnCellsChanged;

				universe[3, 3] = CellState.Alive;

				Assert.Equal(1, eventCount);

				void OnCellsChanged(object sender, EventArgs e)
				{
					eventCount++;
				}
			}

			[Fact]
			public void EventFiresExactlyOnceOnRandomize()
			{
				var eventCount = 0;

				var universe = new Universe(10, 10);
				universe.CellsChanged += OnCellsChanged;

				universe.Randomize();

				Assert.Equal(1, eventCount);

				void OnCellsChanged(object sender, EventArgs e)
				{
					eventCount++;
				}
			}

			[Fact]
			public void EventFiresExactlyOnceOnClear()
			{
				var eventCount = 0;

				var universe = new Universe(10, 10);
				universe.CellsChanged += OnCellsChanged;

				universe.Clear();

				Assert.Equal(1, eventCount);

				void OnCellsChanged(object sender, EventArgs e)
				{
					eventCount++;
				}
			}

			[Fact]
			public void EventDoesNotFireOnUnchangedCell()
			{
				var eventCount = 0;

				var universe = new Universe(10, 10);
				universe[3, 3] = CellState.Alive; // initial set

				universe.CellsChanged += OnCellsChanged;

				universe[3, 3] = CellState.Alive; // "change"

				Assert.Equal(0, eventCount);

				void OnCellsChanged(object sender, EventArgs e)
				{
					eventCount++;
				}
			}

			[Fact]
			public void EventDoesNotFireForDiamondPattern()
			{
				var eventCount = 0;

				var initial = new[]
				{
					0, 0, 0, 0, 0, 0,
					0, 0, 1, 1, 0, 0,
					0, 1, 0, 0, 1, 0,
					0, 0, 1, 1, 0, 0,
					0, 0, 0, 0, 0, 0,
				};

				var universe = new Universe(6, 5);
				universe.SetUniverse(initial);
				universe.CellsChanged += OnCellsChanged;

				universe.Tick();

				Assert.Equal(0, eventCount);

				void OnCellsChanged(object sender, EventArgs e)
				{
					eventCount++;
				}
			}
		}
	}
}
