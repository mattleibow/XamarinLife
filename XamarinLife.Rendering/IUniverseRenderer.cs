using XamarinLife.Engine;

namespace XamarinLife.Rendering
{
	public interface IUniverseRenderer
	{
		void DrawUniverse(Universe universe, IDrawingSurface surface);

		void UpdateTheme(IDrawingTheme theme);
	}
}
