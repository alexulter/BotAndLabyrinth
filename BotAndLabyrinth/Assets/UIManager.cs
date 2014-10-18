using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	[System.NonSerialized]
	public int side_size;
	[System.NonSerialized]
	public float speed;
	public Slider SideSizeSlider;
	public Slider RobotSpeedSlider;
	
	
	void Awake()
	{
		UIManager[] sss = FindObjectsOfType<UIManager>();
		if (sss.Length > 1) Destroy(sss[0].gameObject);
		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
		SetSize();
		SetSpeed();
	}

	public void SetSize()
	{
		side_size = (int)SideSizeSlider.value;
	}
	public void SetSpeed()
	{
		speed = RobotSpeedSlider.value;
	}

}
