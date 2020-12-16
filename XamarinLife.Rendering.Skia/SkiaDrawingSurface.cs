using SkiaSharp;

namespace XamarinLife.Rendering.Skia
{
	public class SkiaDrawingSurface : IDrawingSurface
	{
		public int Width { get; set; }

		public int Height { get; set; }

		public SKCanvas Canvas { get; set; }
	}
}
