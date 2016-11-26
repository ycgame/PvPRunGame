using UnityEngine;
using TMPro;

public class RankingInfo : MonoBehaviour 
{
	[SerializeField]
	TextMeshProUGUI _rankText, _nameText, _valueText;

	public void SetText(int rank, string name, int iVal) 
	{
		SetRankAndNameText(rank, name);
		_valueText.text = iVal.ToString();
	}
	
	public void SetText(int rank, string name, float fVal)
	{
		SetRankAndNameText(rank, name);
		_valueText.text = fVal.ToString();
	}


	void SetRankAndNameText(int rank, string name)
	{
		_rankText.text = rank.ToString()+"位";
		_nameText.text = name;
	}
}
