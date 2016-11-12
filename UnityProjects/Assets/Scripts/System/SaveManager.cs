using UnityEngine;
using System.Collections;

public static class SaveManager
{
	private static readonly string KEY_USERID = "Key_UserID";
	private static readonly string KEY_USERNAME = "Key_UserName";
	private static readonly string KEY_USERTOKEN = "Key_UserToken";

	public static bool ExistUser()
	{
		return PlayerPrefs.HasKey(KEY_USERID) && PlayerPrefs.HasKey(KEY_USERNAME) && PlayerPrefs.HasKey(KEY_USERTOKEN);
	}

	public static void SaveUser()
	{
		PlayerPrefs.SetInt(KEY_USERID, NetworkManager.Instance.Self.id);
		PlayerPrefs.SetString(KEY_USERNAME, NetworkManager.Instance.Self.name);
		PlayerPrefs.SetString(KEY_USERTOKEN, NetworkManager.Instance.Self.token);
		PlayerPrefs.Save();
	}

	public static UserInfo LoadUser()
	{
		return new UserInfo()
		{
			id = PlayerPrefs.GetInt(KEY_USERID),
			name = PlayerPrefs.GetString(KEY_USERNAME),
			token = PlayerPrefs.GetString(KEY_USERTOKEN),
		};
	}
}
