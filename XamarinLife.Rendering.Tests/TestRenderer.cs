using System;
using SkiaSharp;
using XamarinLife.Engine;
using XamarinLife.Rendering.Skia;

namespace XamarinLife.Rendering.Tests
{
	public class TestRenderer : IDisposable
	{
		private SkiaRenderer renderer;
		private SkiaDrawingSurface drawingSurface;
		private SkiaTheme theme;

		private SKBitmap bitmap;
		private SKCanvas canvas;

		public TestRenderer(int surfaceWidth, int surfaceHeight)
		{
			bitmap = new SKBitmap(surfaceWidth, surfaceHeight);
			canvas = new SKCanvas(bitmap);

			renderer = new SkiaRenderer
			{
			};
			drawingSurface = new SkiaDrawingSurface
			{
				Width = surfaceWidth,
				Height = surfaceHeight,
				Canvas = canvas
			};
			theme = new SkiaTheme
			{
				Background = SKColors.White,
				Foreground = SKColors.Black,
			};

			renderer.UpdateTheme(theme);
		}

		public void DrawUniverse(Universe universe) =>
			renderer.DrawUniverse(universe, drawingSurface);

		public SKColor GetPixel(int x, int y) =>
			bitmap.GetPixel(x, y);

		public void Dispose()
		{
			canvas.Dispose();
			bitmap.Dispose();
		}
	}
}
