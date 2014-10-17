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

//	//Скорость движения камеры
//	public float moveSpeed = 5f;
//	public bool CameraFreeze = false;
//	//Коордиаты для нового положения камеры
//	float x=0;
//	float y=0;
//	private bool LockCamera = false; //центрировать ли камеру на персонаже
//	
//	// Update is called once per frame
//	void Update () {
//	
//	
//	
//		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
//		{
//			transform.Translate(0,0,-1);
//		}
//		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
//		{
//			transform.Translate(0,0,1);
//		}
//		//Идет проверка для нижнего края экрана
//		if (Input.mousePosition.y < 20){
//			y = -moveSpeed * Time.deltaTime;
//		}
//		//Идет проверка для верхнего края экрана
//		else if(Input.mousePosition.y > Screen.height-20){
//			y = moveSpeed * Time.deltaTime;
//		}
//		else {y=0;} ;
//		//Идет проверка для левого края экрана
//		if (Input.mousePosition.x < 20){
//			x = -moveSpeed * Time.deltaTime;
//		}
//		//Идет проверка для правого края экрана
//		else if(Input.mousePosition.x > Screen.width-20){
//			x = moveSpeed * Time.deltaTime;
//		}
//		else {x=0;} ;
//		//Устанавливается новое положение камеры, с условием, что ось Х камеры парралельна оси Х области, и наклон сделан по оси Х
//		if (!CameraFreeze)transform.Translate(x,y,y);
//		//Следим за героем
//		int DistanceAwayZ = 3;
//		int DistanceAwayX = 3; // Это расстояние по высоте. Отрицательное, потому что потом я вычитаю
//		if (Input.GetKeyDown (KeyCode.Space)) LockCamera = true;
//		if (Input.GetKeyUp (KeyCode.Space)) LockCamera = false;
//		if (LockCamera)
//		{
//			/// Ищем клон префаба
//			GameObject player = GameObject.Find ("Player(Clone)");
//			if (player)
//			{
//				/// Берем координаты из префаба игрока
//				Vector3 PlayerPOS = player.transform.position;
//				/// Обновляем положение камеры
//				transform.position = new Vector3 (PlayerPOS.x - DistanceAwayX, transform.position.y, PlayerPOS.z - DistanceAwayZ);	
//			}
//			else Debug.Log("no player");
//		}
//	}
	
}
