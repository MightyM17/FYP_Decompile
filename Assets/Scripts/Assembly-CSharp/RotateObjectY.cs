using UnityEngine;
using UnityEngine.EventSystems;

public class RotateObjectY : MonoBehaviour
{
	public float rotationSpeed = 3f;

	private void Start()
	{
		base.transform.localEulerAngles = new Vector3(global.yRotation, 0f, 0f);
	}

	private void Update()
	{
		if (global.grabbed)
		{
			float num = Input.GetAxis("Mouse Y") * rotationSpeed;
			base.transform.Rotate(Vector3.right, 0f - num);
			global.yRotation = base.transform.localEulerAngles.x;
		}
		else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				base.transform.Rotate(Vector3.right, -90f);
				global.yRotation = base.transform.localEulerAngles.x;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				base.transform.Rotate(Vector3.right, 90f);
				global.yRotation = base.transform.localEulerAngles.x;
			}
		}
		else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && !EventSystem.current.IsPointerOverGameObject())
		{
			float num2 = Input.GetAxis("Vertical") * rotationSpeed;
			base.transform.Rotate(Vector3.right, 0f - num2);
			global.yRotation = base.transform.localEulerAngles.x;
		}
	}
}
