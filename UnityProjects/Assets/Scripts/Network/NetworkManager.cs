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
	public UserInfo Opponent { get; set; }
	public WebSocket Socket { get; private set; }
	public Dictionary<string, object> RankingInfo { get; private set; }

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

		StartCoroutine(Initialize());
	}

	void Update()
	{
		_interpreter.Update();
	}


	public void CreateMatch()
	{
		string match = JsonUtility.ToJson (new MatchMessage(Self), false);
		Socket.Send(match);
	}

	public void SendStep(int step)
	{
		string stepJson = JsonUtility.ToJson (new StepMessage(step), false);
		Socket.Send(stepJson);
	}

	public void SendCancel()
	{
		string cancelJson = JsonUtility.ToJson (new CancelMessage(), false);
		Socket.Send(cancelJson);
	}

	IEnumerator Initialize()
	{
		if (SaveManager.ExistUser())
		{
			Self = SaveManager.LoadUser();
			yield return null;
		}
		else
		{
			yield return UserCreatePost();
			SaveManager.SaveUser();
		}

		ConnectWebSocket ();

		string subscribe = JsonUtility.ToJson (new Subscribe(), false);
		Socket.Send(subscribe);

		SceneController.Instance.Initialize();
	}

	IEnumerator UserCreatePost()
	{
		string url = GetURL("users");
		WWWForm form = new WWWForm();
		string token = "Taichiro0709NaotoSasaki";
		form.AddField("token", token);
		WWW www = new WWW(url, form);
		yield return www;

		if (www.error == null) {
			var json = www.text;
			Self = JsonUtility.FromJson(json, typeof(UserInfo)) as UserInfo;
			Debug.Log (json);
		}
	}

	public IEnumerator GetRankingInfo()
	{
		//http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/users/157/ranking?token=token
		string url = GetURL("users/"+Self.id.ToString()+"/ranking?token="+Self.token);
		WWW www = new WWW(url);
		yield return www;

		if (www.error == null) {
			var json = www.text;
			RankingInfo = NetworkUtility.FromJson (www.text);
		}
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

	public IEnumerator NameUpdatePost(string name)
	{
		string url = GetURL("users/"+Self.id.ToString()+"/name");
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("token", Self.token);
		WWW www = new WWW(url, form);
		yield return www;
	}

	public IEnumerator TimeUpdatePost(float time)
	{
		string url = GetURL("users/"+Self.id.ToString()+"/time_attack");
		WWWForm form = new WWWForm();
		form.AddField("time_attack", time.ToString());
		form.AddField("token", Self.token);
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
	public float rate = -1;
	public float time_attack = -1;
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

public class MatchMessage : MessageBase
{
	public MatchMessage(UserInfo userInfo)
	{
		data = JsonUtility.ToJson(new Data(userInfo), false);
	}	

	public class Data
	{
		public int id;
		public string token;
		public string action = "match";

		public Data(UserInfo userInfo)
		{	
			id = userInfo.id;
			token = userInfo.token;
		}
	}
}

public class StepMessage : MessageBase
{
	public StepMessage(int s)
	{
		data = JsonUtility.ToJson(new Data(s), false);
	}

	public class Data
	{
		public int step;
		public string action = "step";

		public Data(int s)
		{
			step = s;
		}
	}
}

public class CancelMessage : MessageBase
{
	public CancelMessage()
	{
		data = JsonUtility.ToJson(new Data(), false);
	}

	public class Data
	{
		public string action = "cancel";
	}
}

public class MessageBase
{
	public string data;
	public string command = "message";
	public string identifier = JsonUtility.ToJson(new Channel(), false);
}