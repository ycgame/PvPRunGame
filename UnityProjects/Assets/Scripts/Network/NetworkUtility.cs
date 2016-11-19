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

	public static int ObjectToInt(object o)
	{
		int res = -1;
		if (int.TryParse(o.ToString(), out res) == false)
		{
			UnityEngine.Debug.LogError("Error");
		}
		return res;
	}

	public static bool ObjectToBool(object o)
	{
		bool res = false;
		if (bool.TryParse(o.ToString(), out res) == false)
		{
			UnityEngine.Debug.LogError("Error");
		}
		return res;
	}
}

public class MyJson : Dictionary<string, object>
{
}