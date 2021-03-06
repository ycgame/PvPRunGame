﻿using UnityEngine;

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
		bool GetTapDown(int index);
		bool GetTap(int index);
		bool GetTapUp(int index);
		Vector2 GetTapPosition(int index);
		Vector2 GetTapPosition01(int index);
	}
}
