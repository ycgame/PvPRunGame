using UnityEngine;

namespace Utility
{
	public interface IInput
	{
		bool TapDown { get; }
		bool Tap { get; }
		bool TapUp { get; }
		bool TapAnyUGUI { get; }
		Vector2 TapPosition { get; }
		Vector2 TapPosition01 { get; }
	}
}
