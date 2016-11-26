using UnityEngine;

public class UI_Result : UIBase
{
	[Header("タイムアタック")]
	[SerializeField]
	TImeAttackResult _timeAttackPanel;

	[Header("オンライン")]
	[SerializeField]
	OnlineResult _onlinePanel;

	public void ShowTimeAttackResult(TimeAttackResultArgs args)
	{
		_timeAttackPanel.gameObject.SetActive(true);
		_onlinePanel.gameObject.SetActive(false);
		_timeAttackPanel.ShowResult(args);
	}

	public void ShowOnlineResult(OnlineResultArgs args)
	{
		_timeAttackPanel.gameObject.SetActive(false);
		_onlinePanel.gameObject.SetActive(true);
		_onlinePanel.ShowResult(args);
	}
}