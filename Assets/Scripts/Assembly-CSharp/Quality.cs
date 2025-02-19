using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quality : MonoBehaviour
{
	public Dropdown qualityDropdown;

	private void Start()
	{
		List<string> options = new List<string>(QualitySettings.names);
		qualityDropdown.AddOptions(options);
		qualityDropdown.value = global.qualityLevel;
		QualitySettings.SetQualityLevel(global.qualityLevel);
	}

	public void OnQualitySelection(int index)
	{
		global.qualityLevel = index;
		QualitySettings.SetQualityLevel(global.qualityLevel);
		Debug.Log("Quality is " + global.qualityLevel);
	}
}
