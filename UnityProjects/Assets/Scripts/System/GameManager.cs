using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField]
    Transform _player = null, _opponent = null;
	public bool IsNetwork { get; private set; }
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
		IsNetwork = false;
		StartGame();
	}

	//通信対戦
	public void StartNetwork(int w, int[] stage)
	{
		IsNetwork = true;
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
		_opponent.gameObject.SetActive(IsNetwork);
		_player.localPosition = Vector3.zero;
		_opponent.localPosition = Vector3.zero;
		_player.localScale = _tileManager.CalcTileScale();
		_opponent.localScale = _tileManager.CalcTileScale();

		Camera.main.transform.position = 10f * Vector3.back;

		_elapsedTime = 0f;
		StartCoroutine(CountDown(3));
	}
	
	//カウントダウン
	IEnumerator CountDown(int count)
	{
		SoundManager.Instance.StopBGM();
		var ui = SceneController.Instance.GetUI<UI_InGame>(SceneController.UIType.InGame);
		SceneController.Instance.Show(SceneController.UIType.InGame, true);
		for (int c = count; c > 0; c--)
		{
			ui.SetCountDownText(c);
			SoundManager.Instance.PlaySE(SEType.CountDown);
			yield return new WaitForSeconds(1f);
		}
		ui.ShowStage();
		_isPlaying = true;
		SoundManager.Instance.PlayBGM(BGMType.InGame);
	}

	//タップの挙動をとる
	void Update()
	{
		if (_isPlaying == false)
			return;

		_elapsedTime += Time.deltaTime;
		for (int i = 0; i < 4; i++)
		{
			if (Utility.Input.GetTapDown(i))
			{
				Vector2 tapPos = Utility.Input.GetTapPosition01(i);
				var result = _tileManager.OnTapTile(tapPos);
				if (IsNetwork)
				{
					NetworkManager.Instance.SendStep(result.step);
				}
				MoveAvator(result.step, result.stepCnt, PlayerType.Player);
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

	//終了したとき呼ばれる
	public void OnFinishGame(PlayerType winner = PlayerType.Player)
	{
		SoundManager.Instance.StopBGM();
		_isPlaying = false;
		StartCoroutine(FinishCoroutine(winner));
	}

	IEnumerator FinishCoroutine(PlayerType winner)
	{
		yield return new WaitForSeconds(1f);
		SoundManager.Instance.PlaySE(winner == PlayerType.Player ? SEType.Win : SEType.Lose);
		SceneController.Instance.Show(SceneController.UIType.Result, true);
		var ui = SceneController.Instance.GetUI<UI_Result>(SceneController.UIType.Result);
		if (IsNetwork)
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
		yield return new WaitForSeconds(1.5f);
		SoundManager.Instance.PlayBGM(BGMType.Menu);
	}

	//タップ失敗
	void OnFailed(TapResult result, Vector2 tapPos)
	{
		SoundManager.Instance.PlaySE(SEType.FailTap);
		FinishGame(false);
	}

	//タップ成功
	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		SoundManager.Instance.PlaySE(SEType.CorrectTap);
	}

	//ゴール
	void OnClear(TapResult result, Vector2 tapPos)
	{
		SoundManager.Instance.PlaySE(SEType.CorrectTap);
		if (IsNetwork == false)
		{
			if (NetworkManager.Instance.Self.time_attack > _elapsedTime) 
			{
				NetworkManager.Instance.Self.time_attack = _elapsedTime;
				NetworkManager.Instance.TimeUpdatePost(_elapsedTime);
				SaveManager.SaveUser ();
				var title = SceneController.Instance.GetUI<UI_Title>(SceneController.UIType.Titie);
				title.SetRank();
			}
		}
		FinishGame(true);
	}

	void FinishGame(bool success)
	{
		if (IsNetwork)
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