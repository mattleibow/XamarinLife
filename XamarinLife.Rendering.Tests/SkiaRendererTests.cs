using XamarinLife.Engine;
using Xunit;

namespace XamarinLife.Rendering.Tests
{
	public class SkiaRendererTests
	{
		public class DrawUniverse
		{
			[Fact]
			public void SingleDeadIsCorrect()
			{
				var state = new[]
				{
					0,
				};
				var universe = Utils.CreateUniverse(1, 1, state);

				using var renderer = new TestRenderer(3, 3);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void SingleAliveIsCorrect()
			{
				var state = new[]
				{
					1,
				};
				var universe = Utils.CreateUniverse(1, 1, state);

				using var renderer = new TestRenderer(3, 3);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void DeadInTwoByTwoIsCorrect()
			{
				var state = new[]
				{
					0, 0,
					0, 0,
				};
				var universe = Utils.CreateUniverse(2, 2, state);

				using var renderer = new TestRenderer(6, 6);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void SingleAliveInTwoByTwoIsCorrect()
			{
				var state = new[]
				{
					1, 0,
					0, 0,
				};
				var universe = Utils.CreateUniverse(2, 2, state);

				using var renderer = new TestRenderer(6, 6);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void SingleAliveInThreeByThreeIsCorrect()
			{
				var state = new[]
				{
					0, 0, 0,
					0, 1, 0,
					0, 0, 0,
				};
				var universe = Utils.CreateUniverse(3, 3, state);

				using var renderer = new TestRenderer(9, 9);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void DoubleAliveInThreeByThreeIsCorrect()
			{
				var state = new[]
				{
					1, 0, 0,
					0, 0, 0,
					0, 0, 1,
				};
				var universe = Utils.CreateUniverse(3, 3, state);

				using var renderer = new TestRenderer(9, 9);
				renderer.DrawUniverse(universe);

				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void PixelSetInNegativeXIsDrawn()
			{
				var universe = new Universe(3, 3);
				universe[-2, 0] = CellState.Alive;

				using var renderer = new TestRenderer(12, 9);

				renderer.DrawUniverse(universe);

				var state = new[]
				{
					0, 0, 0, 0,
					1, 0, 0, 0,
					0, 0, 0, 0,
				};
				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void PixelSetInNegativeYIsDrawn()
			{
				var universe = new Universe(3, 3);
				universe[0, -2] = CellState.Alive;

				using var renderer = new TestRenderer(9, 12);

				renderer.DrawUniverse(universe);

				var state = new[]
				{
					0, 1, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
				};
				RendererAssert.Equal(state, renderer);
			}

			[Fact]
			public void PixelSetInBigNegativeXIsDrawn()
			{
				var universe = new Universe(3, 3);
				universe[0, -10] = CellState.Alive;

				using var renderer = new TestRenderer(9, 36);

				renderer.DrawUniverse(universe);

				var state = new[]
				{
					0, 1, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
				};
				RendererAssert.Equal(state, renderer);
			}
		}
	}
}
