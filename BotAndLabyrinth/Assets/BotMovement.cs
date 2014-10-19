using UnityEngine;
using System.Collections;

public enum Action {None, Idle, Movement, Rotation, Exploring, Pathfinding};
public class BotMovement : MonoBehaviour {
	
	public Action State = Action.None;
	public Vector3 aim_position;
	//private Quaternion aim_rotation;
	public float speed = 3f;
	public float rotation_speed = 300f;
	private float angle = 0;
	//private Bot brain;
	private Vector3 v;
	private Quaternion initial_rotation;
	private float distance = 0;
	
	
	void Start()
	{
	//	brain = gameObject.GetComponent<Bot>();
	}
	
	void OnGUI()
	{
//		if (State == Action.Movement) GUI.Box (new Rect (10,130,150,25), "aim_pos: "+aim_position.ToString());
	}
	void Update()
	{
		if (State == Action.Movement)
		{	
			Vector3 dist_delta = speed*v.normalized*Time.deltaTime;
			distance += dist_delta.magnitude;
			if (distance >= 0.9f)
			{
				transform.position = aim_position;
				distance = 0;
				State = Action.None;
			}
			else
			{
				//v = Vector3.Normalize(v);
				//transform.position = aim_position;
				//transform.position += speed*v.normalized*Time.deltaTime;
				transform.Translate(dist_delta, Space.World);
			}
			//Debug.Log("position: "+transform.position.ToString()+" v= "+v.normalized.ToString());
		}
		if (State == Action.Rotation)
		{
			
			float angle_delta = rotation_speed*Time.deltaTime;
			angle += angle_delta;
			Debug.Log("angle: "+angle.ToString());
			if (angle > 85f) 
			{
			//transform.Rotate(Vector3.up, 90f-angle+angle_delta);
			transform.rotation = initial_rotation * Quaternion.Euler(0,90,0);
			angle = 0;
			State = Action.None;
			}
			else transform.Rotate(Vector3.up, angle_delta);
		}
		
		
	}
	
	public void RotateRight()
	{
		State = Action.Rotation;
		initial_rotation = transform.rotation;
		//aim_rotation = initial_rotation * Quaternion.Euler(0,90,0);
		//transform.Rotate(Vector3.up, 90);
		//State = Action.None;
		//State = Action.Rotation;
		//aim_rotation.SetFromToRotation(Vector3.forward, Vector3.right);//Quaternion.FromToRotation(Vector3.forward, Vector3.right);
		//transform.rotation = aim_rotation;
		//State = Action.None;
	}

//	public void MakeStepDirected(Vector3 direction)
//	{
//		Vector3 a = new Vector3(0,0,0);
//		for (int i = 0; i < 3; i++)	a[i] = (int)direction.normalized[i];
//		gameObject.transform.position += a;
//	}
	
	public void StepTo(Vector3 aim)
	{	
		State = Action.Movement;
		aim_position = aim;
		v = aim_position - transform.position;
		//			float vv = 0;
		//			int k = 0;
		//			for (int i = 0; i<3; i++)
		//				if (Mathf.Abs(v[i]) > Mathf.Abs(vv)) {vv = v[i]; k = i;}
		//			v = new Vector3(0,0,0);
		//			if (vv > 0) v[k] = 1;
		//			else v[k] = -1;
		
		//transform.position = aim;
	}
	
}
