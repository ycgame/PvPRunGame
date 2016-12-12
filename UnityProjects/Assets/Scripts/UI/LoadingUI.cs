using UnityEngine;
using System.Collections;

public class LoadingUI : MonoBehaviour
{
	[SerializeField]
	Transform _Anchor;
	[SerializeField]
	float _Speed = 3f;
	
	void Update ()
	{
		if (_Anchor == null)
			return;

		_Anchor.localEulerAngles -= _Speed * Mathf.Rad2Deg * Time.deltaTime * Vector3.forward;
	}
}
