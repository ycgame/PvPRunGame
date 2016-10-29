using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField]
    Transform _player;

    TileManager _tileManager;

    void Awake()
    {
		Utility.Input.Initialize();
		_tileManager = GetComponentInChildren<TileManager>();
		_tileManager.Initialize();
    }

	void Update()
	{
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
		SceneManager.LoadScene("Game");
	}

	void OnSuccess(TapResult result, Vector2 tapPos)
	{
		Vector3 pos = _player.position;
		pos.x = result.x;
		_player.position = pos;
	}

	void OnClear(TapResult result, Vector2 tapPos)
	{
		SceneManager.LoadScene("Game");
	}
}
