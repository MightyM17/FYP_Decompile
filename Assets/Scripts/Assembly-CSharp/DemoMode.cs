using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoMode : MonoBehaviour
{
	private List<string> sapiStrings = new List<string>
	{
		"/aa/", "/ae/", "/ah/", "/ao/", "/aw/", "/ax/", "/ay/", "/b/", "/ch/", "/d/",
		"/dh/", "/eh/", "/er/", "/ey/", "/f/", "/g/", "/h/", "/ih/", "/iy/", "/jh/",
		"/k/", "/l/", "/m/", "/n/", "/ng/", "/ow/", "/oy/", "/p/", "/r/", "/s/",
		"/sh/", "/t/", "/th/", "/uh/", "/uw/", "/v/", "/w/", "/y/", "/z/", "/zh/"
	};

	public GameObject rotateObject;

	public InputField textCtrl;

	public Button recordButton;

	public Button playButton;

	public Button forwardButton;

	public int rotationSpeed = 10;

	private bool rotate;

	private bool rotating;

	private int rotations;

	private int phoneme = 20;

	private int number;

	private KeyCode key;

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.F1))
		{
			rotate = !rotate;
			rotations = 0;
			key = KeyCode.F1;
		}
		else if (Input.GetKeyUp(KeyCode.F2))
		{
			rotate = !rotate;
			rotations = 0;
			key = KeyCode.F2;
		}
		else if (Input.GetKeyUp(KeyCode.F3))
		{
			rotate = !rotate;
			rotations = 0;
			key = KeyCode.F3;
		}
		else if (Input.GetKeyUp(KeyCode.F4))
		{
			rotate = !rotate;
			rotations = 0;
			key = KeyCode.F4;
		}
		if (rotate && !rotating)
		{
			if (key == KeyCode.F1)
			{
				StartCoroutine(Rotate(rotationSpeed));
			}
			else if (key == KeyCode.F2)
			{
				StartCoroutine(Rotate(rotationSpeed));
			}
			else if (key == KeyCode.F3)
			{
				textCtrl.text = sapiStrings[phoneme++ % sapiStrings.Count];
				recordButton.onClick.Invoke();
				forwardButton.onClick.Invoke();
				StartCoroutine(Rotate(rotationSpeed));
			}
			else if (key == KeyCode.F4)
			{
				textCtrl.text = number++.ToString();
				recordButton.onClick.Invoke();
				playButton.onClick.Invoke();
				StartCoroutine(Rotate(rotationSpeed));
			}
		}
	}

	private IEnumerator Rotate(float duration)
	{
		float startRotation = rotateObject.transform.eulerAngles.y;
		float endRotation = startRotation + 360f;
		float t = 0f;
		rotating = true;
		while (t < duration)
		{
			t += Time.deltaTime;
			float y = Mathf.Lerp(startRotation, endRotation, t / duration) % 360f;
			rotateObject.transform.eulerAngles = new Vector3(rotateObject.transform.eulerAngles.x, y, rotateObject.transform.eulerAngles.z);
			if (key == KeyCode.F2)
			{
				switch (rotations)
				{
				case 1:
					global.lipsTransparency = 1f - t / duration;
					break;
				case 2:
					global.lipsTransparency = 0f;
					global.upperJawTransparency = 1f - t / duration;
					global.lowerJawTransparency = 1f - t / duration;
					break;
				case 3:
					global.upperJawTransparency = 0f;
					global.lowerJawTransparency = 0f;
					global.upperTeethTransparency = 1f - t / duration;
					global.lowerTeethTransparency = 1f - t / duration;
					break;
				default:
					global.upperTeethTransparency = 0f;
					global.lowerTeethTransparency = 0f;
					break;
				case 0:
					break;
				}
			}
			yield return null;
		}
		rotating = false;
		rotations++;
	}
}
