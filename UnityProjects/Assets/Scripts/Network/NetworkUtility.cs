using System.Collections.Generic;
using MiniJSON;

public static class NetworkUtility
{
	public static Dictionary<string, object> FromJson(string jsonText)
	{
		return Json.Deserialize(jsonText) as Dictionary<string, object>;
	}

	public static string ToJson(object json)
	{
		return Json.Serialize(json);
	}
}
