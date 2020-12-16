using SkiaSharp;
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
				var universe = Utils.CreateUniverse(1, 1, new[]
				{
					0,
				});

				using var renderer = new TestRenderer(3, 3);

				renderer.DrawUniverse(universe);

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 1));
			}

			[Fact]
			public void SingleAliveIsCorrect()
			{
				var universe = Utils.CreateUniverse(1, 1, new[]
				{
					1,
				});

				using var renderer = new TestRenderer(3, 3);

				renderer.DrawUniverse(universe);

				Assert.Equal(SKColors.Black, renderer.GetPixel(1, 1));
			}

			[Fact]
			public void SingleAliveInThreeByThreeIsCorrect()
			{
				var universe = Utils.CreateUniverse(3, 3, new[]
				{
					0, 0, 0,
					0, 1, 0,
					0, 0, 0,
				});

				using var renderer = new TestRenderer(9, 9);

				renderer.DrawUniverse(universe);

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 1));
				Assert.Equal(SKColors.White, renderer.GetPixel(4, 1));
				Assert.Equal(SKColors.White, renderer.GetPixel(7, 1));

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 4));
				Assert.Equal(SKColors.Black, renderer.GetPixel(4, 4));
				Assert.Equal(SKColors.White, renderer.GetPixel(7, 4));

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 7));
				Assert.Equal(SKColors.White, renderer.GetPixel(4, 7));
				Assert.Equal(SKColors.White, renderer.GetPixel(7, 7));
			}

			[Fact]
			public void DoubleAliveInThreeByThreeIsCorrect()
			{
				var universe = Utils.CreateUniverse(3, 3, new[]
				{
					1, 0, 0,
					0, 0, 0,
					0, 0, 1,
				});

				using var renderer = new TestRenderer(9, 9);

				renderer.DrawUniverse(universe);

				Assert.Equal(SKColors.Black, renderer.GetPixel(1, 1));
				Assert.Equal(SKColors.White, renderer.GetPixel(4, 1));
				Assert.Equal(SKColors.White, renderer.GetPixel(7, 1));

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 4));
				Assert.Equal(SKColors.White, renderer.GetPixel(4, 4));
				Assert.Equal(SKColors.White, renderer.GetPixel(7, 4));

				Assert.Equal(SKColors.White, renderer.GetPixel(1, 7));
				Assert.Equal(SKColors.White, renderer.GetPixel(4, 7));
				Assert.Equal(SKColors.Black, renderer.GetPixel(7, 7));
			}
		}
	}
}
