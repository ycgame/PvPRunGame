using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
	public class PCInput : IInput
	{
		public bool TapDown
		{
			get
			{
				return UnityEngine.Input.GetMouseButtonDown(0);
			}
		}

		public bool Tap
		{
			get
			{
				return UnityEngine.Input.GetMouseButton(0);
			}
		}

		public bool TapUp
		{
			get
			{
				return UnityEngine.Input.GetMouseButtonUp(0);
			}
		}

		public bool TapAnyUGUI
		{
			get
			{
				return EventSystem.current.IsPointerOverGameObject();
			}
		}

		public Vector2 TapPosition
		{
			get
			{
				return UnityEngine.Input.mousePosition;
			}
		}
	}
}