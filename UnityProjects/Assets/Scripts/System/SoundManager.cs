﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public static readonly string KEY_MUTE = "IsMute";

	public static SoundManager Instance { get; private set; }
	public bool IsEnabled { get; private set; }

	float _defaultBGMVolume;
	float _defaultSEVolume;
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
		_defaultBGMVolume = _bgmSource.volume;
		_defaultSEVolume = _seSource.volume;
		if (PlayerPrefs.GetInt(KEY_MUTE) == 1)
		{
			Off();
		}
		else
		{
			On();
		}
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

	public void On()
	{
		PlayerPrefs.SetInt(KEY_MUTE, 0);
		PlayerPrefs.Save();
		_bgmSource.volume = _defaultBGMVolume;
		_seSource.volume = _defaultSEVolume;
		IsEnabled = true;
	}
	
	public void Off()
	{
		PlayerPrefs.SetInt(KEY_MUTE, 1);
		PlayerPrefs.Save();
		_bgmSource.volume = 0f;
		_seSource.volume = 0f;
		IsEnabled = false;
	}
	
	public void SwitchVolume()
	{
		if(IsEnabled)
			Off();
		else
			On();
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
