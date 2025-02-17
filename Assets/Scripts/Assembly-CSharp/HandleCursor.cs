using UnityEngine;
using UnityEngine.EventSystems;

public class HandleCursor : MonoBehaviour
{
	public Texture2D hand;

	public Texture2D grab;

	private Vector2 cursorHotspot = Vector2.zero;

	private void Start()
	{
		Cursor.SetCursor(hand, cursorHotspot, CursorMode.Auto);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			global.grabbed = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			global.grabbed = false;
		}
		if (EventSystem.current.IsPointerOverGameObject())
		{
			Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto);
		}
		else if (global.grabbed)
		{
			Cursor.SetCursor(grab, cursorHotspot, CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor(hand, cursorHotspot, CursorMode.Auto);
		}
	}
}
