using UnityEngine;
using UnityEngine.UI;

public class SetMaterial : MonoBehaviour
{
	public GameObject lowerJaw;

	public GameObject upperJaw;

	public GameObject lowerTeeth;

	public GameObject upperTeeth;

	public GameObject tongue;

	public GameObject lips;

	public Material realisticMouth;

	public Material stylisedMouth;

	public Material realisticLips;

	public Material stylisedLips;

	// public Toggle appearanceToggle;

	// private void Start()
	// {
	// 	appearanceToggle.isOn = global.isRealistic;
	// }

	public void OnRealisticToggle(bool isRealistic)
	{
		global.isRealistic = isRealistic;
		Renderer component = lowerJaw.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticMouth : stylisedMouth);
		}
		component = upperJaw.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticMouth : stylisedMouth);
		}
		component = lowerTeeth.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticMouth : stylisedMouth);
		}
		component = upperTeeth.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticMouth : stylisedMouth);
		}
		component = tongue.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticMouth : stylisedMouth);
		}
		component = lips.GetComponent<Renderer>();
		if (component != null)
		{
			component.material = (isRealistic ? realisticLips : stylisedLips);
		}
	}
}
