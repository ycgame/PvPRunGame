using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }

	[SerializeField]
	AudioSource _bgmSource;
	[SerializeField]
	AudioSource _seSource;

	[SerializeField]
	AudioClip[] _bgmClip;
	[SerializeField]
	AudioClip[] _seClip;

	void Awake()
	{
		Instance = this;
	}

	public void PlayBGM(BGMType type)
	{
		_bgmSource.clip = _bgmClip[(int)type];
		_bgmSource.Play();
	}

	public void StopBGM()
	{
		_bgmSource.Stop();
	}

	public void PlaySE(SEType type)
	{
		_seSource.clip = _seClip[(int)type];
		_seSource.PlayOneShot(_seSource.clip);
	}
}

public enum BGMType
{
	Menu,
	InGame,
}

public enum SEType
{
	Win,
	Lose,
	CountDown,
	CorrectTap,
	FailTap,
	ButtonClick,
}
