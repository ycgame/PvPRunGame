using UnityEngine;

public class TileManager : MonoBehaviour
{
	int _currentIndex;
	int[] _correctIndexes;
	GameObject[,] _tileObjects;
	GameObject[] _afterTileObjects;
	GameObject _failTileObject;
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
	[SerializeField]
	private GameObject _afterTilePrefab;
	[SerializeField]
	private GameObject _failTilePrefab;
	[SerializeField]
	private GameObject _goalTextObject;
	private float _tileHeight;
	private float _tileWidth;

	void Awake()
	{
		CreateTiles();
	}

	private void CreateTiles()
	{
		_tileObjects = new GameObject[_width,_createLength];
		_afterTileObjects = new GameObject[_createLength];
		for (int j = 0; j < _createLength; j++)
		{
			for (int i = 0; i < _width; i++) {
				var prefab = i==0 ? _correnctTilePrefab : _wrongTilePrefab;
				GameObject tile = Instantiate(prefab) as GameObject;
				tile.transform.SetParent(this.transform);
				_tileObjects[i, j] = tile;
			}
			GameObject afterTile = Instantiate(_afterTilePrefab) as GameObject;
			afterTile.transform.SetParent(this.transform);
			_afterTileObjects[j] = afterTile;
		}
		GameObject failTile = Instantiate(_failTilePrefab) as GameObject;
		failTile.transform.SetParent(this.transform);
		_failTileObject = failTile;
	}

	private void SetTiles()
	{
		HideTile();
		float screenH = 2f * Camera.main.orthographicSize;
		float screenW = screenH * Camera.main.aspect;
		_tileWidth = screenW / _width;
		_tileHeight = _tileWidth;
		//_tileHeight = screenH / (_height+1);
		for (int j = 0; j < _length; j++)
		{
			int count = 1;
			for (int i = 0; i < _width; i++) {
				bool correct = i == _correctIndexes[j];
				GameObject tile = correct ? _tileObjects[0, j] : _tileObjects[count++, j];
				tile.transform.position = CalcTilePosition(i, j);
				tile.transform.localScale = CalcTileScale();
				tile.SetActive(true);
				if (correct)
				{
					GameObject afterTile = _afterTileObjects[j];
					afterTile.transform.position = CalcTilePosition(i, j);
					afterTile.transform.localScale = CalcTileScale();
				}
			}
		}
		var pos = _goalTextObject.transform.position;
		pos.y = CalcTilePosition(0, _length).y;
		_goalTextObject.transform.position = pos;
		_goalTextObject.gameObject.SetActive(false);
		_failTileObject.transform.localScale = new Vector3(_tileWidth, _tileHeight, 1);
	}

	private void HideTile()
	{
		foreach (var tile in _tileObjects)
		{
			tile.SetActive(false);
		}
		foreach (var tile in _afterTileObjects)
		{
			tile.SetActive(false);
		}
		_failTileObject.SetActive(false);
	}

	public void Initialize()
	{
		_correctIndexes = new int[_length];
		for (int i = 0; i < _length; i++)
		{
			_correctIndexes[i] = Random.Range(0, _width);
		}
		InitializeShare();
	}

	public void Initialize(int w, int[] indexes)
	{
		_width = w;
		_correctIndexes = indexes;
		_length = indexes.Length;
		InitializeShare();
	}

	void InitializeShare()
	{
		_currentIndex = 0;
		SetTiles();
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
			_afterTileObjects[_currentIndex].SetActive(true);
			_currentIndex++;
			if (_currentIndex == _length)
			{
				result.type = TapResult.Type.Clear;
				_goalTextObject.gameObject.SetActive(true);
			}
			else
			{
				result.type = TapResult.Type.Success;
			}
		}
		else
		{
			result.type = TapResult.Type.Failed;
			_failTileObject.SetActive(true);
			_failTileObject.transform.position = CalcTilePosition(step, _currentIndex);
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

	public Vector3 CalcTileScale()
	{
		return new Vector3(_tileWidth, _tileHeight, 1);
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
