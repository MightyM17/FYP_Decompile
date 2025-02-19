using UnityEngine;
using UnityEngine.UI;

public class UpperJawControl : MonoBehaviour
{
	// public Toggle upperJawToggle;

	// public Slider upperJawTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		// upperJawToggle.isOn = global.showUpperJaw;
		global.showUpperJaw = true;
		// upperJawTransparencySlider.minValue = global.transparencyMin;
		// upperJawTransparencySlider.maxValue = global.transparencyMax;
		// upperJawTransparencySlider.value = global.upperJawTransparency;
		global.upperJawTransparency = 1f;
	}

	private void Update()
	{
		if (!global.showUpperJaw)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.upperJawTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.upperJawTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.upperJawTransparency));
			prevTransparency = global.upperJawTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowUpperJawToggle(bool value)
	{
		global.showUpperJaw = value;
		base.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = value;
		// upperJawTransparencySlider.interactable = value;
	}

	public void OnUpperJawTransparencySlider(float value)
	{
		global.upperJawTransparency = value;
	}
}
