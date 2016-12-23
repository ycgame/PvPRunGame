using UnityEngine;
using UnityEngine.UI;

public class OnlineResultParts : MonoBehaviour
{
	[SerializeField]
	ImageNumber _DiffNumber;
	[SerializeField]
	Image _RankUp;

	public void SetInfo(int diff)
	{
		_DiffNumber.SetNumber(Mathf.Abs(diff));
	}

	public void SetRankUpActive(bool value)
	{
		_RankUp.enabled = value;
	}
}
