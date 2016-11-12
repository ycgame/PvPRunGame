using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

public class MessageInterpreter
{
	List<MessageInfo> _messageBuffer = new List<MessageInfo>();

	public void OnRecvMessage(object sender, MessageEventArgs e)
	{
		Debug.Log("Recv Message : " + e.Data);
		var recv = NetworkUtility.FromJson(e.Data);
		if (!recv.ContainsKey("type"))
		{
			//Debug.Log("Recv Message : " + e.Data);
			var message = recv["message"] as Dictionary<string, object>;
			var args = new MessageInfo()
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
		SceneController.Instance.OnCreateTile(4, stage.Select(x => NetworkUtility.ObjectToInt(x)).ToArray());
	}

	void RecvStepMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
		int step = NetworkUtility.ObjectToInt(message["step"]);
		int stepCnt = NetworkUtility.ObjectToInt(message["step_count"]);
		GameManager.Instance.OnStepOppopnent(step, stepCnt);
	}

	void RecvFinishMessage(Dictionary<string, object> message, object sender, MessageEventArgs e)
	{
		GameManager.Instance.OnFinishGame();
	}

	public struct MessageInfo
	{
		public string type;
		public Dictionary<string, object> message;
		public object sender;
		public MessageEventArgs args;
	}
}
