using UnityEngine;
using System.Collections;

//public enum BotState {Idle, Exploring, Pathfinding};
public class Bot : MonoBehaviour {
	
	int[,,] LabyrinthMap;
	private int side_size;
	bool isHit;
	public float distanceOfView = 0.5f;
	ArrayList explorable = new ArrayList();
	public GameObject CubePref;
	//BotState state = BotState.Idle;
	Pathfinding PF;
	bool isPFWorking = false;
	ArrayList MapBlocks = new ArrayList();
	
	void Start () {
	GameObject go = GameObject.Find("LabyrinthCreator");
	if (go != null) side_size = go.GetComponent<LabyrinthCreator>().side_size;
	LabyrinthMap = new int[side_size+1,side_size+1,side_size+1];
	for (int i = 0; i < side_size; i++)
		for (int j = 0; j < side_size; j++)
			for (int k = 0; k < side_size; k++)
				LabyrinthMap[i,j,k] = -1;
	
	LookAround();
	InvokeRepeating("MyUpdate", 0f, 0.5f);
	}

	void MyUpdate () {
		CheckExplorable();
		if (explorable.Contains(transform.position)) LookAround();
		else 
		{
			Debug.Log("Position: "+ transform.position.ToString()+
				"isExplorable = "+explorable.Contains(transform.position));
		Debug.Log("Exploring... ");
		EchoLocate ();
		GoToUnexplored();
		}
		
		BuildMap();
		//if (!isHit) MoveFwd();
		//else RotateRight();
//		if (Input.GetKeyDown(KeyCode.UpArrow)) MoveFwd();
//		if (Input.GetKeyDown(KeyCode.LeftArrow)) RotateLeft();
//		if (Input.GetKeyDown(KeyCode.RightArrow)) RotateRight();
	}
	

	void CheckExplorable()
	{
		ArrayList already_explored = new ArrayList();
		Debug.Log("Explorable bl0cks1: "+explorable.Count.ToString());
		foreach (Vector3 cell in explorable)
		{
			int NAcount = 0;
			for (int i = 0; i<3; i++)
			{
				Vector3 v = new Vector3(0,0,0);
				v[i] = 1;
				v+=cell;
				if (LabyrinthMap[(int)v.x,(int)v.y,(int)v.z] == -1) NAcount++;
				v = new Vector3(0,0,0);
				v[i] = -1;
				v+=cell;
				if (LabyrinthMap[(int)v.x,(int)v.y,(int)v.z] == -1) NAcount++;
			}
			//Debug.Log ("explorable: "+NAcount.ToString()+" : "+cell.ToString());
			if (NAcount == 0) already_explored.Add (cell);
		}
		foreach (Vector3 cell in already_explored)
			explorable.Remove(cell);
		Debug.Log("Explorable bl0cks2: "+explorable.Count.ToString());
	}
	
	void GoToUnexplored()
	{
		Vector3 aim = new Vector3(0,0,0);
		if (explorable.Count > 0) aim = (Vector3)explorable[0];
		else aim = transform.position;
		//Debug.Log("Aim is set to: "+aim.ToString());
		if (!isPFWorking && aim!=transform.position) StartPathFinding(aim);
		else if (PF != null && PF.isFinished) PFNextMove();
		else Debug.Log("nothing to do about going anywhere");
	}
	
	void PFNextMove()
	{
		Debug.Log("Finishing PF");
		Debug.Log("MovingTo: "+((Vector3)PF.ThePath[1]).ToString());
		transform.position = (Vector3)PF.ThePath[1];
		isPFWorking = false;
		Destroy(PF.gameObject);
	}
	
	void StartPathFinding(Vector3 aim)
	{
		Debug.Log("Starting PF");
		isPFWorking = true;
		GameObject go = new GameObject("Pathfinding");
		PF = go.AddComponent<Pathfinding>();
		PF.side_size = side_size;
		PF.LabyrinthMap = LabyrinthMap;
		PF.init_pos = transform.position;
		PF.aim_pos = aim;
		Debug.Log ("Position: "+transform.position.ToString());
		Debug.Log ("New aim: "+aim.ToString());
		PF.gameObject.SetActive(true);
		//while (!PF.isFinished) {}
	}
	
	void OnGUI()
	{
		GUI.Box (new Rect (10,10,50,20), isHit.ToString());
		//GUI.Box (new Rect (50,50,50,50)
	}
	void LookAround()
	{
		Debug.Log("Looking around");
		for (int i = 0; i<4; i++)
		{
			RotateRight();
			EchoLocate();
		}
//		for (int i = 0; i<4; i++)
//		{
//			RotateUp();
//			EchoLocate();
//		}
		
	}
	void RotateUp()
	{
		transform.Rotate(Vector3.left, 90);
	}
	void EchoLocate()
	{
		
		Vector3 fwd = transform.TransformDirection(Vector3.forward * Time.deltaTime);
		Vector3 up = transform.TransformDirection(Vector3.up * Time.deltaTime);
		Vector3 dwn = transform.TransformDirection(Vector3.down * Time.deltaTime);
		if (Physics.Raycast(transform.position, fwd, distanceOfView))
			isHit = true;
		else isHit = false;
		RaycastInDirection(fwd);
		RaycastInDirection(up);
		RaycastInDirection(dwn);
	}
	
	void RaycastInDirection(Vector3 direction)
	{
		RaycastHit hit;
		int axis = 0;
		//Debug.Log("Raycast");
		if (Physics.Raycast(transform.position, direction, out hit))
		{
			for (int i = 0; i<3; i++)
				if ((int)(transform.position - hit.transform.position)[i] != 0) axis = i;
			Vector3 a = new Vector3(0,0,0);
			if (transform.position[axis] < hit.transform.position[axis]) a[axis] = 1;
			else if (transform.position[axis] > hit.transform.position[axis]) a[axis] = -1;
			else a[axis] = 0;
			for (Vector3 b = transform.position+a; b!= hit.transform.position; b += a)
			{
				//Debug.Log("Raycast: Scanning: "+b.ToString()+" --> "+LabyrinthMap[(int)b.x,(int)b.y,(int)b.z]);
				if (LabyrinthMap[(int)b.x,(int)b.y,(int)b.z] == -1) 
				{
					LabyrinthMap[(int)b.x, (int)b.y, (int)b.z] = 0;
					if (!explorable.Contains(b))
						explorable.Add(b);
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
					if (LabyrinthMap[i,j,k] == 1 && !MapBlocks.Contains(new Vector3(i,j,k))) 
						{
						Instantiate(CubePref, new Vector3(i+side_size+1,j,k), Quaternion.identity);
						MapBlocks.Add(new Vector3(i,j,k));
						}
		
	}
	
	
	
}
