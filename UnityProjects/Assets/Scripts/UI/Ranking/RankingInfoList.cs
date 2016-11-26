using UnityEngine;
using System.Collections.Generic;

public class RankingInfoList : MonoBehaviour 
{
	protected RankingInfo[] _rankInfoList;
	[SerializeField]
	float _margin = 30f;
	
	void Awake()
	{
		_rankInfoList = GetComponentsInChildren<RankingInfo>();
		int length = _rankInfoList.Length;
		for(int i = 0; i < length; i++)
		{
			_rankInfoList[i].GetComponent<RectTransform>().localPosition = ((float)(length-1)/2 - (i * _margin)) * Vector3.up;
		}
	}

	public virtual void SetRankingInfo(List<Dictionary<string, object>> list)
	{
	}
}
