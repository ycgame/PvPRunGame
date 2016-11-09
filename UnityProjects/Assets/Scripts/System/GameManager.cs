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
		_opponent.gameObject.SetActive(_isNetwork);
		OnStartGame();
		_tileManager.Initialize();
	}

	void OnStartGame()
	{
		_isPlaying = true;
	}

	public void FinishGame()
	{
		_isPlaying = false;
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

	public void MoveAvator(int step, int stepCnt, bool isPlayer)
	{
		Transform avator = isPlayer ? _player : _opponent;
		avator.DOMove(_tileManager.CalcTilePosition(step, stepCnt), 0.05f).SetEase(Ease.Linear);
	}

	void OnFailed(TapResult result, Vector2 tapPos)
	{
		FinishGame();
		//SceneManager.LoadScene("Game");
	}

	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		MoveAvator(result.step, result.stepCnt, true);
		Camera.main.transform.DOMoveY(_tileManager.CalcCameraY(result.stepCnt), 0.05f);
	}

	void OnClear(TapResult result, Vector2 tapPos)
	{
		FinishGame();
		//SceneManager.LoadScene("Game");
	}
}
