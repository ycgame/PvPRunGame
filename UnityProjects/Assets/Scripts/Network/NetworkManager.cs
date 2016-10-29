using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;

public static class NetworkManager
{
	private static UserInfo _self;
	private static WebSocket _socket;

	public static IEnumerator UserCreatePost()
	{
		string url = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/users";
		WWWForm form = new WWWForm();
		string name = "test" + Random.Range (0, 1000);
		form.AddField("name", name);
		WWW www = new WWW(url, form);
		yield return www;
		if (www.error == null) {
			var json = www.text;
			_self = JsonUtility.FromJson(json, typeof(UserInfo)) as UserInfo;
			Debug.LogFormat("id = {0}", _self.id);
			Debug.LogFormat("name = {0}", _self.name);
			Debug.LogFormat("token = {0}", _self.token);
		}

		ConnectWebSocket ();
	}

	public static void ConnectWebSocket()
	{
		_socket = new WebSocket("ws://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/cable");

		_socket.OnOpen += (sender, e) =>
		{
			Debug.Log("WebSocket Open");
		};

		_socket.OnMessage += (sender, e) =>
		{
			Debug.Log("WebSocket Message Data: " + e.Data);
		};

		_socket.OnError += (sender, e) =>
		{
			Debug.Log("WebSocket Error Message: " + e.Message);
		};

		_socket.OnClose += (sender, e) =>
		{
			Debug.Log("WebSocket Close");
		};

		_socket.Connect();
	}

	public static void OnDestroy()
	{
		_socket.Close();
		_socket = null;
	}
}

public class UserInfo
{
	public int id;
	public string name;
	public string token;
}