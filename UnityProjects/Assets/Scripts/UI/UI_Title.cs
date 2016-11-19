using UnityEngine;
using System.Collections;
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
		_timeRankText.text =  "123位";
		_rateRankText.text =  "453位";
	}
	
	public void SetInfo()
	{
		SetName();
		SetRank();
	}
}
