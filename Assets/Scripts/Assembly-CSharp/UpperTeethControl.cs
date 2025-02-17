using UnityEngine;
using UnityEngine.UI;

public class UpperTeethControl : MonoBehaviour
{
	public Toggle UpperTeethToggle;

	public Slider UpperTeethTransparencySlider;

	private Material activeMaterial;

	private Material prevMaterial;

	private float prevTransparency;

	private void Start()
	{
		UpperTeethToggle.isOn = global.showUpperTeeth;
		UpperTeethTransparencySlider.minValue = global.transparencyMin;
		UpperTeethTransparencySlider.maxValue = global.transparencyMax;
		UpperTeethTransparencySlider.value = global.upperTeethTransparency;
	}

	private void Update()
	{
		if (!global.showUpperTeeth)
		{
			return;
		}
		activeMaterial = GetComponent<Renderer>().material;
		if (global.upperTeethTransparency != prevTransparency || activeMaterial != prevMaterial)
		{
			if (global.upperTeethTransparency < 1f)
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Fade);
			}
			else
			{
				utils.SetMaterialRenderingMode(activeMaterial, utils.BlendMode.Opaque);
			}
			Color color = activeMaterial.GetColor("_Color");
			activeMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, global.upperTeethTransparency));
			prevTransparency = global.upperTeethTransparency;
			prevMaterial = activeMaterial;
		}
	}

	public void OnShowUpperTeethToggle(bool value)
	{
		global.showUpperTeeth = value;
		base.gameObject.GetComponent<MeshRenderer>().enabled = value;
		UpperTeethTransparencySlider.interactable = value;
	}

	public void OnUpperTeethTransparencySlider(float value)
	{
		global.upperTeethTransparency = value;
	}
}
