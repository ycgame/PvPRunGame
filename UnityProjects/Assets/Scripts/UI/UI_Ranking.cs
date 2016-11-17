using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Ranking : UIBase
{
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
