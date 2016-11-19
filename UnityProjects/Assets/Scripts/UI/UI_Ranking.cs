using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Ranking : UIBase
{
	int _timeRank;
	List<MyJson> _timeRank10 = new List<MyJson>(10);
	int _rateRank;
	List<MyJson> _rateRank10 = new List<MyJson>(10);
	
	void OnEnable()
	{
		StartCoroutine (DisplayRanking());
	}
	
	IEnumerator DisplayRanking()
	{
		yield return NetworkManager.Instance.GetRankingInfo ();
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		Debug.Log (rankingInfo);
	}
}
