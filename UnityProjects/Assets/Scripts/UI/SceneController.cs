using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
	[SerializeField]
	GameManager _gameManager;
	[SerializeField]
	GameObject _titleGUI;

	void Awake()
	{
		Utility.Input.Initialize();
	}

	public void OnStartGame()
	{
		_gameManager.StartAI();
		_titleGUI.SetActive(false);
	}
}
