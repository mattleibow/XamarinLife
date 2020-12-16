using SkiaSharp;

namespace XamarinLife.Rendering.Skia
{
	public class SkiaTheme : IDrawingTheme
	{
		public SKColor Background { get; set; } = SKColors.White;

		public SKColor Foreground { get; set; } = SKColors.Black;
	}
}
