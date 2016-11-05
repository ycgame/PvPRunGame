using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
	public static SceneController Instance { get; private set; }

	[SerializeField]
	GameManager _gameManager;
	[SerializeField]
	GameObject[] _GUIs;

	int _w;
	int[] _stage;
	bool _matching;
	bool _isReady;

	void Awake()
	{
		Instance = this;
		Utility.Input.Initialize();
		_GUIs[(int)UIType.Matching].SetActive(false);
	}

	void Update()
	{
		if (_matching)
			return;

		if (_isReady)
		{
			_matching = true;
			_gameManager.StartNetwork(_w, _stage);
			_GUIs[(int)UIType.Matching].SetActive(false);
		}
	}

	public void OnStartTimeAttack()
	{
		_gameManager.StartAI();
		_GUIs[(int)UIType.Titie].SetActive(false);
	}

	public void OnStartNetwork()
	{
		NetworkManager.Instance.CreateMatch();
		_GUIs[(int)UIType.Titie].SetActive(false);
		_GUIs[(int)UIType.Matching].SetActive(true);
		StartCoroutine(RequestAI());
	}

	IEnumerator RequestAI()
	{
		yield return new WaitForSeconds(0.5f);
		yield return NetworkManager.Instance.AIRequestPost();
	}

	public void OnCreateTile(int w, int[] stage)
	{
		_w = w;
		_stage = stage;
		_isReady = true;
	}

	public enum UIType
	{
		Titie,
		Matching,
	}
}
