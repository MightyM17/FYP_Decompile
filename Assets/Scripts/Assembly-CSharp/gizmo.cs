using UnityEngine;

public class gizmo : MonoBehaviour
{
	private float size = 0.1f;

	private Color colourDefault = Color.blue;

	private Color colourSelected = Color.green;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = colourSelected;
		Gizmos.DrawSphere(base.transform.position, size);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = colourDefault;
		Gizmos.DrawSphere(base.transform.position, size);
	}
}
