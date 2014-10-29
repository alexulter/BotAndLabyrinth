using UnityEngine;
using System.Collections;

public class Tests : MonoBehaviour {

	bool isHit = false;
	public float distance = 1f;
	// Update is called once per frame
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
