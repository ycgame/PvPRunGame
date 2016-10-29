using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	[SerializeField]
    Transform _player;
    Transform _opponent;
    bool _isNetwork;
    bool _isPlaying;

    TileManager _tileManager;

    void Awake()
    {
		_tileManager = GetComponentInChildren<TileManager>();
    }

	public void StartAI()
	{
		_isNetwork = false;
		StartGame();
	}

	public void StartNetwork()
	{
		_isNetwork = true;
		StartGame();
	}

	void StartGame()
	{
		_isPlaying = true;
		_tileManager.Initialize();
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

	void OnFailed(TapResult result, Vector2 tapPos)
	{
		FinishGame();
		SceneManager.LoadScene("Game");
	}

	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		Vector3 pos = _player.position;
		pos.x = result.x;
		_player.DOMove(pos, 0.05f);
	}

	void OnClear(TapResult result, Vector2 tapPos)
	{
		FinishGame();
		SceneManager.LoadScene("Game");
	}
}
