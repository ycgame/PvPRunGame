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
		SoundManager.Instance.PlayBGM(BGMType.Menu);
		Show(UIType.Titie, true);
		var title = GetUI<UI_Title>(UIType.Titie);
		if (title != null)
		{
			title.SetName();
		}
	}

	public void OnStartTimeAttack()
	{
		SoundManager.Instance.PlaySE(SEType.ButtonClick);
		GameManager.Instance.StartTimeAttack();
	}

	public void OnStartNetwork()
	{
		SoundManager.Instance.PlaySE(SEType.ButtonClick);
		NetworkManager.Instance.CreateMatch();
		Show(UIType.Matching, true);
		StartCoroutine(RequestAI());
	}

	public void OnRanking()
	{
		SoundManager.Instance.PlaySE(SEType.ButtonClick);
		Show(UIType.Ranking, true);
	}

	public void OnBack()
	{
		SoundManager.Instance.PlaySE(SEType.ButtonClick);
		Show(UIType.Titie, true);
	}

	public void OnRestart()
	{
		if (GameManager.Instance.IsNetwork) 
		{
			OnStartNetwork ();
		} else 
		{
			OnStartTimeAttack ();
		}
	}

	public void OnCancel()
	{
		KillAIRequest();
		NetworkManager.Instance.SendCancel();
		Show(UIType.Titie, true);
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

	public void KillAIRequest()
	{
		StopAllCoroutines();
	}

	IEnumerator RequestAI()
	{
		yield return new WaitForSeconds(10f);
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
		Ranking,
		MAX,
	}
}
