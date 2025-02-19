using UnityEngine;
using UnityEngine.UI;

public class LowerJawControl : MonoBehaviour
{
	// public Toggle lowerJawToggle;

	// public Slider lowerJawTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		// lowerJawToggle.isOn = global.showLowerJaw;.
		global.showLowerJaw = true;
		// lowerJawTransparencySlider.minValue = global.transparencyMin;
		// lowerJawTransparencySlider.maxValue = global.transparencyMax;
		// lowerJawTransparencySlider.value = global.lowerJawTransparency;
		global.lowerJawTransparency = 1f;
	}

	private void Update()
	{
		if (!global.showLowerJaw)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.lowerJawTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.lowerJawTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.lowerJawTransparency));
			prevTransparency = global.lowerJawTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowLowerJawToggle(bool value)
	{
		global.showLowerJaw = value;
		base.gameObject.GetComponent<MeshRenderer>().enabled = value;
		// lowerJawTransparencySlider.interactable = value;
	}

	public void OnLowerJawTransparencySlider(float value)
	{
		global.lowerJawTransparency = value;
	}
}
