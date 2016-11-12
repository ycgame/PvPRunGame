using UnityEngine;

public class TileManager : MonoBehaviour
{
	int _currentIndex;
	int[] _correctIndexes;
	GameObject[,] _tileObjects;
	[SerializeField]
	private int _createLength = 50;
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
	private GameObject _wrongTilePrefab;
	[SerializeField]
	private GameObject _correnctTilePrefab;
	private float _tileHeight;
	private float _tileWidth;

	void Awake()
	{
		CreateTiles();
	}

	private void CreateTiles()
	{
		_tileObjects = new GameObject[_width,_createLength];
		for (int j = 0; j < _createLength; j++)
		{
			for (int i = 0; i < _width; i++) {
				var prefab = i==0 ? _correnctTilePrefab : _wrongTilePrefab;
				GameObject tile = Instantiate(prefab) as GameObject;
				tile.transform.SetParent(this.transform);
				_tileObjects[i, j] = tile;
			}
		}
	}

	private void SetTiles()
	{
		float screenH = 2f * Camera.main.orthographicSize;
		float screenW = screenH * Camera.main.aspect;
		_tileWidth = screenW / _width;
		//_tileHeight = screenH / (_height+1);
		_tileHeight = _tileWidth;
		for (int j = 0; j < _length; j++)
		{
			int count = 1;
			for (int i = 0; i < _width; i++) {
				GameObject tile = i == _correctIndexes[j] ? _tileObjects[0, j] : _tileObjects[count++, j];
				tile.transform.position = CalcTilePosition(i, j);
				tile.transform.localScale = new Vector3(_tileWidth, _tileHeight, 1);
			}
		}
	}


	public void Initialize()
	{
		_currentIndex = 0;
		_correctIndexes = new int[_length];
		for (int i = 0; i < _length; i++)
		{
			_correctIndexes[i] = Random.Range(0, _width);
		}
		SetTiles();
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
		float x = tapPos.x;
		int step = (int)(x * _width);
		TapResult result = new TapResult()
		{
			step = step,
			stepCnt = _currentIndex
		};
		if (step == correct)
		{
			_currentIndex++;
			if (_currentIndex == _length)
			{
				result.type = TapResult.Type.Clear;
			}
			else
			{
				result.type = TapResult.Type.Success;
			}
		}
		else
		{
			result.type = TapResult.Type.Failed;
		}
		return result;
	}

	public Vector3 CalcTilePosition(int step, int stepCnt)
	{
		float l = -_tileWidth * ((float)_width / 2 - 0.5f);
		float screenH = 2f * Camera.main.orthographicSize;
		float b = 2 * _tileHeight - screenH / 2;
		return new Vector3(l+_tileWidth*step, b+_tileHeight*stepCnt, 0);
	}

	public float CalcCameraY(int stepCnt)
	{
		return _tileHeight * (stepCnt + 1);
	}
}

public struct TapResult
{
	public int step;
	public int stepCnt;
	public Type type;

	public enum Type
	{
		Failed,
		Success,
		Clear,
	}
}
