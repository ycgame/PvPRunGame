using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    TileManager _tileManager;

    void Awake()
    {
		_tileManager = GetComponentInChildren<TileManager>();
		_tileManager.Initialize();
    }
}
