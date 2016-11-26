using UnityEngine;
using TMPro;

public class TImeAttackResult : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _clearTimeText;
	
	public void ShowResult(TimeAttackResultArgs args)
	{
		if (args.success)
		{
			_clearTimeText.text = args.time.ToString();
		}
		else
		{
			_clearTimeText.text = "miss";
		}
	}
}

public struct TimeAttackResultArgs
{
	public bool success;
	public float time;
}
