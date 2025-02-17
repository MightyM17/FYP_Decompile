using UnityEngine;
using UnityEngine.UI;

public class SetBackgroundColour : MonoBehaviour
{
	public Slider backgroundColorSlider;

	private void Start()
	{
		backgroundColorSlider.minValue = (int)global.backgroundColorMin;
		backgroundColorSlider.maxValue = (int)global.backgroundColorMax;
		backgroundColorSlider.value = (int)global.backgroundColor;
		Camera.main.backgroundColor = new Color32(global.backgroundColor, global.backgroundColor, global.backgroundColor, byte.MaxValue);
	}

	public void OnBackgroundColorSlider(float value)
	{
		global.backgroundColor = (byte)value;
		Camera.main.backgroundColor = new Color32(global.backgroundColor, global.backgroundColor, global.backgroundColor, byte.MaxValue);
	}
}
