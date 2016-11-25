using UnityEngine;
using NendUnityPlugin.AD;

public class AdManager : MonoBehaviour 
{
	[SerializeField]
	NendAdBanner[] _banners;
	int _index;
	
	void Awake()
	{
		if(_banners == null)
			return;

		_index = Random.Range(0, _banners.Length);
		foreach(var banner in _banners)
		{
			banner.Hide();
		}
		_banners[_index].Show();
	}

	public void Show()
	{
		_banners[_index].Show();
	}
	
	public void Hide()
	{
		_banners[_index].Hide();
	}
}
