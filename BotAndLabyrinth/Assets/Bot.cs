using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour {
	
	//----------
	//DELETE
//	private ArrayList explorable = new ArrayList();
//	private ArrayList MazeMap = new ArrayList();
//	private ArrayList MazeCells = new ArrayList();
//	int axis = 0;
	//---------
	
	private ArrayList EmptySpaces = new ArrayList();
	private ArrayList WallsConfig = new ArrayList();
	private int freespace = -1;
	bool isHit;
	public float distanceOfView = 0.5f;
	ArrayList ExplorableSpaces = new ArrayList();
	public GameObject CubePref;
	public GameObject MarkedCubePref;
	Pathfinding PF;
	bool isPFWorking = false;
	ArrayList MapBlocks = new ArrayList();
	private float speed = 1f;
	BotMovement Move;
	private bool isSmoothMovement = true;
	private bool isUpdating = false;
	private Object BotImageInstance = null;
	public GameObject BotImage;
	public Object FinishScreen;
	private bool isMarking = true;
	
	void Awake() {
	Move = gameObject.AddComponent<BotMovement>();
	}
	
	void Start () 
	{
	SetSettings();
	if (freespace < 0) 
		{	
		Destroy(gameObject);
		Debug.LogError("The Bot script couldn't get size of the maze");
		}
	EmptySpaces.Add (transform.position);
	WallsConfig.Add(new int[] {-1,-1,-1,-1,-1,-1});
	ExplorableSpaces.Add(transform.position);
	EchoLocate();
	if (speed <= 2)InvokeRepeating("MyUpdate", 0f, 2f/speed/speed);
	else isUpdating = true;
	}
	
	void Update()
	{
		if (isUpdating) MyUpdate();
		DestroyImmediate(BotImageInstance);
		BotImageInstance = Instantiate(BotImage, transform.position + new Vector3(freespace,0,0), transform.rotation);
	}
	
	void MyUpdate()
	{
		if (Move.State == Action.Pathfinding && PF != null && PF.isFinished) Move.State = Action.Idle;
		if (Move.State == Action.Idle) MyActions();
		if (Move.State == Action.None) StartCoroutine(EchoLocate());
	}
	void MyActions () {
		CheckExplorable();
		//if (Move.State == Action.None) StartCoroutine(EchoLocate());
		if (Move.State == Action.Idle)
			if (ExplorableSpaces.Contains(transform.position)) 
				RotateRight();
		else GoToUnexplored();
		//StartCoroutine(MyUpdateRoutine());
	}
	
	//	IEnumerator MyUpdateRoutine()
	//	{
	//	CheckExplorable();
	//	if (Move.State == Action.None) StartCoroutine(EchoLocate());
	//	if (Move.State == Action.None)
	//		if (explorable.Contains(transform.position)) 
	//			RotateRight();
	//	else GoToUnexplored();
	//	yield return 0;
	//	}
	
	IEnumerator EchoLocate()
	{
		Move.State = Action.Exploring;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Vector3 up = transform.TransformDirection(Vector3.up);
		Vector3 dwn = transform.TransformDirection(Vector3.down);
		if (Physics.Raycast(transform.position, fwd, distanceOfView))
			isHit = true;
		else isHit = false;
		RaycastInDirection(fwd);
		RaycastInDirection(up);
		RaycastInDirection(dwn);
		BuildMap();
		Move.State = Action.Idle;
		yield return 0;
	}
	void SetSettings()
	{
		GameObject go = GameObject.Find("LabyrinthCreator");
		if (go != null) freespace = go.GetComponent<LabyrinthCreator>().side_size + 5;
		
		GameObject UIgo = GameObject.Find("UIManager");
		if (UIgo != null) 
		{
			UIManager UI = UIgo.GetComponent<UIManager>();
			speed = UI.speed;
			isMarking = UI.isMarking;
		}
		if (speed > 2) 
		{
		Move.speed *= speed*speed/10;
		Move.rotation_speed *= speed*speed/10;
		}
		else if (speed > 0)
		{
			Move.speed *= speed/2;
			Move.rotation_speed *= speed/2;
		}
	}


	void CheckExplorable()
	{
		ArrayList already_explored = new ArrayList();
		//Debug.Log("Explorable bl0cks1: "+explorable.Count.ToString());
		foreach (Vector3 cell in ExplorableSpaces)
		{
			int[] ddd = (int[])WallsConfig[EmptySpaces.IndexOf(cell)];
			int NAcount = 0;
			for (int i = 0; i<6; i++)
			{
				if (ddd[i] == -1) NAcount++;
			}
			if (NAcount == 0) already_explored.Add (cell);
		}
		foreach (Vector3 cell in already_explored)
			ExplorableSpaces.Remove(cell);
		//Debug.Log("Explorable bl0cks2: "+explorable.Count.ToString());
	}
	
	void GoToUnexplored()
	{
		Vector3 aim = new Vector3(0,0,0);
		if (ExplorableSpaces.Count > 0) aim = (Vector3)ExplorableSpaces[0];
		else 
		{
			aim = transform.position;
			if (FinishScreen != null) Instantiate(FinishScreen);
		}
		//Debug.Log("Aim is set to: "+aim.ToString());
		if (!isPFWorking && aim!=transform.position) StartPathFinding(aim);
		else if (PF != null && PF.isFinished) PFNextMove();
		else Debug.LogWarning("nothing to do about going anywhere");
	}
	
	
	
	void PFNextMove()
	{
		//Debug.Log("MovingTo: "+((Vector3)PF.ThePath[1]).ToString());
		Vector3 v = (Vector3)PF.ThePath[1];
		//if (LabyrinthMap[(int)v.x,(int)v.y,(int)v.z] == -1) RotateV(v);
		//else 
		if (isSmoothMovement) Move.StepTo(v);
		else transform.position = v;
		isPFWorking = false;
		Destroy(PF.gameObject);
	}
//	void RotateV(Vector3 v)
//	{
//		if ((v - transform.position).normalized == Vector3.right) RotateRight();
//		else if ((v - transform.position).normalized == Vector3.left) RotateLeft();
//		else RotateLeft();
//	}
	void StartPathFinding(Vector3 aim)
	{
		Debug.Log("Starting PF");
		Move.State = Action.Pathfinding;
		isPFWorking = true;
		GameObject go = new GameObject("Pathfinding");
		PF = go.AddComponent<Pathfinding>();
		//PF.MazeMap = MazeMap;
		//PF.MazeCells = MazeCells;
		PF.EmptySpaces = EmptySpaces;
		PF.WallsConfig = WallsConfig;
		PF.init_pos = transform.position;
		PF.aim_pos = aim;
		Debug.Log ("Position: "+transform.position.ToString());
		Debug.Log ("New aim: "+aim.ToString());
		PF.gameObject.SetActive(true);
	}
	
	void OnGUI()
	{	
		//GUI.Box (new Rect (10,10,250,25), "view depth= "+"  "+" ( use keys [ and ] )");
		GUI.Box (new Rect (10,40,150,25), "FaceToWall?    "+isHit.ToString());
		GUI.Box (new Rect (10,70,150,25), "position: "+transform.position.ToString());
		GUI.Box (new Rect (10,100,150,25), "explorable cells: "+ExplorableSpaces.Count.ToString());
		GUI.Box (new Rect (10,130,150,25), "State: "+Move.State.ToString());
		if (Move.State == Action.Movement) GUI.Box (new Rect (10,160,200,25), "Going to: "+Move.aim_position.ToString());
	}

	void RotateUp()
	{
		transform.Rotate(Vector3.left, 90);
	}
	
	int GetIndexFromDirection(Vector3 direction)
	{
		if ((int)direction.x >0) return 0;
		else if ((int)direction.x <0) return 1;
		else if ((int)direction.y >0) return 2;
		else if ((int)direction.y <0) return 3;
		else if ((int)direction.z >0) return 4;
		else if ((int)direction.z <0) return 5;
		else 
		{
		Debug.LogError("Direction To Index conversion error"+direction.ToString());
		return -1;
		}
	}
	
	void RaycastInDirection(Vector3 direction)
	{
		RaycastHit hit;
		//int axis = 0;
//		int step = 1;
//		for (int kkk = 0; kkk < 1000000; kkk++)
//		{
//			Physics.Raycast(transform.position, direction, step);
//			
//			step++;
//		}
		if (Physics.Raycast(transform.position, direction, 1f)) {
		//draw wall
			((int[]) WallsConfig[EmptySpaces.IndexOf(transform.position)])[GetIndexFromDirection(direction)] = 1;
		}
		else {
			//add new cell
			((int[]) WallsConfig[EmptySpaces.IndexOf(transform.position)])[GetIndexFromDirection(direction)] = 0;
			Vector3 target = transform.position + direction.normalized;
			if (!EmptySpaces.Contains(target))
			{
			EmptySpaces.Add(target);
			WallsConfig.Add(new int[] {-1,-1,-1,-1,-1,-1});
			ExplorableSpaces.Add (target);
			}
		} 
		
//		if (Physics.Raycast(transform.position, direction, out hit))
//		{
//		
//			for (int i = 0; i<3; i++)
//				if ((int)(transform.position - hit.transform.position)[i] != 0) axis = i;
//			Vector3 a = new Vector3(0,0,0);
//			if (transform.position[axis] < hit.transform.position[axis]) a[axis] = 1;
//			else if (transform.position[axis] > hit.transform.position[axis]) a[axis] = -1;
//			else a[axis] = 0;
//			for (Vector3 b = transform.position+a; b!= hit.transform.position; b += a)
//			{
//				if (!MazeMap.Contains(b))
//				{
//					MazeMap.Add(b);
//					MazeCells.Add((int)0);
//					if (!explorable.Contains(b))
//						explorable.Add(b);
//				}
//			}
//			Vector3 c = hit.transform.position;
//			if (isMarking && !MazeMap.Contains(c)) 
//				{
//				Destroy(hit.transform.gameObject);
//				Instantiate(MarkedCubePref, c, Quaternion.identity);
//				}
//			MazeMap.Add (c);
//			MazeCells.Add((int)1);
//		}
	}
//	void MoveFwd()
//	{
//		Move.StepTo(transform.position + transform.forward.normalized);
//		
//	}
	
	void RotateRight()
	{
		if (isSmoothMovement) Move.RotateRight();
		else transform.Rotate(Vector3.up, 90);
	}
//	void RotateLeft()
//	{
//		if (isSmoothMovement) Move.RotateLeft();
//		else transform.Rotate(Vector3.up, -90);
//	}
	
	void BuildMap()
	{
//	foreach (Vector3 cell in EmptySpaces)
//					for (int ddd =0; ddd<6; ddd++)
//					if (((int)WallsConfig[EmptySpaces.IndexOf(cell)])[ddd] == 1 && 
//						!MapBlocks.Contains(cell+))
//						{
//						Instantiate(CubePref, new Vector3(cell.x+freespace,cell.y,cell.z), Quaternion.identity);
//						MapBlocks.Add(cell);
//						}

	}
	

	
	
}
