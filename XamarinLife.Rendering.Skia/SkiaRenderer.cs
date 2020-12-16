using System;
using SkiaSharp;
using XamarinLife.Engine;

namespace XamarinLife.Rendering.Skia
{
	public class SkiaRenderer : IUniverseRenderer
	{
		private SKPaint aliveCellPaint;
		private SKPaint deadCellPaint;
		private SKColor backgroundColor;

		public SkiaRenderer()
		{
			aliveCellPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = SKColors.Black,
			};
			deadCellPaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Black,
				StrokeWidth = 1,
			};
		}

		void IUniverseRenderer.UpdateTheme(IDrawingTheme theme) =>
			UpdateTheme((SkiaTheme)theme);

		public void UpdateTheme(SkiaTheme theme)
		{
			backgroundColor = theme.Background;
			aliveCellPaint.Color = theme.Foreground;
			deadCellPaint.Color = theme.Foreground;
		}

		void IUniverseRenderer.DrawUniverse(Universe universe, IDrawingSurface surface) =>
			DrawUniverse(universe, (SkiaDrawingSurface)surface);

		public void DrawUniverse(Universe universe, SkiaDrawingSurface surface)
		{
			var cellSizeX = surface.Width / universe.Width;
			var cellSizeY = surface.Height / universe.Height;
			var cellSize = Math.Min(cellSizeX, cellSizeY);

			surface.Canvas.Clear(backgroundColor);

			for (var x = 0; x < universe.Width; x++)
			{
				for (var y = 0; y < universe.Height; y++)
				{
					var cellRect = SKRect.Create(x * cellSize, y * cellSize, cellSize, cellSize);
					var paint = universe[x, y] == CellState.Alive
						? aliveCellPaint
						: deadCellPaint;

					surface.Canvas.DrawRect(cellRect, paint);
				}
			}
		}
	}
}
