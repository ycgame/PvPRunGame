using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : UIBase
{
	[SerializeField]
	Image _backgroundImage;
	[SerializeField]
	GameObject _vsInfo;
	[SerializeField]
	TextMeshProUGUI _playerNameText;
	[SerializeField]
	TextMeshProUGUI _opponentNameText;
	[SerializeField]
	TextMeshProUGUI _playerRateText;
	[SerializeField]
	TextMeshProUGUI _opponentRateText;
	[SerializeField]
	Image[] _countDownImages;

	void OnEnable()
	{
		SetCountDownActive(true);
		if (GameManager.Instance.IsNetwork)
		{
			_playerNameText.text = NetworkManager.Instance.Self.name;
			_opponentNameText.text = NetworkManager.Instance.Opponent.name;
			_playerRateText.text = NetworkManager.Instance.Self.rate.ToString();
			_opponentRateText.text = NetworkManager.Instance.Opponent.rate.ToString();
		}
		else
		{
			SetVSActive(false);
			foreach (var countDown in _countDownImages)
			{
				countDown.enabled = false;
			}
		}
	}

	public void HideCountDown()
	{
		foreach (var countDown in _countDownImages)
		{
			countDown.enabled = false;
		}
	}

	public void SetCountDown(int count)
	{
		HideCountDown();
		_countDownImages[count-1].enabled = true;
	}

	public void ShowStage()
	{
		SetCountDownActive(false);
	}

	void SetCountDownActive(bool value)
	{
		_backgroundImage.enabled = value;
		SetVSActive(value);
	}

	void SetVSActive(bool value)
	{
		_vsInfo.SetActive(value);
	}
}
