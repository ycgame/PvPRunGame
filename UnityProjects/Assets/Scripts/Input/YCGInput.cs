using UnityEngine;

namespace Utility
{
	public static class Input
	{
		private static IInput _input;
		public static void Initialize()
		{
#if UNITY_EDITOR
		_input = new PCInput();
#elif UNITY_ANDROID || UNITY_IOS
		_input = new MobileInput();
#else
			_input = new PCInput();
#endif
		}

		public static bool TapDown { get { return _input.TapDown; } }

		public static bool Tap { get { return _input.Tap; } }

		public static bool TapUp { get { return _input.TapUp; } }

		public static bool TapAnyUGUI { get { return _input.TapAnyUGUI; } }

		public static Vector2 TapPosition { get { return _input.TapPosition; } }

		public static Vector2 TapPosition01 { get { return _input.TapPosition01; } }
	}
}