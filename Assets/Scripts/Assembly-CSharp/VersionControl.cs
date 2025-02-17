using UnityEngine;
using UnityEngine.UI;

public class VersionControl : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.GetComponent<Text>().text = global.version;
	}
}
