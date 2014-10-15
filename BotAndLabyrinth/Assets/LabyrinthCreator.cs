using UnityEngine;
using System.Collections;

public class LabyrinthCreator : MonoBehaviour {

	
	int[,,] Labyrinth;
	public int side_size = 5;


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
					if (Labyrinth[i,j,k] == 1) Instantiate(CubePref, new Vector3(i,j,k), Quaternion.identity);
					
	}
	
	void Start () {
	
	
		GenerateLabyrinth();
		//GenerateBorders();			
		BuildLabyrinth();
		
	}
	
	
	void Update () {
	
	}
}
