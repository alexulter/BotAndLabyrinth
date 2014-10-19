using UnityEngine;
using System.Collections;

public class CubeHider : MonoBehaviour {

	
	private LabyrinthCreator Maze;
	private MeshRenderer ppp;
	void Start () {
		GameObject go = GameObject.Find("LabyrinthCreator");
		if (go != null) Maze = go.GetComponent<LabyrinthCreator>();
		ppp = gameObject.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - Camera.main.transform.position).magnitude <= Maze.noDrawDistance) 
			ppp.enabled = false;
		else ppp.enabled = true;
	}
}
