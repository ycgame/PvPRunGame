﻿using UnityEngine;
using UnityEngine.UI;
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
	TextMeshProUGUI _volumeText;
	[SerializeField]
	Image _volumeOn;
	[SerializeField]
	Image _volumeOff;

	void Start()
	{
		SetVolumeButton();
	}
	
	void OnEnable()
	{
		SetInfo();
	}

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
		_rateText.text =  NetworkManager.Instance.Self.rate.ToString();
		_rateRankText.text = "通信中";
		StartCoroutine(DisplayRanking());
	}
	
	IEnumerator DisplayRanking()
	{
		yield return NetworkManager.Instance.GetRankingInfo ();
		
		var rankingInfo = NetworkManager.Instance.RankingInfo;
		if(rankingInfo == null)
			yield break;
			
		var timeInfo = rankingInfo["time_attack"] as Dictionary<string, object>;
		GameManager.Instance.TimeRanking = NetworkUtility.ObjectToInt(timeInfo["rank"]);

		var rateInfo = rankingInfo["rate"] as Dictionary<string, object>;
		GameManager.Instance.RateRanking = NetworkUtility.ObjectToInt(rateInfo["rank"]);
		_rateRankText.text = (GameManager.Instance.RateRanking+1).ToString();
	}
	
	public void SetInfo()
	{
		SetName();
		SetRank();
	}

	public void OnVolume()
	{
		SoundManager.Instance.SwitchVolume();
		SetVolumeButton();
	}

	void SetVolumeButton()
	{
		_volumeText.text = SoundManager.Instance.IsEnabled ? "ON": "OFF";
		_volumeOn.enabled = SoundManager.Instance.IsEnabled;
		_volumeOff.enabled = SoundManager.Instance.IsEnabled == false;
	}
}
