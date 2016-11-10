using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour
{
	public virtual void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}
}
