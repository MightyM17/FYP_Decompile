using UnityEngine;
using UnityEngine.UI;

public class AirflowControl : MonoBehaviour
{
	// public Toggle airflowToggle;

	public ParticleSystem nasalAirflow;

	public ParticleSystem oralAirflow;

	private ParticleSystemRenderer nasalAirflowRenderer;

	private ParticleSystemRenderer oralAirflowRenderer;

	private ParticleSystem.TrailModule trails;

	private ParticleSystem.ShapeModule shape;

	private ParticleSystem.CollisionModule collision;

	private long phoneme;

	private void Start()
	{
		// airflowToggle.isOn = global.showAirflow;
		global.showAirflow = true;
		nasalAirflowRenderer = nasalAirflow.GetComponent<ParticleSystemRenderer>();
		oralAirflowRenderer = oralAirflow.GetComponent<ParticleSystemRenderer>();
		trails = oralAirflow.trails;
		shape = oralAirflow.shape;
		collision = oralAirflow.collision;
	}

	private void OnEnable()
	{
		if (global.showAirflow)
		{
			nasalAirflow.Play();
			oralAirflow.Play();
		}
		else
		{
			nasalAirflow.Stop();
			oralAirflow.Stop();
		}
	}

	private void Update()
	{
		if (phoneme != global.phoneme)
		{
			SetPath();
			SetTrails();
			SetShape();
			SetCollider();
			phoneme = global.phoneme;
		}
	}

	public void TestingFunc() {
		SetPath();
			SetTrails();
			SetShape();
			SetCollider();
			phoneme = global.phoneme;
	}

	private void SetPath()
	{
		long num = global.phoneme;
		if (num <= 19)
		{
			if (num == 7 || (ulong)(num - 17) <= 2uL)
			{
				goto IL_006d;
			}
		}
		else
		{
			long num2 = num - 25;
			if ((ulong)num2 <= 12uL)
			{
				switch (num2)
				{
				case 0L:
				case 4L:
				case 5L:
				case 12L:
					goto IL_006d;
				case 7L:
				case 8L:
				case 9L:
					nasalAirflowRenderer.enabled = true;
					oralAirflowRenderer.enabled = false;
					return;
				case 1L:
				case 2L:
				case 3L:
				case 6L:
				case 10L:
				case 11L:
					goto IL_009f;
				}
			}
			if (num == 41)
			{
				goto IL_006d;
			}
		}
		goto IL_009f;
		IL_009f:
		nasalAirflowRenderer.enabled = false;
		oralAirflowRenderer.enabled = true;
		return;
		IL_006d:
		nasalAirflowRenderer.enabled = false;
		oralAirflowRenderer.enabled = false;
	}

	private void SetTrails()
	{
		switch (global.phoneme)
		{
		case 24L:
		case 26L:
		case 39L:
		case 40L:
		case 42L:
		case 50L:
		case 52L:
		case 55L:
		case 57L:
			trails.enabled = true;
			oralAirflowRenderer.renderMode = ParticleSystemRenderMode.None;
			break;
		default:
			trails.enabled = false;
			oralAirflowRenderer.renderMode = ParticleSystemRenderMode.Billboard;
			break;
		}
	}

	private void SetShape()
	{
		switch (global.phoneme)
		{
		case 39L:
		case 48L:
			shape.scale = new Vector3(1f, 1f, 1f);
			break;
		case 40L:
		case 49L:
			shape.scale = new Vector3(1f, 2f, 1f);
			break;
		default:
			shape.scale = new Vector3(1f, 3f, 1f);
			break;
		}
	}

	private void SetCollider()
	{
		long num = global.phoneme;
		if (num == 31)
		{
			collision.enabled = true;
		}
		else
		{
			collision.enabled = false;
		}
	}

	public void OnShowAirflowToggle(bool value)
	{
		global.showAirflow = value;
		if (global.showAirflow)
		{
			nasalAirflow.Play();
			oralAirflow.Play();
		}
		else
		{
			nasalAirflow.Stop();
			oralAirflow.Stop();
		}
	}
}
