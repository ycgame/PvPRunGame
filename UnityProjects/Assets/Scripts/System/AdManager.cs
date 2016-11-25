using UnityEngine;
using NendUnityPlugin.AD;

public class AdManager : MonoBehaviour 
{
	public static AdManager Instance { get; private set; }
	
	[SerializeField]
	NendAdBanner[] _banners;
	int _index;
	
	void Awake()
	{
		Instance = this;
		if(_banners == null)
			return;

		_index = Random.Range(0, _banners.Length);
		_banners[_index].AdLoaded += (sender, e) => Show();
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
