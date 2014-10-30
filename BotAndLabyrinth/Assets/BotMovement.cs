using UnityEngine;
using System.Collections;

public enum Action {None, Idle, Movement, Rotation, Exploring, Pathfinding};
public class BotMovement : MonoBehaviour {
	
	public Action State = Action.None;
	public Vector3 aim_position;
	public float speed = 3f;
	public float rotation_speed = 300f;
	private float angle = 0;
	private Vector3 v;
	private Quaternion initial_rotation;
	private float distance = 0;
	
	
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
			else transform.Translate(dist_delta, Space.World);
		}
		if (State == Action.Rotation)
		{
			
			float angle_delta = rotation_speed*Time.deltaTime;
			angle += angle_delta;
			if (angle > 85f) 
			{
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
	}
	
	public void StepTo(Vector3 aim)
	{	
		State = Action.Movement;
		aim_position = aim;
		v = aim_position - transform.position;
	}
	
}
