using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TImeAttackResult : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _clearTimeText;
	[SerializeField]
	TextMeshProUGUI _bestTimeText;
	[SerializeField]
	TextMeshProUGUI _rankingText;
	[SerializeField]
	Image _newScoreImage;
	[SerializeField]
	Image _rankupImage;
	[SerializeField]
	GameObject _SuccessRoot;
	[SerializeField]
	GameObject _FailedRoot;
	
	public void ShowResult(TimeAttackResultArgs args)
	{
		if (args.success)
		{
			_SuccessRoot.SetActive(true);
			_FailedRoot.SetActive(false);
			_clearTimeText.text = args.time.ToString("f3");
		}
		else
		{
			_SuccessRoot.SetActive(false);
			_FailedRoot.SetActive(true);
			_clearTimeText.text = "----";
		}
		_bestTimeText.text = NetworkManager.Instance.Self.time_attack.ToString("f3");
		StartCoroutine(DisplayRanking(args));
	}

	IEnumerator DisplayRanking(TimeAttackResultArgs args)
	{
		_rankingText.text = "通信中";
		_rankupImage.enabled = false;
		_newScoreImage.enabled = false;
		yield return NetworkManager.Instance.GetRankingInfo ();
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		if(rankingInfo == null)
			yield break;

		var timeInfo = rankingInfo["time_attack"] as Dictionary<string, object>;
		var timeRank = NetworkUtility.ObjectToInt(timeInfo["rank"]);
		_rankingText.text = (timeRank+1).ToString();
		_newScoreImage.enabled = args.newScore;
		if (args.success)
		{
			_rankupImage.enabled = timeRank < GameManager.Instance.TimeRanking;
		}
		GameManager.Instance.TimeRanking = timeRank;
	}
}

public struct TimeAttackResultArgs
{
	public bool success;
	public bool newScore;
	public float time;
}
