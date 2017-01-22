using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class UI_Ranking : UIBase
{
	[SerializeField]
	GameObject _RateContents;
	[SerializeField]
	GameObject _TimeContents;
	[SerializeField]
	TextMeshProUGUI _SelfRate;
	[SerializeField]
	TextMeshProUGUI _SelfTime;
	[SerializeField]
	RatingRankList _rateRankInfoList;
	[SerializeField]
	TimeAttackRankList _timeRankInfoList;
	List<Dictionary<string, object>> _timeRank10 = new List<Dictionary<string, object>>(10);
	List<Dictionary<string, object>> _rateRank10 = new List<Dictionary<string, object>>(10);
	bool _IsShowRate = true;

	void OnEnable()
	{
		StartCoroutine (DisplayRanking());
		ShowContents();
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
		_SelfTime.text = NetworkManager.Instance.Self.time_attack.ToString("f3");
		var timeTop10 = timeInfo["top10"] as List<object>;
		_timeRank10 = timeTop10.Select(x => x as Dictionary<string, object>).ToList();
		
		var rateInfo = rankingInfo["rate"] as Dictionary<string, object>;
		_SelfRate.text = NetworkManager.Instance.Self.rate.ToString();
		var rateTop10 = rateInfo["top10"] as List<object>;
		_rateRank10 = rateTop10.Select(x => x as Dictionary<string, object>).ToList();

		_timeRankInfoList.SetRankingInfo(_timeRank10);
		_rateRankInfoList.SetRankingInfo(_rateRank10);
	}

	void ShowContents()
	{
		_RateContents.SetActive(_IsShowRate);
		_TimeContents.SetActive(!_IsShowRate);
	}

	public void SwichShowContents()
	{
		_IsShowRate = !_IsShowRate;
		ShowContents();
	}
}
	
