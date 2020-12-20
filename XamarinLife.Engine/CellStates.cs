using System;
using System.Collections.Generic;

namespace XamarinLife.Engine
{
	public class CellStates : Dictionary<(int x, int y), CellState>
	{
		internal CellState TryGetValue((int x, int y) p)
		{
			throw new NotImplementedException();
		}
	}
}
