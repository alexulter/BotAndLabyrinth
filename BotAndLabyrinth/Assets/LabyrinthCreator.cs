using UnityEngine;
using System.Collections;

public class LabyrinthCreator : MonoBehaviour {

	
	int[,,] Labyrinth;
	public int side_size = 5;
	private ArrayList MazeBlocks = new ArrayList();
	public float noDrawDistance = 6f;
	public ArrayList HidedCubes = new ArrayList();


	public GameObject CubePref;
	
	void GenerateLabyrinth()
	{
		Labyrinth = new int[side_size,side_size,side_size];
		for (int i = 0; i < side_size; i++)
			for (int j = 0; j < side_size; j++)
				for (int k = 0; k < side_size; k++)
					if (i!=0 && j!=0 && k!=0 && i!=side_size-1 && j!=side_size-1 && k!=side_size-1) 
						Labyrinth[i,j,k] = Random.Range(0,2);
					else Labyrinth[i,j,k] = 1;
		for (int i = 1; i<4; i++)
			for (int j = 1; j<4; j++)
				for (int k = 1; k<4; k++)
					Labyrinth[i,j,k] = 0;
	}
	
	void GenerateBorders()
	{
		Labyrinth = new int[side_size,side_size,side_size];
		for (int i = 0; i < side_size; i++)
			for (int j = 0; j < side_size; j++)
				for (int k = 0; k < side_size; k++)
				{
					if (j == 0) Labyrinth[i,j,k] = 1;
					else if ((i == 0 || i == side_size-1 || k == 0 || k == side_size-1) && j <= 2) Labyrinth[i,j,k] = 1;
					else Labyrinth[i,j,k] = 0;
				}
					
	}
	
	
	void BuildLabyrinth()
	{
		for (int i = 0; i < side_size; i++)
			for (int j = 0; j < side_size; j++)
				for (int k = 0; k < side_size; k++)
				{
				bool isExist = MazeBlocks.Contains(new Vector3(i,j,k));
				bool isTooClose = ((new Vector3(i,j,k) - Camera.main.transform.position).magnitude <= noDrawDistance);
				
					if (Labyrinth[i,j,k] == 1 && !isTooClose && !isExist)
					{
						Instantiate(CubePref, new Vector3(i,j,k), Quaternion.identity);
						MazeBlocks.Add(new Vector3(i,j,k));
					}

				}
					
	}
	
	void GetSettings()
	{
		GameObject UIgo = GameObject.Find("UIManager");
		if (UIgo != null) 
			{
			UIManager UI = UIgo.GetComponent<UIManager>();
			side_size = UI.side_size;
			}
	}
	
	void OnGUI()
	{
		GUI.Box (new Rect (10,10,260,25), "view depth= "+noDrawDistance.ToString()+" ( use keys [ and ] to change)"); 
	}
	
	void Start () {
	
		GetSettings();
		GenerateLabyrinth();
		//GenerateBorders();			
		BuildLabyrinth();
		
	}
	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightBracket)) ChangeNoDrawDist(1f);
		if (Input.GetKeyDown(KeyCode.LeftBracket)) ChangeNoDrawDist(-1f);
	}
	void ChangeNoDrawDist(float delta)
	{
		noDrawDistance += delta;
		if (noDrawDistance < 3) noDrawDistance = 3;
		else if (noDrawDistance > 20) noDrawDistance = 20;
	}
}
