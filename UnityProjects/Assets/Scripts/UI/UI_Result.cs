using UnityEngine;
using System.Collections;

public class UI_Result : UIBase
{
	public void ShowTimeAttackResult(TimeAttackResultArgs args)
	{
	}

	public void ShowOnlineResult(OnlineResultArgs args)
	{
	}
}

public struct TimeAttackResultArgs
{
	public float time;
}

public struct OnlineResultArgs
{
	public PlayerType winner;
	public string opponentName;
	public float rate;
}