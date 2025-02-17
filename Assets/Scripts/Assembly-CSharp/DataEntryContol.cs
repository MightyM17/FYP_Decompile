using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataEntryContol : MonoBehaviour
{
	public InputField textCtrl;

	public Button recordButton;

	public Button playButton;

	private void Update()
	{
		if (Input.GetKey(KeyCode.Return) && textCtrl.text != "" && (bool)EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.name == textCtrl.name)
		{
			recordButton.onClick.Invoke();
			playButton.onClick.Invoke();
		}
	}

	public void OnPaste()
	{
		textCtrl.text = GUIUtility.systemCopyBuffer;
	}
}
