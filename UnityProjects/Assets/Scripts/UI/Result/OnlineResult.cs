using UnityEngine;
using TMPro;

public class OnlineResult : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _winnerText, _factorText, _prevRateText, _diffText, _rateText;
	
	public void ShowResult(OnlineResultArgs args)
	{
		_winnerText.text = args.winner == PlayerType.Player ? "勝利！！" : "敗北...";
		switch(args.factor)
		{
			case "miss":
				if(args.winner == PlayerType.Player)
					_factorText.text = "相手のミスで勝利しました";
				else
					_factorText.text = "自分のミスで敗北しました";
				break;
			case "goal":
				if(args.winner == PlayerType.Player)
					_factorText.text = "自分が先にゴールしました";
				else
					_factorText.text = "相手が先にゴールしました";
				break;
		}
		_prevRateText.text = args.prevRate.ToString();
		_rateText.text = args.rate.ToString();
		var diff = args.rate - args.prevRate;
		var sign = diff > 0 ? "+" : "-";
		_diffText.text = sign + diff.ToString();
	}
}

public struct OnlineResultArgs
{
	public PlayerType winner;
	public string factor;
	public string opponentName;
	public int rate;
	public int prevRate;
}