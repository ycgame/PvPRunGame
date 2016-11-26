using UnityEngine;
using System.Collections.Generic;

public class RatingRankList : RankingInfoList 
{
	public override void SetRankingInfo(List<Dictionary<string, object>> list)
	{
        for(int i = 0; i < Mathf.Min(list.Count, _rankInfoList.Length); i++)
        {
            var info = list[i];
            var rank = _rankInfoList[i];
            var name = (string)info["name"];
            var rate = NetworkUtility.ObjectToInt(info["rate"]);
            rank.SetText(i+1, name, rate);
        }
	}
}
