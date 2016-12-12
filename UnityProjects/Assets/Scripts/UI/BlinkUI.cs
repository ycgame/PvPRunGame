using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlinkUI : MonoBehaviour
{
	Image _TragetImage;

	void Awake()
	{
		_TragetImage = GetComponent<Image>();
	}

	void Update ()
	{
		var color = _TragetImage.color;
		color.a = (Mathf.Sin(4 * Time.time) + 1.2f) / 2.2f;
		_TragetImage.color = color;
	}
}
