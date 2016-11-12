using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField]
    Transform _player = null, _opponent = null;
	bool _isNetwork;
    bool _isPlaying;
    float _elapsedTime;

    TileManager _tileManager;

    void Awake()
    {
		Instance = this;
		_tileManager = GetComponentInChildren<TileManager>();
		_player.gameObject.SetActive(false);
		_opponent.gameObject.SetActive(false);
    }

	//タイムアタック
	public void StartTimeAttack()
	{
		_isNetwork = false;
		StartGame();
	}

	//通信対戦
	public void StartNetwork(int w, int[] stage)
	{
		_isNetwork = true;
		StartGame(w, stage);
	}

	//ステージ指定のゲームを開始
	void StartGame(int w, int[] stage)
	{
		_tileManager.Initialize(w, stage);
		OnStartGame();
	}

	//ランダムステージのゲームを開始
	void StartGame()
	{
		_tileManager.Initialize();
		OnStartGame();
	}

	//ゲームスタート時に絶対呼ばれる
	void OnStartGame()
	{
		_player.gameObject.SetActive(true);
		_opponent.gameObject.SetActive(_isNetwork);
		Camera.main.transform.position = 10f * Vector3.back;
		_player.localPosition = Vector3.zero;
		_opponent.localPosition = Vector3.zero;
		_elapsedTime = 0f;
		StartCoroutine(CountDown(3));
	}
	
	//カウントダウン
	IEnumerator CountDown(int count)
	{
		var ui = SceneController.Instance.GetUI<UI_InGame>(SceneController.UIType.InGame);
		SceneController.Instance.Show(SceneController.UIType.InGame, true);
		for (int c = count; c > 0; c--)
		{
			ui.SetCountDownText(c);
			yield return new WaitForSeconds(1f);
		}
		ui.ShowStage();
		_isPlaying = true;
	}

	//タップの挙動をとる
	void Update()
	{
		if (_isPlaying == false)
			return;

		_elapsedTime += Time.deltaTime;
		if (Utility.Input.TapDown)
		{
			Vector2 tapPos = Utility.Input.TapPosition01;
			var result = _tileManager.OnTapTile(tapPos);
			if (_isNetwork)
			{
				NetworkManager.Instance.SendStep(result.step);
			}
			switch (result.type)
			{
				case TapResult.Type.Failed:
					OnFailed(result, tapPos);
					break;
				case TapResult.Type.Success:
					OnSuccess(result, tapPos);
					break;
				case TapResult.Type.Clear:
					OnClear(result, tapPos);
					break;
			}
		}
	}

	//アバター動かす
	void MoveAvator(int step, int stepCnt, PlayerType type)
	{
		bool isPlayer = type == PlayerType.Player;
		Transform avator = isPlayer ? _player : _opponent;
		avator.DOMove(_tileManager.CalcTilePosition(step, stepCnt), 0.05f).SetEase(Ease.Linear);
		if (isPlayer)
		{
			Camera.main.transform.DOMoveY(_tileManager.CalcCameraY(stepCnt), 0.05f);
		}
	}

	//通信相手からのステップメッセージ来たとき
	public void OnStepOppopnent(int step, int stepCnt)
	{
		MoveAvator(step, stepCnt, PlayerType.Opponent);
	}

	//したとき呼ばれる
	public void OnFinishGame(PlayerType winner = PlayerType.Player)
	{
		_isPlaying = false;
		SceneController.Instance.Show(SceneController.UIType.Result, true);
		var ui = SceneController.Instance.GetUI<UI_Result>(SceneController.UIType.Result);
		if (_isNetwork)
		{
			ui.ShowOnlineResult(new OnlineResultArgs()
			{
				winner = winner,
			});
		}
		else
		{
			ui.ShowTimeAttackResult(new TimeAttackResultArgs()
			{
				success = winner == PlayerType.Player,
				time = _elapsedTime,
			});
		}
	}

	void OnFailed(TapResult result, Vector2 tapPos)
	{
		FinishGame(false);
	}

	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		if (_isNetwork == false)
		{
			NetworkManager.Instance.TimeUpdatePost(_elapsedTime);
		}
		MoveAvator(result.step, result.stepCnt, PlayerType.Player);
	}

	void OnClear(TapResult result, Vector2 tapPos)
	{
		MoveAvator(result.step, result.stepCnt, PlayerType.Player);
		FinishGame(true);
	}

	void FinishGame(bool success)
	{
		if (_isNetwork)
		{
			_isPlaying = false;
		}
		else
		{
			OnFinishGame(success ? PlayerType.Player : PlayerType.Opponent);
		}
	}
}


public enum PlayerType
{
	Player,
	Opponent,
}