using UnityEngine;
using UnityEngine.EventSystems;

public class RotateObjectX : MonoBehaviour
{
	public float rotationSpeed = 3f;

	private void Start()
	{
		base.transform.localEulerAngles = new Vector3(0f, global.xRotation, 0f);
	}

	private void Update()
	{
		if (global.grabbed)
		{
			float num = Input.GetAxis("Mouse X") * rotationSpeed;
			base.transform.Rotate(Vector3.up, 0f - num);
			global.xRotation = base.transform.localEulerAngles.y;
		}
		else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				base.transform.Rotate(Vector3.up, 90f);
				global.xRotation = base.transform.localEulerAngles.y;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				base.transform.Rotate(Vector3.up, -90f);
				global.xRotation = base.transform.localEulerAngles.y;
			}
		}
		else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && !EventSystem.current.IsPointerOverGameObject())
		{
			float num2 = Input.GetAxis("Horizontal") * rotationSpeed;
			base.transform.Rotate(Vector3.up, 0f - num2);
			global.xRotation = base.transform.localEulerAngles.y;
		}
	}
}
