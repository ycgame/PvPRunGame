using UnityEngine;
using TMPro;

public class DiffImage : MonoBehaviour 
{
	[SerializeField]
	SpriteRenderer _up, _down, _main;
	[SerializeField]
	TextMeshPro _count;
	
	void Awake()
	{
		SetActive(false);
		_count.renderer.sortingOrder = 1100;
	}
	
	public void SetScale(Vector3 scale)
	{
		transform.localScale = scale;
	}
	
	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}
	
	public void SetImage(float x, int diff)
	{
		bool isLead = diff < 0;
		_up.enabled = isLead;
		_down.enabled = !isLead;
		_count.text = Mathf.Abs(diff).ToString();
		var cameraPos = Camera.main.transform.position;
		var height = Camera.main.orthographicSize;
		var scale = transform.localScale;
		var pos = new Vector3(x, 0f, 0f);
		var sign = isLead ? 1 : -1;
		var offset = height - scale.y;
		pos.y = cameraPos.y + sign * offset;
		transform.position = pos;
	}
}
