using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LicenseManager : MonoBehaviour
{
	[DllImport("pclm")]
	private static extern bool InitLicense();

	[DllImport("pclm")]
	private static extern void ResetLicense();

	[DllImport("pclm")]
	private static extern bool EvaluationLicense();

	private void Awake()
	{
		if (!_InitLicense())
		{
			Debug.Log("License manager returned false");
			QuitApplication();
		}
	}

	private void OnDestroy()
	{
		_ResetLicense();
		Debug.Log("Application quit");
	}

	private bool _InitLicense()
	{
		try
		{
			return InitLicense();
		}
		catch (DllNotFoundException)
		{
			Debug.Log("license manager DLL not found");
			return false;
		}
	}

	private void _ResetLicense()
	{
		try
		{
			ResetLicense();
		}
		catch (DllNotFoundException)
		{
			Debug.Log("license manager DLL not found");
		}
	}

	private void QuitApplication()
	{
		Application.Quit();
	}

	public static bool IsEvaluationLicense()
	{
		try
		{
			return EvaluationLicense();
		}
		catch (DllNotFoundException)
		{
			Debug.Log("license manager DLL not found");
			return false;
		}
	}
}
