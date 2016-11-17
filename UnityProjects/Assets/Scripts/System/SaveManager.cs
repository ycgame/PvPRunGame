using UnityEngine;
using System.Collections;

public static class SaveManager
{
	private static readonly string KEY_USERID = "Key_UserID";
	private static readonly string KEY_USERNAME = "Key_UserName";
	private static readonly string KEY_USERTOKEN = "Key_UserToken";
	private static readonly string KEY_USERRATE = "Key_UserRate";
	private static readonly string KEY_USERTIME = "Key_UserTime";

	public static bool ExistUser()
	{
		return PlayerPrefs.HasKey(KEY_USERID)
			&& PlayerPrefs.HasKey(KEY_USERNAME) 
			&& PlayerPrefs.HasKey(KEY_USERTOKEN)
			&& PlayerPrefs.HasKey(KEY_USERRATE)
			&& PlayerPrefs.HasKey(KEY_USERTIME);
	}

	public static void SaveUser()
	{
		PlayerPrefs.SetInt(KEY_USERID, NetworkManager.Instance.Self.id);
		PlayerPrefs.SetString(KEY_USERNAME, NetworkManager.Instance.Self.name);
		PlayerPrefs.SetString(KEY_USERTOKEN, NetworkManager.Instance.Self.token);
		PlayerPrefs.SetString(KEY_USERRATE, NetworkManager.Instance.Self.rate);
		PlayerPrefs.SetString(KEY_USERTIME, NetworkManager.Instance.Self.time_attack);
		PlayerPrefs.Save();
	}

	public static UserInfo LoadUser()
	{
		return new UserInfo()
		{
			id = PlayerPrefs.GetInt(KEY_USERID),
			name = PlayerPrefs.GetString(KEY_USERNAME),
			token = PlayerPrefs.GetString(KEY_USERTOKEN),
			rate = PlayerPrefs.GetString(KEY_USERRATE),
			time_attack = PlayerPrefs.GetString(KEY_USERTIME),
		};
	}
}
