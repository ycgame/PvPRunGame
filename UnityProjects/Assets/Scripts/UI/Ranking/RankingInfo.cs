using UnityEngine;
using TMPro;

public class RankingInfo : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _rankText = null, _nameText = null, _valueText = null;

	public void SetText(int rank, string name, int iVal) 
	{
		SetRankAndNameText(rank, name);
		_valueText.text = iVal.ToString();
	}
	
	public void SetText(int rank, string name, float fVal)
	{
		SetRankAndNameText(rank, name);
		_valueText.text = fVal.ToString("f3");
	}


	void SetRankAndNameText(int rank, string name)
	{
		_rankText.text = rank.ToString();
		_nameText.text = name;
	}
}
