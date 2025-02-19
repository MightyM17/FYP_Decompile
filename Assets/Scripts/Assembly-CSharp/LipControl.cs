using UnityEngine;
using UnityEngine.UI;

public class LipControl : MonoBehaviour
{
	// public Toggle lipsToggle;

	// public Slider lipsTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		// lipsToggle.isOn = global.showLips;
		global.showLips = true;
		// lipsTransparencySlider.minValue = global.transparencyMin;
		// lipsTransparencySlider.maxValue = global.transparencyMax;
		// lipsTransparencySlider.value = global.lipsTransparency;
		global.lipsTransparency = 1f;
	}

	private void Update()
	{
		if (!global.showLips)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.lipsTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.lipsTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.lipsTransparency));
			prevTransparency = global.lipsTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowLipsToggle(bool value)
	{
		global.showLips = value;
		base.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = value;
		// lipsTransparencySlider.interactable = value;
	}

	public void OnLipsTransparencySlider(float value)
	{
		global.lipsTransparency = value;
	}
}
