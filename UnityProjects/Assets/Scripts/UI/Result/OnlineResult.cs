using UnityEngine;
using TMPro;

public class OnlineResult : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _winnerText;
	
	public void ShowResult(OnlineResultArgs args)
	{
		_winnerText.text = args.winner == PlayerType.Player ? "win" : "lose";
	}
}
