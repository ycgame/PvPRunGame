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
					SceneManager.LoadScene("Game");
					break;
				case TapResult.Type.Success:
					Vector3 pos = _player.position;
					pos.x = result.x;
					_player.position = pos;
					break;
				case TapResult.Type.Clear:
					SceneManager.LoadScene("Game");
					break;
			}
		}
	}
}
