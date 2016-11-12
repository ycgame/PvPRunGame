using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
	public class MobileInput : IInput
	{

		public bool TapDown
		{
			get
			{
				return IsValidTouchCount(1) && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began;
			}
		}

		public bool Tap
		{
			get
			{
				return IsValidTouchCount(1) && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved;
			}
		}

		public bool TapUp
		{
			get
			{
				return IsValidTouchCount(1) && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended;
			}
		}

		public bool TapAnyUGUI
		{
			get
			{
				return IsValidTouchCount(1) && EventSystem.current.IsPointerOverGameObject(0);
			}
		}

		public Vector2 TapPosition
		{
			get
			{
				if (IsValidTouchCount(1))
					return UnityEngine.Input.GetTouch(0).position;
				else
					return Vector2.zero;
			}
		}

		public Vector2 TapPosition01
		{
			get
			{
				Vector2 p = TapPosition;
				return new Vector2(p.x / Screen.width, p.y / Screen.height);
			}
		}

		private static bool IsValidTouchCount(int count)
		{
			return count <= UnityEngine.Input.touchCount;
		}

		public bool GetTapDown(int index)
		{
			if (index < 0 || index >= UnityEngine.Input.touchCount)
				return false;
			else
				return UnityEngine.Input.GetTouch(index).phase == TouchPhase.Began;
		}

		public bool GetTap(int index)
		{
			if (index < 0 || index >= UnityEngine.Input.touchCount)
				return false;
			else
				return UnityEngine.Input.GetTouch(index).phase == TouchPhase.Moved;
		}

		public bool GetTapUp(int index)
		{
			if (index < 0 || index >= UnityEngine.Input.touchCount)
				return false;
			else
				return UnityEngine.Input.GetTouch(index).phase == TouchPhase.Ended;
		}

		public Vector2 GetTapPosition(int index)
		{
			if (index < 0 || index >= UnityEngine.Input.touchCount)
				return Vector2.zero;
			else
				return UnityEngine.Input.GetTouch(index).position;
		}

		public Vector2 GetTapPosition01(int index)
		{
			Vector2 p = GetTapPosition(index);
			return new Vector2(p.x / Screen.width, p.y / Screen.height);
		}
	}
}