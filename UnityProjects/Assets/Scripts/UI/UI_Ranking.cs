using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI_Ranking : UIBase
{
	[SerializeField]
	GameObject _RateContents;
	[SerializeField]
	GameObject _TimeContents;
	[SerializeField]
	RatingRankList _rateRankInfoList;
	[SerializeField]
	TimeAttackRankList _timeRankInfoList;
	int _timeRank;
	List<Dictionary<string, object>> _timeRank10 = new List<Dictionary<string, object>>(10);
	int _rateRank;
	List<Dictionary<string, object>> _rateRank10 = new List<Dictionary<string, object>>(10);

	void OnEnable()
	{
		StartCoroutine (DisplayRanking());
		ShowRateContents();
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}
	
	IEnumerator DisplayRanking()
	{
		yield return NetworkManager.Instance.GetRankingInfo ();
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		if(rankingInfo == null)
			yield break;
			
		var timeInfo = rankingInfo["time_attack"] as Dictionary<string, object>;
		_timeRank = NetworkUtility.ObjectToInt(timeInfo["rank"]);
		var timeTop10 = timeInfo["top10"] as List<object>;
		_timeRank10 = timeTop10.Select(x => x as Dictionary<string, object>).ToList();
		
		var rateInfo = rankingInfo["rate"] as Dictionary<string, object>;
		_rateRank = NetworkUtility.ObjectToInt(rateInfo["rank"]);
		var rateTop10 = rateInfo["top10"] as List<object>;
		_rateRank10 = rateTop10.Select(x => x as Dictionary<string, object>).ToList();

		_timeRankInfoList.SetRankingInfo(_timeRank10);
		_rateRankInfoList.SetRankingInfo(_rateRank10);
	}

	public void ShowRateContents()
	{
		_RateContents.SetActive(true);
		_TimeContents.SetActive(false);
	}

	public void ShowTimeContents()
	{
		_TimeContents.SetActive(true);
		_RateContents.SetActive(false);
	}
}
	
