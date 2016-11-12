using UnityEngine;
using TMPro;

public class UI_Result : UIBase
{
	[Header("タイムアタック")]
	[SerializeField]
	GameObject _timeAttackPanel;
	[SerializeField]
	TextMeshProUGUI _clearTimeText;

	[Header("オンライン")]
	[SerializeField]
	GameObject _onlinePanel;
	[SerializeField]
	TextMeshProUGUI _winnerText;

	public void ShowTimeAttackResult(TimeAttackResultArgs args)
	{
		_timeAttackPanel.SetActive(true);
		_onlinePanel.SetActive(false);
		if (args.success)
		{
			_clearTimeText.text = args.time.ToString();
		}
		else
		{
			_clearTimeText.text = "miss";
		}
	}

	public void ShowOnlineResult(OnlineResultArgs args)
	{
		_timeAttackPanel.SetActive(false);
		_onlinePanel.SetActive(true);
		_winnerText.text = args.winner == PlayerType.Player ? "win" : "lose";
	}
}

public struct TimeAttackResultArgs
{
	public bool success;
	public float time;
}

public struct OnlineResultArgs
{
	public PlayerType winner;
	public string opponentName;
	public float rate;
}