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
		SetCountDownActive(true);
	}

	public void SetCountDownText(int count)
	{
		_countDownText.text = count.ToString();
	}

	public void ShowStage()
	{
		SetCountDownActive(false);
	}

	void SetCountDownActive(bool value)
	{
		_backgroundImage.enabled = value;
		_countDownText.enabled = value;
	}
}
