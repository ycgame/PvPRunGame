using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
	[SerializeField]
	Sprite[] _Numbers;
	[SerializeField]
	Image _NumberImage;

	public void SetNumber(int num)
	{
		_NumberImage.sprite = _Numbers[num];
	}
}
