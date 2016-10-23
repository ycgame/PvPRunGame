using UnityEngine;

public class TileManager : MonoBehaviour
{
	int _currentIndex;
	int[] _correctIndexes;
	[SerializeField]
	private int _length = 50;
	public int Length { get { return _width; } }
	[SerializeField]
	private int _width = 4;
	public int Width { get { return _width; } }
	[SerializeField]
	private int _height = 4;
	public int Height { get { return _height; } }
	[SerializeField]
	private SpriteRenderer _tilePrefab;
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
				SpriteRenderer tile = Instantiate(_tilePrefab) as SpriteRenderer;
				tile.transform.SetParent(this.transform);
				tile.transform.position = new Vector2(l+w*i, b+h*j);
				tile.transform.localScale = new Vector3(w, h, 1);
				if (_correctIndexes[j] == i)
				{
					tile.color = Color.black;
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

	public TapResult OnTapTile(Vector2 tapPos)
	{
		int correct = _correctIndexes[_currentIndex];
		float start = (float)correct / _width;
		float end = (float)(correct + 1) / _width;
		float x = tapPos.x;
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
				transform.position -= _tileHeight * Vector3.up;
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
