using UnityEngine;
using System.Collections;

public class CubeHider : MonoBehaviour {

	
	private LabyrinthCreator Maze;
	private MeshRenderer my_renderer;
	void Start () {
		GameObject go = GameObject.Find("LabyrinthCreator");
		if (go != null) Maze = go.GetComponent<LabyrinthCreator>();
		renderer = gameObject.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - Camera.main.transform.position).magnitude <= Maze.noDrawDistance) 
			my_renderer.enabled = false;
		else my_renderer.enabled = true;
	}
}
