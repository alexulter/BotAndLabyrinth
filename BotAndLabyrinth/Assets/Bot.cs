using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour {

	int[,,] LabyrinthMap;
	private int side_size;
	bool isHit;
	public float distanceOfView = 0.5f;
	ArrayList explorable = new ArrayList();
	public GameObject CubePref;
	
	void Start () {
	GameObject go = GameObject.Find("LabyrinthCreator");
	if (go != null) side_size = go.GetComponent<LabyrinthCreator>().side_size;
	LabyrinthMap = new int[side_size+1,side_size+1,side_size+1];
	for (int i = 0; i < side_size; i++)
		for (int j = 0; j < side_size; j++)
			for (int k = 0; k < side_size; k++)
				LabyrinthMap[i,j,k] = -1;
	
	InvokeRepeating("MyUpdate", 0f, 0.5f);
	}

	void MyUpdate () {
		if (explorable.Contains(transform.position)) LookAround();
		else 
		{
		EchoLocate ();
		
		}
		BuildMap();
		if (!isHit) MoveFwd();
		else RotateRight();
		if (Input.GetKeyDown(KeyCode.UpArrow)) MoveFwd();
		if (Input.GetKeyDown(KeyCode.LeftArrow)) RotateLeft();
		if (Input.GetKeyDown(KeyCode.RightArrow)) RotateRight();
	}
	
	void GoToUnexplored()
	{
		Vector3 aim;
		if (explorable.Count > 0) aim = (Vector3)explorable[0];
		//pathfinding
	}
	
	void OnGUI()
	{
		GUI.Box (new Rect (10,10,50,20), isHit.ToString());
	}
	void LookAround()
	{
		for (int i = 0; i<4; i++)
		{
			RotateRight();
			EchoLocate();
		}
	}
	void EchoLocate()
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward * Time.deltaTime);
		if (Physics.Raycast(transform.position, fwd, distanceOfView))
			isHit = true;
		else isHit = false;
			int axis = 0;
			if (Physics.Raycast(transform.position, fwd, out hit))
					{
					for (int i = 0; i<3; i++)
						if ((int)(transform.position - hit.transform.position)[i] != 0) axis = i;
					Vector3 a = new Vector3(0,0,0);
					if (transform.position[axis] < hit.transform.position[axis]) a[axis] = 1;
					else if (transform.position[axis] > hit.transform.position[axis]) a[axis] = -1;
					else a[axis] = 0;
					for (Vector3 b = transform.position; b!= hit.transform.position; b += a)
						{
						if (LabyrinthMap[(int)b.x,(int)b.y,(int)b.z] == -1) 
							{
							LabyrinthMap[(int)b.x, (int)b.y, (int)b.z] = 0;
							if (!explorable.Contains(new Vector3(b.x, b.y, b.z))) 
								explorable.Add(new Vector3(b.x, b.y, b.z));
							}
						}
					Vector3 c = hit.transform.position;
					LabyrinthMap[(int)c.x,(int)c.y,(int)c.z] = 1;
					}

	}
	void MoveFwd()
	{
		//transform.Translate(transform.TransformDirection(Vector3.forward).normalized);
		Vector3 a = new Vector3(0,0,0);
		for (int i = 0; i < 3; i++)	a[i] = (int)gameObject.transform.forward.normalized[i];
		gameObject.transform.position += a;
		
	}
	void RotateLeft()
	{
		//transform.Rotate(Vector3.left);
		transform.Rotate(Vector3.up, -90);
	}
	void RotateRight()
	{
		//transform.Rotate(Vector3.right);
		transform.Rotate(Vector3.up, 90);
	}
	
	void BuildMap()
	{
		for (int i = 0; i < side_size; i++)
			for (int j = 0; j < side_size; j++)
				for (int k = 0; k < side_size; k++)
					if (LabyrinthMap[i,j,k] == 1) Instantiate(CubePref, new Vector3(i+side_size+1,j,k), Quaternion.identity);
		
	}
	
	
	
}
