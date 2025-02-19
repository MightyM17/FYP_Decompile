using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomObject : MonoBehaviour
{
	private readonly float sensitivity = 2f;

	// public Slider zoomSlider;

	private void Start()
	{
		// zoomSlider.minValue = global.zoomMin;
		// zoomSlider.maxValue = global.zoomMax - 1f;
		// zoomSlider.value = global.zoomMax - global.zoom;
		Camera.main.orthographicSize = global.zoom;
	}

	private void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			// zoomSlider.value += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
			// zoomSlider.value = Mathf.Clamp(zoomSlider.value, global.zoomMin, global.zoomMax - 1f);
		}
	}

	public void OnZoomSlider(float value)
	{
		global.zoom = global.zoomMax - value;
		Camera.main.orthographicSize = global.zoom;
	}
}
