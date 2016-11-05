using UnityEngine;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
	int _currentIndex;
	int[] _correctIndexes;
	[SerializeField]
	private int _length = 50;
	public int Length { get { return _length; } }
	[SerializeField]
	private int _width = 4;
	public int Width { get { return _width; } }
	[SerializeField]
	private int _height = 4;
	public int Height { get { return _height; } }
	[SerializeField]
	private GameObject _tilePrefab;
	private float _tileHeight;
	private float _tileWidth;

	private void CreateTiles()
	{
		float screenH = 2f * Camera.main.orthographicSize;
		float screenW = screenH * Camera.main.aspect;
		float h = screenH / (_height+1);
		float w = screenW / _width;
		float b = -h * ((float)_height / 2 - 1f);
		float l = -w * ((float)_width / 2 - 0.5f);
		for (int j = 0; j < _length; j++)
		{
			for (int i = 0; i < _width; i++) {
				GameObject tile = Instantiate(_tilePrefab) as GameObject;
				tile.transform.SetParent(this.transform);
				tile.transform.position = new Vector2(l+w*i, b+h*j);
				tile.transform.localScale = new Vector3(w, h, 1);
				if (_correctIndexes[j] == i)
				{
					tile.GetComponentInChildren<SpriteRenderer>().color = Color.black;
				}
			}
		}

		_tileHeight = h;
		_tileWidth = w;
	}


	public void Initialize()
	{
		_correctIndexes = new int[_length];
		for (int i = 0; i < _length; i++)
		{
			_correctIndexes[i] = Random.Range(0, _width);
		}
		CreateTiles();
	}

	public void Initialize(int w, int[] indexes)
	{
		_width = w;
		_correctIndexes = indexes;
		_length = indexes.Length;
		CreateTiles();
	}

	public TapResult OnTapTile(Vector2 tapPos)
	{
		int correct = _correctIndexes[_currentIndex];
		float start = (float)correct / _width;
		float end = (float)(correct + 1) / _width;
		float x = tapPos.x;
		int step = (int)(x * _width);
		Debug.Log(step);
		NetworkManager.Instance.SendStep(step);
		TapResult result = new TapResult();
		if (start <= x && x <= end)
		{
			_currentIndex++;
			if (_currentIndex == _length)
			{
				result.type = TapResult.Type.Clear;
			}
			else
			{
				transform.DOMoveY(-1 * _currentIndex * _tileHeight, 0.05f).SetEase(Ease.Linear);
				result.x = -_tileWidth * ((float)_width / 2 - 0.5f) + _tileWidth * correct;
				result.type = TapResult.Type.Success;
			}
		}
		else
		{
			result.type = TapResult.Type.Failed;
		}
		return result;
	}

}

public struct TapResult
{
	public float x;
	public Type type;

	public enum Type
	{
		Failed,
		Success,
		Clear,
	}
}
