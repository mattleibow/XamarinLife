using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using XamarinLife.Engine;
using XamarinLife.Rendering.Skia;

namespace XamarinLife
{
	public partial class MainPage : ContentPage
	{
		private Universe universe = new Universe(10, 10);

		private SkiaRenderer renderer = new SkiaRenderer();
		private SkiaDrawingSurface drawingSurface = new SkiaDrawingSurface();
		private SkiaViewport viewport = new SkiaViewport();

		public MainPage()
		{
			InitializeComponent();

			universe.CellsChanged += OnCellsChanged;

			renderer.SetViewport(viewport);
		}

		private void OnCellsChanged(object sender, EventArgs e)
		{
			canvasView.InvalidateSurface();
		}

		private void OnPaintUniverse(object sender, SKPaintSurfaceEventArgs e)
		{
			drawingSurface.Canvas = e.Surface.Canvas;
			drawingSurface.Width = e.Info.Width;
			drawingSurface.Height = e.Info.Height;

			e.Surface.Canvas.Translate(scrollOffest);

			renderer.DrawUniverse(universe, drawingSurface);
		}

		private void OnRandomizeClicked(object sender, EventArgs e)
		{
			universe.Randomize();
		}

		private void OnClearClicked(object sender, EventArgs e)
		{
			universe.Clear();
		}

		private void OnTickClicked(object sender, EventArgs e)
		{
			universe.Tick();
		}

		SKPoint scrollOffest;
		SKPoint downLocation;

		private void OnTouch(object sender, SKTouchEventArgs e)
		{
			switch (e.ActionType)
			{
				case SKTouchAction.Pressed:
					downLocation = e.Location;
					break;
				case SKTouchAction.Moved when e.InContact:
					scrollOffest += e.Location - downLocation;
					downLocation = e.Location;
					canvasView.InvalidateSurface();
					break;
				case SKTouchAction.WheelChanged:
					var val = cellSizeSlider.Value + (e.WheelDelta / 100.0);
					cellSizeSlider.Value = Math.Round(val, 0);
					break;
			}

			e.Handled = true;
		}

		private void OnCellSizeChanged(object sender, ValueChangedEventArgs e)
		{
			var oldValue = e.OldValue;
			var newValue = (int)e.NewValue;

			if (e.OldValue > 0)
			{
				var dx = scrollOffest.X / oldValue;
				var dy = scrollOffest.Y / oldValue;

				dx *= newValue;
				dy *= newValue;

				scrollOffest = new SKPoint((float)dx, (float)dy);
			}

			viewport.CellSize = newValue;
			canvasView.InvalidateSurface();
		}
	}
}
