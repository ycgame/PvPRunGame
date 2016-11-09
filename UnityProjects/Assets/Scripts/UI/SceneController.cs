using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
	public static SceneController Instance { get; private set; }

	[SerializeField]
	GameObject[] _GUIs;

	int _w;
	int[] _stage;
	bool _matching;

	void Awake()
	{
		Instance = this;
		Utility.Input.Initialize();
		Show(UIType.Titie, true);
	}

	public void OnStartTimeAttack()
	{
		GameManager.Instance.StartAI();
		HideAll();
	}

	public void OnStartNetwork()
	{
		NetworkManager.Instance.CreateMatch();
		Show(UIType.Matching, true);
		StartCoroutine(RequestAI());
	}

	public void OnBack()
	{
		SceneManager.LoadScene("Game");
	}

	public void OnRestart()
	{
		SceneManager.LoadScene("Game");
	}

	public void Show(UIType type, bool isHideOthers = false)
	{
		if (isHideOthers)
			HideAll();
		_GUIs[(int)type].SetActive(true);
	}

	public void Hide(UIType type)
	{
		_GUIs[(int)type].SetActive(false);
	}

	public void HideAll()
	{
		for (int i = 0; i < (int)UIType.MAX; i++)
		{
			_GUIs[i].SetActive(false);
		}
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
		_matching = true;
		GameManager.Instance.StartNetwork(_w, _stage);
		HideAll();
	}

	public enum UIType
	{
		Titie,
		Matching,
		Result,
		MAX,
	}
}
