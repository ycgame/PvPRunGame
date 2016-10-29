using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Net;

public class NetworkTest : MonoBehaviour 
{
	TestUser _testUser;
	WebSocket _ws;

	void Awake () 
	{
		//StartCoroutine (Get());
		StartCoroutine (UserCreatePost());
	}

	void Update()
	{
		if (Input.GetKeyUp("s"))
		{
			string subscribe = JsonUtility.ToJson (new TestSubscribe(), false);
			_ws.Send(subscribe);
		}

		if (Input.GetKeyUp("t"))
		{
			string auth = JsonUtility.ToJson (new TestAuth(_testUser), false);
			_ws.Send(auth);
		}

		if (Input.GetKeyUp("m"))
		{
			string move = JsonUtility.ToJson (new TestMove(new TestMoveData()), false);
			_ws.Send(move);
		}
	}

	IEnumerator GetTest()
	{
		string url = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/status";
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null) {
			Debug.Log(www.text);
		}
	}

	IEnumerator UserCreatePost()
	{
		string url = "http://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/users";
		WWWForm form = new WWWForm();
		string name = "test" + Random.Range (0, 1000);
		form.AddField("name", name);
		WWW www = new WWW(url, form);
		yield return www;
		if (www.error == null) {
			//Debug.Log(www.text);
			var json = www.text;
			_testUser = JsonUtility.FromJson(json, typeof(TestUser)) as TestUser;
			Debug.LogFormat("id = {0}", _testUser.id);
			Debug.LogFormat("name = {0}", _testUser.name);
			Debug.LogFormat("token = {0}", _testUser.token);
		}
		ConnectWebSocket ();
	}

	void ConnectWebSocket()
	{
		_ws = new WebSocket("ws://ec2-54-250-144-197.ap-northeast-1.compute.amazonaws.com:3000/cable");

		_ws.OnOpen += (sender, e) =>
		{
			Debug.Log("WebSocket Open");
		};

		_ws.OnMessage += (sender, e) =>
		{
			Debug.Log("WebSocket Message Data: " + e.Data);
		};

		_ws.OnError += (sender, e) =>
		{
			Debug.Log("WebSocket Error Message: " + e.Message);
		};

		_ws.OnClose += (sender, e) =>
		{
			Debug.Log("WebSocket Close");
		};

		_ws.Connect();
	}

	void OnDestroy()
	{
		_ws.Close();
		_ws = null;
	}
		
	class TestUser
	{
		public int id;
		public string name;
		public string token;
	}

	class TestChannel
	{
		public string channel = "DungeonChannel";
	}

	class TestSubscribe
	{
		public string command = "subscribe";
		public string identifier = JsonUtility.ToJson(new TestChannel(), false);
	}

	class TestAuthData
	{
		public string action;
		public int id;
		public string token;
	}

	class TestAuth
	{
		public string command;
		public string identifier;
		public string data;

		public TestAuth(TestUser user)
		{
			command = "message";
			identifier = JsonUtility.ToJson(new TestChannel(), false);
			var authdata = new TestAuthData()
			{
				action = "auth",
				id = user.id,
				token = user.token
			};
			data = JsonUtility.ToJson(authdata, false);
		}
	}

	class TestMoveData
	{
		public string action = "move";
		public float x;
		public float y;
		public float pos_x;
		public float pos_y;

		public TestMoveData()
		{
			x = Random.Range(-1f, 1f);
			y = Random.Range(-1f, 1f);
			pos_x = Random.Range(-1f, 1f);
			pos_y = Random.Range(-1f, 1f);
		}
	}

	class TestMove
	{
		public string command;
		public string identifier;
		public string data;

		public TestMove(TestMoveData movedata)
		{
			command = "message";
			identifier = JsonUtility.ToJson(new TestChannel(), false);
			data = JsonUtility.ToJson(movedata, false);
		}
	}
}