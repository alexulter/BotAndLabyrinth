using UnityEngine;
using System.Collections;

public class Tests : MonoBehaviour {

	bool isHit = false;
	public float distance = 1f;
	public GameObject WallPref;
	
	
	void Start()
	{
		Instantiate(WallPref,transform.position, Quaternion.Euler(90,0,0));
	}
	
	
	void Update () {
	
	
		if (Physics.Raycast(transform.position, Vector3.forward, distance)) isHit = true;
		else isHit = false;
	
//	while (!Physics.Raycast(transform.position, Vector3.forward))
//	{
//	
//		
//	}
	
	
	}
	void OnGUI()
	{
		GUI.Box (new Rect (10,10,260,25), "State: "+isHit); 
	}
}
