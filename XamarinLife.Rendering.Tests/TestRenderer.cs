using System;
using System.IO;
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
				Canvas = canvas,
			};
			theme = new SkiaTheme
			{
				Background = SKColors.White,
				Foreground = SKColors.Black,
				CellSize = 3,
			};

			renderer.UpdateTheme(theme);
		}

		public int Width => bitmap.Width;

		public int Height => bitmap.Height;

		public int CellSize => theme.CellSize;

		public void DrawUniverse(Universe universe)
		{
			// offset a bit to consider bounds changes
			var offX = universe.Width - universe.InitialWidth;
			var offY = universe.Height - universe.InitialHeight;
			canvas.Translate(offX * CellSize / 2f, offY * CellSize / 2f);

			renderer.DrawUniverse(universe, drawingSurface);

			//using var pixmap = bitmap.PeekPixels();
			//using var file = File.Create("test.png");
			//pixmap.Encode(file, SKEncodedImageFormat.Png, 100);
		}

		public SKColor GetPixel(int x, int y) =>
			bitmap.GetPixel(x, y);

		public void Dispose()
		{
			canvas.Dispose();
			bitmap.Dispose();
		}
	}
}
