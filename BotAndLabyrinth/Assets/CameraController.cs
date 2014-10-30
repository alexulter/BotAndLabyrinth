using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseX;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationY = 0;
	
public float move_speed = 5f;
void Update()
{	
	if (Input.GetKey(KeyCode.Escape)) Application.LoadLevel("StartingScene");
	if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward*move_speed*Time.deltaTime);
	if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back*move_speed*Time.deltaTime);
	if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left*move_speed*Time.deltaTime);
	if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right*move_speed*Time.deltaTime);
	
	
		
		

			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
			if (Input.GetKey(KeyCode.Space)) transform.Translate(new Vector3(0,move_speed*Time.deltaTime,0), Space.World);
		
}

	
}
