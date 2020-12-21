using SkiaSharp;
using XamarinLife.Engine;

namespace XamarinLife.Rendering.Skia
{
	public class SkiaRenderer : IUniverseRenderer
	{
		private SKPaint aliveCellPaint;
		private SKPaint deadCellPaint;
		private SKColor backgroundColor;
		private SkiaViewport viewport;

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

			SetViewport(new SkiaViewport());
			UpdateTheme(new SkiaTheme());
		}

		void IUniverseRenderer.UpdateTheme(IDrawingTheme theme) =>
			UpdateTheme((SkiaTheme)theme);

		public void UpdateTheme(SkiaTheme theme)
		{
			backgroundColor = theme.Background;
			aliveCellPaint.Color = theme.Foreground;
			deadCellPaint.Color = theme.Foreground;
		}

		void IUniverseRenderer.SetViewport(IViewport viewport) =>
			SetViewport((SkiaViewport)viewport);

		public void SetViewport(SkiaViewport viewport) =>
			this.viewport = viewport;

		void IUniverseRenderer.DrawUniverse(Universe universe, IDrawingSurface surface) =>
			DrawUniverse(universe, (SkiaDrawingSurface)surface);

		public void DrawUniverse(Universe universe, SkiaDrawingSurface surface)
		{
			var canvas = surface.Canvas;

			canvas.Clear(backgroundColor);

			var offX = universe.InitialWidth % 2 == 1 ? viewport.CellSize : 0;
			var offY = universe.InitialHeight % 2 == 1 ? viewport.CellSize : 0;
			canvas.Translate((surface.Width - offX) / 2f, (surface.Height - offY) / 2f);

			for (var y = universe.MinimumY; y <= universe.MaximumY; y++)
			{
				for (var x = universe.MinimumX; x <= universe.MaximumX; x++)
				{
					var state = universe[x, y];
					var paint = state == CellState.Alive
						? aliveCellPaint
						: deadCellPaint;

					var cellRect = SKRect.Create(
						x * viewport.CellSize,
						y * viewport.CellSize,
						viewport.CellSize,
						viewport.CellSize);

					canvas.DrawRect(cellRect, paint);
				}
			}
		}
	}
}
