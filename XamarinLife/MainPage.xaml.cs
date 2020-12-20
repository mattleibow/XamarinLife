using System;
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

		public MainPage()
		{
			InitializeComponent();

			universe.CellsChanged += OnCellsChanged;
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

			e.Surface.Canvas.Translate(offsetX, 0);

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

		int offsetX = 0;

		private void OnTouch(object sender, SKTouchEventArgs e)
		{
			offsetX++;
			canvasView.InvalidateSurface();
		}
	}
}
