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
			var message = recv["message"] as MyJson;
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

	void RecvMatchMessage(MyJson message, object sender, MessageEventArgs e)
	{
		var stage = message["stage"] as List<object>;
		var opponentInfo = message["matched"] as MyJson;
		NetworkManager.Instance.Opponent = new UserInfo()
		{
			id = -1,
			name = (string)opponentInfo["name"],
			token = "dummy",
		};
		SceneController.Instance.KillAIRequest();
		SceneController.Instance.OnCreateTile(4, stage.Select(x => NetworkUtility.ObjectToInt(x)).ToArray());
	}

	void RecvStepMessage(MyJson message, object sender, MessageEventArgs e)
	{
		int step = NetworkUtility.ObjectToInt(message["step"]);
		int stepCnt = NetworkUtility.ObjectToInt(message["step_count"]);
		GameManager.Instance.OnStepOppopnent(step, stepCnt);
	}

	void RecvFinishMessage(MyJson message, object sender, MessageEventArgs e)
	{
		bool win = NetworkUtility.ObjectToBool(message["fin"]);
		PlayerType winner = win ? PlayerType.Player : PlayerType.Opponent;
		GameManager.Instance.OnFinishGame(winner);
	}

	public struct MessageInfo
	{
		public string type;
		public MyJson message;
		public object sender;
		public MessageEventArgs args;
	}
}
