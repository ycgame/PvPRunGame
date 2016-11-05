using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Net;
using MiniJSON;

public class NetworkManager : MonoBehaviour
{
	public static NetworkManager Instance { get; private set; }

	public UserInfo Self { get; private set; }
	public WebSocket Socket { get; private set; }

	void Awake()
	{
		Instance = this;

		StartCoroutine(UserCreatePost());
	}

	Dictionary<string, object> FromJson(string jsonText)
	{
		return Json.Deserialize(jsonText) as Dictionary<string, object>;
	}

	string ToJson(object json)
	{
		return Json.Serialize(json);
	}

	public IEnumerator UserCreatePost()
	{
		string url = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/users";
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
		string url = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/ais";
		WWWForm form = new WWWForm();
		string token = "Taichiro0709NaotoSasaki";
		form.AddField("token", token);
		WWW www = new WWW(url, form);
		yield return www;
	}

	public void ConnectWebSocket()
	{
		Socket = new WebSocket("ws://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/cable");

		Socket.OnOpen += (sender, e) =>
		{
			Debug.Log("WebSocket Open");
		};

		Socket.OnMessage += (sender, e) =>
		{
			OnMessage(sender, e);
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

	void OnMessage(object sender, MessageEventArgs e)
	{
		var recv = FromJson(e.Data);
		if (!recv.ContainsKey("type"))
		{
			Debug.Log(e.Data);
			var message = recv["message"] as Dictionary<string, object>;
			var msgType = (string)message["type"];
			if (msgType == "match")
			{
				RecvMatchMessage(message, sender, e);
			}
			else if (msgType == "step")
			{
				RecvStepMessage(message, sender, e);
			}
			else if (msgType == "fin")
			{
				RecvFinishMessage(message, sender, e);
			}
		}
	}

	void RecvMatchMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
		Debug.Log(message["stage"]);
		var stage = message["stage"] as List<object>;
		SceneController.Instance.OnCreateTile(4, stage.Select(x => int.Parse(x.ToString())).ToArray());
	}

	void RecvStepMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
		Debug.Log(message["step"]);
		Debug.Log(message["step_count"]);
	}

	void RecvFinishMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
		Debug.Log(message["fin"]);
		Debug.Log(message["user"]);
		Debug.Log(message["matched"]);
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
