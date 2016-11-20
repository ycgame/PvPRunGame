using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UI_Title : UIBase
{
	[SerializeField]
	TMP_InputField _userNameText;
	[SerializeField]
	TextMeshProUGUI _rateText;
	[SerializeField]
	TextMeshProUGUI _rateRankText;
	[SerializeField]
	TextMeshProUGUI _timeText;
	[SerializeField]
	TextMeshProUGUI _timeRankText;

	public void OnUpdateName(string name)
	{
		NetworkManager.Instance.Self.name = name;
		StartCoroutine( NetworkManager.Instance.NameUpdatePost(name) );
		SetName();
		SaveManager.SaveUser();
	}

	public void SetName()
	{
		_userNameText.text =  NetworkManager.Instance.Self.name;
	}
	
	public void SetRank()
	{
		_timeText.text =  NetworkManager.Instance.Self.time_attack.ToString();
		_rateText.text =  NetworkManager.Instance.Self.rate.ToString();
		_timeRankText.text =  "??";
		_rateRankText.text =  "??";
		StartCoroutine(DisplayRanking());
	}
	
	IEnumerator DisplayRanking()
	{
		yield return NetworkManager.Instance.GetRankingInfo ();
		
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		var timeInfo = rankingInfo["time_attack"] as Dictionary<string, object>;
		var timeRank = NetworkUtility.ObjectToInt(timeInfo["rank"]);
		_timeRankText.text = timeRank.ToString()+"位";
		
		var rateInfo = rankingInfo["rate"] as Dictionary<string, object>;
		var rateRank = NetworkUtility.ObjectToInt(rateInfo["rank"]);
		_rateRankText.text = rateRank.ToString()+"位";
	}
	
	public void SetInfo()
	{
		SetName();
		SetRank();
	}
}
