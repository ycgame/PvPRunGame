using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour
{
	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}
}
