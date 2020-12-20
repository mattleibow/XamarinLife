using SkiaSharp;
using XamarinLife.Engine;

namespace XamarinLife.Rendering.Skia
{
	public class SkiaRenderer : IUniverseRenderer
	{
		private SKPaint aliveCellPaint;
		private SKPaint deadCellPaint;
		private SKColor backgroundColor;
		private int cellSize;

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
			cellSize = 10;
		}

		void IUniverseRenderer.UpdateTheme(IDrawingTheme theme) =>
			UpdateTheme((SkiaTheme)theme);

		public void UpdateTheme(SkiaTheme theme)
		{
			backgroundColor = theme.Background;
			aliveCellPaint.Color = theme.Foreground;
			deadCellPaint.Color = theme.Foreground;
			cellSize = theme.CellSize;
		}

		void IUniverseRenderer.DrawUniverse(Universe universe, IDrawingSurface surface) =>
			DrawUniverse(universe, (SkiaDrawingSurface)surface);

		public void DrawUniverse(Universe universe, SkiaDrawingSurface surface)
		{
			var canvas = surface.Canvas;

			canvas.Clear(backgroundColor);

			var offX = universe.InitialWidth % 2 == 1 ? cellSize : 0;
			var offY = universe.InitialHeight % 2 == 1 ? cellSize : 0;
			canvas.Translate((surface.Width - offX) / 2f, (surface.Height - offY) / 2f);

			for (var y = universe.MinimumY; y <= universe.MaximumY; y++)
			{
				for (var x = universe.MinimumX; x <= universe.MaximumX; x++)
				{
					var cellRect = SKRect.Create(x * cellSize, y * cellSize, cellSize, cellSize);
					var state = universe[x, y];
					var paint = state == CellState.Alive
						? aliveCellPaint
						: deadCellPaint;

					canvas.DrawRect(cellRect, paint);
				}
			}
		}
	}
}
