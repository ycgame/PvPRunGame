using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : UIBase
{
	[SerializeField]
	Image _backgroundImage;
	[SerializeField]
	TextMeshProUGUI _playerNameText;
	[SerializeField]
	TextMeshProUGUI _opponentNameText;
	[SerializeField]
	TextMeshProUGUI _countDownText;

	TextMeshProUGUI[] _texts;

	void Awake()
	{
		_texts = GetComponentsInChildren<TextMeshProUGUI>();
	}

	void OnEnable()
	{
		SetCountDownActive(true);
		_playerNameText.text = NetworkManager.Instance.Self.name;
		_opponentNameText.text = NetworkManager.Instance.Opponent.name;
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
		foreach (var text in _texts)
		{
			text.enabled = value;
		}
	}
}
