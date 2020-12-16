using XamarinLife.Engine;
using XamarinLife.Engine.Tests;

namespace XamarinLife.Rendering.Tests
{
	public class Utils
	{
		public static Universe CreateUniverse(int width, int height, int[] initial)
		{
			var universe = new Universe(width, height);
			universe.SetUniverse(initial);
			return universe;
		}
	}
}
