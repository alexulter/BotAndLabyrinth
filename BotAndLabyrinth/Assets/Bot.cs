using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour {
	
	private ArrayList EmptySpaces = new ArrayList();
	private ArrayList WallsConfig = new ArrayList();
	private int freespace = -1;
	bool isHit;
	public float distanceOfView = 0.5f;
	ArrayList ExplorableSpaces = new ArrayList();
	public GameObject WallPref;
	public GameObject WallColoredPref;
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
		if (Move.State == Action.Idle)
			if (ExplorableSpaces.Contains(transform.position)) 
				RotateRight();
		else GoToUnexplored();
	}
	
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
		if (!isPFWorking && aim!=transform.position) StartPathFinding(aim);
		else if (PF != null && PF.isFinished) PFNextMove();
		else Debug.LogWarning("nothing to do about going anywhere");
	}
	
	
	
	void PFNextMove()
	{
		Vector3 v = (Vector3)PF.ThePath[1];
		if (isSmoothMovement) Move.StepTo(v);
		else transform.position = v;
		isPFWorking = false;
		Destroy(PF.gameObject);
	}

	void StartPathFinding(Vector3 aim)
	{
		Debug.Log("Starting PF");
		Move.State = Action.Pathfinding;
		isPFWorking = true;
		GameObject go = new GameObject("Pathfinding");
		PF = go.AddComponent<Pathfinding>();
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
		int index = -1;
		if (EmptySpaces.Contains(transform.position)) index = EmptySpaces.IndexOf(transform.position);
			
		if (Physics.Raycast(transform.position, direction, 1f)) {
		//draw wall
			((int[]) WallsConfig[index])[GetIndexFromDirection(direction)] = 1;
			Debug.Log("traget-Wall"+(transform.position+direction.normalized).ToString());
		}
		else {
			//add new cell
			((int[]) WallsConfig[index])[GetIndexFromDirection(direction)] = 0;
			
			Vector3 target = transform.position + new Vector3((int)direction.x, (int)direction.y,
				(int)direction.z);
			if (!EmptySpaces.Contains(target))
			{
			EmptySpaces.Add(target);
			WallsConfig.Add(new int[] {-1,-1,-1,-1,-1,-1});
			ExplorableSpaces.Add (target);
				//Debug.Log("traget-added");
			}
		} 
		
	}

	
	void RotateRight()
	{
		if (isSmoothMovement) Move.RotateRight();
		else transform.Rotate(Vector3.up, 90);
	}

	void BuildMap()
	{
	foreach (Vector3 cell in EmptySpaces)
					for (int ddd =0; ddd<6; ddd++)
					if (((int[])WallsConfig[EmptySpaces.IndexOf(cell)])[ddd] == 1 && 
				    !MapBlocks.Contains(((int)cell.x).ToString()+" "+
				                    ((int)cell.y).ToString()+" "+((int)cell.z).ToString()+" "+ddd.ToString()))
						{
						Instantiate(WallPref, new Vector3(cell.x+freespace,cell.y,cell.z), GetRotationFromDirections(ddd));
						Instantiate(WallColoredPref, new Vector3(cell.x,cell.y,cell.z), GetRotationFromDirections(ddd));
					MapBlocks.Add(((int)cell.x).ToString()+" "+
				              ((int)cell.y).ToString()+" "+((int)cell.z).ToString()+" "+ddd.ToString());

						}

	}
	
	Quaternion GetRotationFromDirections(int ddd)
	{
		if (ddd == 0)return Quaternion.Euler(0,90,0);
		else if (ddd == 1)return Quaternion.Euler(0,-90,0);
		else if (ddd == 5)return Quaternion.Euler(0,180,0);
		else if (ddd == 3)return Quaternion.Euler(90,0,0);
		else if (ddd == 2)return Quaternion.Euler(-90,0,0);
		else return Quaternion.identity;
	}
	

	
	
}
