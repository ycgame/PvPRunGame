using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	GameManager _gameManager;

	void Awake()
	{
		Utility.Input.Initialize();
	}

	void Start()
	{
		_gameManager.StartAI();
	}
}
