using System;
using SkiaSharp;
using Xunit;

namespace XamarinLife.Rendering.Tests
{
	public static class RendererAssert
	{
		public static void Equal(int[] expected, TestRenderer actual)
		{
			var w = actual.Width / actual.CellSize;
			var h = actual.Height / actual.CellSize;

			if (w * h != expected.Length)
				throw new ArgumentOutOfRangeException(nameof(expected));

			for (var y = 0; y < h; y++)
			{
				for (var x = 0; x < w; x++)
				{
					var expectedColor = expected[x + (y * w)] == 0
						? SKColors.White
						: SKColors.Black;

					var px = (actual.CellSize / 2) + (x * actual.CellSize);
					var py = (actual.CellSize / 2) + (y * actual.CellSize);

					Assert.Equal(expectedColor, actual.GetPixel(px, py));
				}
			}
		}
	}
}
