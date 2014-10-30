﻿using UnityEngine;
using System.Collections;
//using System;



public class Pathfinding : MonoBehaviour {
	
	//public Bot bot;
	Vector3[] directions = new Vector3[6];
	int ways_length;
	ArrayList NewWaysList;
	ArrayList WaysList;
	public Vector3 init_pos;
	public Vector3 aim_pos;
	bool isAtFinish = false;
	public bool isFinished = false;
	ArrayList usedCells = new ArrayList();
	int num_of_iterations = 100000;
	[System.NonSerialized]
	public ArrayList ThePath;
	
	//public int[,,] LabyrinthMap;
//	public ArrayList MazeMap;
//	public ArrayList MazeCells;
	public ArrayList EmptySpaces = new ArrayList();
	public ArrayList WallsConfig = new ArrayList();
	//public int side_size = 10;
	public GameObject CubePref;
	
//	Vector3 aim_direction = aim_pos - init_pos;
	
//	private void GenerateLabyrinth()
//	{
//		LabyrinthMap = new int[side_size,side_size,side_size];
//		for (int i = 0; i < side_size; i++)
//			for (int j = 0; j < side_size; j++)
//				for (int k = 0; k < side_size; k++)
//					LabyrinthMap[i,j,k] = 0;
//	}
	
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
			Debug.LogError("Direction To Index conversion error");
			return -1;
		}
	}
		
		public bool isPassable(Vector3 fromCell, Vector3 direction)
		{
//		if (cell.x < 0 || cell.y < 0 || cell.z < 0) return false;
//		else if (cell.x > side_size-1 || cell.y > side_size-1 || cell.z > side_size-1) return false;
//		else if (LabyrinthMap[(int)cell.x, (int)cell.y, (int)cell.z] == 1) return false;
//		else if (LabyrinthMap[(int)cell.x, (int)cell.y, (int)cell.z] == 11) return false;
//		else return true;
		Vector3 cell = fromCell + direction;
		bool isContains = EmptySpaces.Contains(fromCell);
		//UNITY BUG HERE
//		int index3 = -1;
//		if (EmptySpaces.Contains(fromCell)) isContains = true;
//		else foreach (Vector3 itm in EmptySpaces)
//			if (itm == fromCell) isContains = true;
			
			
		int index = GetIndexFromDirection(direction);
		
		//UNITY BUG HERE
		int index2 = -1;
		if (EmptySpaces.Contains(fromCell)) index2 = EmptySpaces.IndexOf(fromCell);
//		else foreach (Vector3 itm in EmptySpaces)
//			if (itm == fromCell) index2 = EmptySpaces.IndexOf(itm);
			
		if (!isContains || ((int[])WallsConfig[index2])[index] != 1) return true;
		else return false;
		}
		public bool inUse(Vector3 cell)
		{
			if (usedCells.Contains(cell)) return true;
			else 
			// UNITY BUG HERE
//			foreach (Vector3 itm in usedCells)
//			if (itm == cell) return true;
			//
			return false;
			
		}
	
	//public class Way : Array{}
	

	//int i,j,k,;
	// Use this for initialization
	void Awake()
	{
		/*init_pos = new Vector3(1,0,0);
		aim_pos = init_pos + new Vector3(6,9,9);
		GenerateLabyrinth();
		LabyrinthMap[3,0,0] = 1;*/
		gameObject.SetActive(false);
	}
	void Start () {
	
	for (int i = 0; i<3; i++)
	{
		directions[i] = new Vector3(0,0,0);
		directions[i+3] = new Vector3(0,0,0);
		directions[i][i] = 1;
		directions[i+3][i] = -1;
	}
	
	ways_length = 0;
	WaysList = new ArrayList();
	ArrayList initway = new ArrayList();
	initway.Add (init_pos);
	WaysList.Add (initway);
	usedCells.Add (init_pos);
	for (int mmm = 0; mmm < num_of_iterations; mmm++) 
		if(!isAtFinish) ExpandAllWays();
//		else 
//		{
//			foreach (Vector3 cell in ThePath)
//			{
//				Instantiate(CubePref,cell, Quaternion.identity);
//			}
//			break;
//		}
	isFinished = isAtFinish;
	if (!isFinished) Debug.LogWarning("Failed to find a path!");
	
	
	}
	
	void ExpandWay(ArrayList way)
	{
		//Vector3 cell = (Vector3) way[ways_length];
		Vector3 cell = (Vector3) way[way.Count-1];
		for (int i = 0; i < 6; i++) 
		{
			Vector3 newStep = cell + directions[i];
			if (isPassable(cell, directions[i]) && !inUse(newStep))
			{
				//newWay = ((ArrayList)way.Clone()).Resize(ways_length+1);
				ArrayList newWay = (ArrayList)way.Clone();
				//newWay[ways_length+1] = newStep;
				newWay.Add (newStep);
				NewWaysList.Add(newWay);
				usedCells.Add (newStep);
				if (newStep == aim_pos)
				{
					isAtFinish = true;
					ThePath = newWay;
				}
			}

		}
	}
	
	void ExpandAllWays()
	{
		NewWaysList = new ArrayList(WaysList.Count*4);
		for (int k = 0; k < WaysList.Count; k++)
		{
			ArrayList kkk = (ArrayList)WaysList[k];
			ExpandWay(kkk);
			if (isAtFinish) break;
		}
		WaysList = NewWaysList;
		ways_length++;
	}
}
