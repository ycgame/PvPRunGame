using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using MiniJSON;

public class NetworkManager : MonoBehaviour
{
	private readonly string URL = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/";
	private readonly string WSURL = "ws://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/";
	public static NetworkManager Instance { get; private set; }

	public UserInfo Self { get; private set; }
	public WebSocket Socket { get; private set; }

	private MessageInterpreter _interpreter = new MessageInterpreter();

	public string GetURL(string suffix)
	{
		return URL+suffix;
	}

	public string GetWebSocketURL(string suffix)
	{
		return WSURL+suffix;
	}

	void Awake()
	{
		Instance = this;

		StartCoroutine(UserCreatePost());
	}

	void Update()
	{
		_interpreter.Update();
	}

	public IEnumerator UserCreatePost()
	{
		string url = GetURL("users");
		WWWForm form = new WWWForm();
		string name = "test" + Random.Range (0, 1000);
		form.AddField("name", name);
		WWW www = new WWW(url, form);
		yield return www;

		if (www.error == null) {
			var json = www.text;
			Self = JsonUtility.FromJson(json, typeof(UserInfo)) as UserInfo;
		}

		ConnectWebSocket ();

		string subscribe = JsonUtility.ToJson (new Subscribe(), false);
		Socket.Send(subscribe);
	}

	public void CreateMatch()
	{
		string match = JsonUtility.ToJson (new Match(Self), false);
		Socket.Send(match);
	}

	public void SendStep(int step)
	{
		string stepJson = JsonUtility.ToJson (new Step(step), false);
		Socket.Send(stepJson);
	}

	public IEnumerator AIRequestPost()
	{
		string url = GetURL("ais");
		WWWForm form = new WWWForm();
		string token = "Taichiro0709NaotoSasaki";
		form.AddField("token", token);
		WWW www = new WWW(url, form);
		yield return www;
	}

	public void ConnectWebSocket()
	{
		Socket = new WebSocket(GetWebSocketURL("cable"));

		Socket.OnOpen += (sender, e) =>
		{
			Debug.Log("WebSocket Open");
		};

		Socket.OnMessage += (sender, e) =>
		{
			_interpreter.OnRecvMessage(sender, e);
		};

		Socket.OnError += (sender, e) =>
		{
			Debug.Log("WebSocket Error Message: " + e.Message);
		};

		Socket.OnClose += (sender, e) =>
		{
			Debug.Log("WebSocket Close");
		};

		Socket.Connect();
	}

	public void OnDestroy()
	{
		Socket.Close();
		Socket = null;
	}
}

public class UserInfo
{
	public int id;
	public string name;
	public string token;
}

public class Subscribe
{
	public string command = "subscribe";
	public string identifier = JsonUtility.ToJson(new Channel(), false);
}

public class Channel
{
	public string channel = "MatchChannel";
}

public class Match
{
	public string data;
	public string command = "message";
	public string identifier = JsonUtility.ToJson(new Channel(), false);

	public Match(UserInfo userInfo)
	{
		data = JsonUtility.ToJson(new MatchData(userInfo), false);
	}

	public class MatchData
	{
		public int id;
		public string token;
		public string action = "match";

		public MatchData(UserInfo userInfo)
		{
			id = userInfo.id;
			token = userInfo.token;
		}
	}
}

public class Step
{
	public string data;
	public string command = "message";
	public string identifier = JsonUtility.ToJson(new Channel(), false);

	public Step(int s)
	{
		data = JsonUtility.ToJson(new StepData(s), false);
	}

	public class StepData
	{
		public int step;
		public string action = "step";

		public StepData(int s)
		{
			step = s;
		}
	}
}
