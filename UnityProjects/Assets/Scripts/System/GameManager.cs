using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField]
    Transform _player, _opponent;
	bool _isNetwork;
    bool _isPlaying;

    TileManager _tileManager;

    void Awake()
    {
		Instance = this;
		_tileManager = GetComponentInChildren<TileManager>();
		_player.gameObject.SetActive(false);
		_opponent.gameObject.SetActive(false);
    }

	public void StartAI()
	{
		_isNetwork = false;
		StartGame();
	}

	public void StartNetwork(int w, int[] stage)
	{
		_isNetwork = true;
		StartGame(w, stage);
	}

	void StartGame(int w, int[] stage)
	{
		OnStartGame();
		_tileManager.Initialize(w, stage);
	}

	void StartGame()
	{
		OnStartGame();
		_tileManager.Initialize();
	}

	void OnStartGame()
	{
		_player.gameObject.SetActive(true);
		_opponent.gameObject.SetActive(_isNetwork);
		_isPlaying = true;
	}

	void Update()
	{
		if (_isPlaying == false)
			return;

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

	void MoveAvator(int step, int stepCnt, bool isPlayer)
	{
		Transform avator = isPlayer ? _player : _opponent;
		avator.DOMove(_tileManager.CalcTilePosition(step, stepCnt), 0.05f).SetEase(Ease.Linear);
		if (isPlayer)
		{
			Camera.main.transform.DOMoveY(_tileManager.CalcCameraY(stepCnt), 0.05f);
		}
	}

	public void OnStepOppopnent(int step, int stepCnt)
	{
		MoveAvator(step, stepCnt, false);
	}

	public void OnFinishGame()
	{
		_isPlaying = false;
		SceneController.Instance.Show(SceneController.UIType.Result, true);
	}

	void OnFailed(TapResult result, Vector2 tapPos)
	{
		FinishGame();
	}

	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		MoveAvator(result.step, result.stepCnt, true);
	}

	void OnClear(TapResult result, Vector2 tapPos)
	{
		MoveAvator(result.step, result.stepCnt, true);
		FinishGame();
	}

	void FinishGame()
	{
		if (_isNetwork)
		{
			_isPlaying = false;
		}
		else
		{
			OnFinishGame();
		}
	}
}
