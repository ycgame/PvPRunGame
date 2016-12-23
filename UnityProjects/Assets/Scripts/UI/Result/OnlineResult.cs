using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnlineResult : MonoBehaviour 
{
	[SerializeField]
	OnlineResultParts _WinParts;
	[SerializeField]
	OnlineResultParts _LoseParts;
	[SerializeField]
	TextMeshProUGUI _factorText = null, _rateText = null, _rankingText = null;
	
	public void ShowResult(OnlineResultArgs args)
	{
		_WinParts.gameObject.SetActive(args.winner == PlayerType.Player);
		_LoseParts.gameObject.SetActive(args.winner != PlayerType.Player);
		var parts = args.winner == PlayerType.Player ? _WinParts : _LoseParts;
		_factorText.color = args.winner == PlayerType.Player ? new Color(0f/255f, 101f/255f, 172f/255f) : new Color(157f/255f, 191f/255f, 214f/255f);
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
		_rateText.text = args.rate.ToString();
		var diff = args.rate - args.prevRate;
		parts.SetInfo(diff);
		StartCoroutine(DisplayRanking(args, parts));
	}

	IEnumerator DisplayRanking(OnlineResultArgs args, OnlineResultParts parts)
	{
		parts.SetRankUpActive(false);
		_rankingText.text = "通信中";
		yield return NetworkManager.Instance.GetRankingInfo ();
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		if(rankingInfo == null)
			yield break;

		var rateInfo = rankingInfo["rate"] as Dictionary<string, object>;
		var rateRank = NetworkUtility.ObjectToInt(rateInfo["rank"]);
		_rankingText.text = (rateRank+1).ToString();
		if (args.winner == PlayerType.Player)
		{
			parts.SetRankUpActive(rateRank < GameManager.Instance.RateRanking);
		}
		else
		{
			parts.SetRankUpActive(rateRank > GameManager.Instance.RateRanking);
		}
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