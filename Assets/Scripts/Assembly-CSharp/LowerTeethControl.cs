using UnityEngine;
using UnityEngine.UI;

public class LowerTeethControl : MonoBehaviour
{
	// public Toggle lowerTeethToggle;

	// public Slider lowerTeethTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		// lowerTeethToggle.isOn = global.showLowerTeeth;
		global.showLowerTeeth = true;
		// lowerTeethTransparencySlider.minValue = global.transparencyMin;
		// lowerTeethTransparencySlider.maxValue = global.transparencyMax;
		// lowerTeethTransparencySlider.value = global.lowerTeethTransparency;
		global.lowerTeethTransparency = 1f;
	}

	private void Update()
	{
		if (!global.showLowerTeeth)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.lowerTeethTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.lowerTeethTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.lowerTeethTransparency));
			prevTransparency = global.lowerTeethTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowLowerTeethToggle(bool value)
	{
		global.showLowerTeeth = value;
		base.gameObject.GetComponent<MeshRenderer>().enabled = value;
		// lowerTeethTransparencySlider.interactable = value;
	}

	public void OnLowerTeethTransparencySlider(float value)
	{
		global.lowerTeethTransparency = value;
	}
}
