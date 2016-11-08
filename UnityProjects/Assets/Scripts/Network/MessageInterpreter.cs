using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

public class MessageInterpreter
{
	List<MessageArgs> _messageBuffer = new List<MessageArgs>();

	public void OnRecvMessage(object sender, MessageEventArgs e)
	{
		Debug.Log("Recv Message : " + e.Data);
		var recv = NetworkUtility.FromJson(e.Data);
		if (!recv.ContainsKey("type"))
		{
			var message = recv["message"] as Dictionary<string, object>;
			var args = new MessageArgs()
			{
				message = message,
				type = (string)message["type"],
				sender = sender,
				args = e,
			};
			_messageBuffer.Add(args);
		}
	}

	public void Update()
	{
		if (_messageBuffer.Any())
		{
			foreach (var info in _messageBuffer)
			{
				switch (info.type)
				{
					case "match":
						RecvMatchMessage(info.message, info.sender, info.args);
						break;
					case "step":
						RecvStepMessage(info.message, info.sender, info.args);
						break;
					case "fin":
						RecvFinishMessage(info.message, info.sender, info.args);
						break;
				}
			}
			_messageBuffer.Clear();
		}
	}

	void RecvMatchMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
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

	public struct MessageArgs
	{
		public string type;
		public Dictionary<string, object> message;
		public object sender;
		public MessageEventArgs args;
	}
}
