using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : UIBase
{
	[SerializeField]
	Image _backgroundImage;
	[SerializeField]
	TextMeshProUGUI _countDownText;

	void OnEnable()
	{
		_backgroundImage.enabled = true;
	}

	public void SetCountDownText(int count)
	{
		_countDownText.text = count.ToString();
	}

	public void ShowStage()
	{
		_backgroundImage.enabled = false;
	}
}
