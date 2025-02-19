using UnityEngine;
using UnityEngine.UI;

public class TongueControl : MonoBehaviour
{
	// public Toggle tongueToggle;

	// public Slider tongueTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		// tongueToggle.isOn = global.showTongue;
		global.showTongue = true;
		// tongueTransparencySlider.minValue = global.transparencyMin;
		// tongueTransparencySlider.maxValue = global.transparencyMax;
		// tongueTransparencySlider.value = global.tongueTransparency;
		global.tongueTransparency = 1f;
	}

	private void Update()
	{
		if (!global.showTongue)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.tongueTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.tongueTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.tongueTransparency));
			prevTransparency = global.tongueTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowTongueToggle(bool value)
	{
		global.showTongue = value;
		base.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = value;
		// tongueTransparencySlider.interactable = value;
	}

	public void OnTongueTransparencySlider(float value)
	{
		global.tongueTransparency = value;
	}
}
