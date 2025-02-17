using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
	private int width;

	private int height;

	[DllImport("pcasr")]
	private static extern bool asrAudioSettings();

	public void OnToggleFullScreen(bool activate)
	{
		if (activate)
		{
			width = Screen.width;
			height = Screen.height;
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
		}
		else
		{
			Screen.SetResolution(width, height, FullScreenMode.Windowed);
		}
	}

	public void OnHelpButton()
	{
		Process.Start(Application.productName + " help.chm");
	}

	public void OnSpeakerButton()
	{
		Process.Start(Environment.GetEnvironmentVariable("windir") + "\\system32\\sndvol.exe");
	}

	public void OnMicrophoneButton()
	{
		asrAudioSettings();
	}
}
