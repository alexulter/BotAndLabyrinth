using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {
	
	public UIManager UI_Manager;
	public void Initialize()
	{
		Application.LoadLevel("Scene1");
	}
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) Application.Quit();
	}
}
