using UnityEngine;
using System.Collections;
using TMPro;

public class UI_Title : UIBase
{
	[SerializeField]
	TMP_InputField _userNameText;

	public void OnUpdateName(string name)
	{
		NetworkManager.Instance.Self.name = name;
		StartCoroutine( NetworkManager.Instance.NameUpdatePost(name) );
		SetName();
		SaveManager.SaveUser();
	}

	public void SetName()
	{
		_userNameText.text =  NetworkManager.Instance.Self.name;
	}
}
