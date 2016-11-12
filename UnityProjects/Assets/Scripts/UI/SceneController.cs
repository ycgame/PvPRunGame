using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
	public static SceneController Instance { get; private set; }

	[SerializeField]
	UIBase[] _GUIs;

	void Awake()
	{
		Instance = this;
		Utility.Input.Initialize();
		HideAll();
	}

	public void Initialize()
	{
		Show(UIType.Titie, true);
		var title = GetUI<UI_Title>(UIType.Titie);
		if (title != null)
		{
			title.SetName();
		}
	}

	public void OnStartTimeAttack()
	{
		GameManager.Instance.StartTimeAttack();
	}

	public void OnStartNetwork()
	{
		NetworkManager.Instance.CreateMatch();
		Show(UIType.Matching, true);
		StartCoroutine(RequestAI());
	}

	public void OnBack()
	{
		Show(UIType.Titie, true);
	}

	public void OnRestart()
	{
		OnBack();
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

	public UIDrived GetUI<UIDrived>(UIType type) where UIDrived : UIBase
	{
		return _GUIs[(int)type] as UIDrived;
	}

	public void OnCreateTile(int w, int[] stage)
	{
		GameManager.Instance.StartNetwork(w, stage);
	}

	public enum UIType
	{
		Titie,
		Matching,
		Result,
		InGame,
		MAX,
	}
}
