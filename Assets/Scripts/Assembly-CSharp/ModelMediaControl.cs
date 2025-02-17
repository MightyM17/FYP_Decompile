using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModelMediaControl : MonoBehaviour
{
	public enum MediaState
	{
		uninitialised = 0,
		initialised = 1,
		stopped = 2,
		paused = 3,
		playing = 4
	}

	private List<string> ipaStrings = new List<string>
	{
		"/./", "/./", "/./", "/./", "/./", "/./", "/./", "/./", "/./", "/./",
		"/ɑ/", "/æ/", "/ʌ/", "/ɔ/", "/ɑʊ/", "/ə/", "/ɑɪ/", "/b/", "/ʧ/", "/d/",
		"/ð/", "/ɛ/", "/ɚ/", "/e/", "/f/", "/g/", "/h/", "/ɪ/", "/i/", "/ʤ/",
		"/k/", "/l/", "/m/", "/n/", "/ŋ/", "/o/", "/ɔɪ/", "/p/", "/r/", "/s/",
		"/ʃ/", "/t/", "/ɵ/", "/ʊ/", "/u/", "/v/", "/w/", "/j/", "/z/", "/ʒ/",
		"/p/", "/b/", "/ʧ/", "/ʤ/", "/d/", "/t/", "/g/", "/k/"
	};

	private List<string> sapiStrings = new List<string>
	{
		"/aa/", "/ae/", "/ah/", "/ao/", "/aw/", "/ax/", "/ay/", "/b/", "/ch/", "/d/",
		"/dh/", "/eh/", "/er/", "/ey/", "/f/", "/g/", "/h/", "/ih/", "/iy/", "/jh/",
		"/k/", "/l/", "/m/", "/n/", "/ng/", "/ow/", "/oy/", "/p/", "/r/", "/s/",
		"/sh/", "/t/", "/th/", "/uh/", "/uw/", "/v/", "/w/", "/y/", "/z/", "/zh/"
	};

	private MediaState mediaState;

	private float startTime;

	public Text counter;

	public Text prompt;

	public Button playButton;

	public Button stopButton;

	public Slider progressSlider;

	public Slider speechRateSlider;

	public InputField textCtrl;

	public Dropdown voiceDropdown;

	public RawImage waveform;

	public Toggle ipaToggle;

	public Text ipaText;

	public GameObject sapiErrorDlg;

	public GameObject dataEntryErrorDlg;

	public GameObject evaluationWatermark;

	public GameObject model3D;

	public GameObject model2D;

	public Toggle modeToggle;

	private long mediaDurationMs;

	private long mediaPositionMs;

	private static readonly int maxString = 260;

	private StringBuilder voice = new StringBuilder(maxString);

	private StringBuilder waveformImageFileName = new StringBuilder(maxString);

	private List<string> installedVoices = new List<string>();

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsInit(string applicationPath);

	[DllImport("pctts")]
	private static extern void ttsReset();

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsRecord(string text);

	[DllImport("pctts")]
	private static extern bool ttsPlay();

	[DllImport("pctts")]
	private static extern bool ttsPause();

	[DllImport("pctts")]
	private static extern bool ttsStop();

	[DllImport("pctts")]
	private static extern bool ttsNext();

	[DllImport("pctts")]
	private static extern bool ttsPrevious();

	[DllImport("pctts")]
	private static extern void ttsSetMediaPosition(long mediaPositionMs);

	[DllImport("pctts")]
	private static extern long ttsGetMediaPosition();

	[DllImport("pctts")]
	private static extern long ttsGetMediaDuration();

	[DllImport("pctts")]
	private static extern int ttsGetMediaState();

	[DllImport("pctts")]
	private static extern long ttsGetPhonemeFromMediaPosition(long mediaPositionMs);

	[DllImport("pctts")]
	private static extern bool ttsSetSpeechRate(long speechRate);

	[DllImport("pctts")]
	private static extern long ttsGetSpeechRate();

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsGetVoice(StringBuilder voice);

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsGetInstalledVoice(StringBuilder voice, int index);

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsSetVoice(string voice);

	[DllImport("pctts", CharSet = CharSet.Unicode)]
	private static extern bool ttsGetWaveformImageFileName(StringBuilder fileName);

	private void Start()
	{
		if (ttsInit(Application.dataPath + "/"))
		{
			Debug.Log("TTS Init");
			speechRateSlider.minValue = global.speechRateMin;
			speechRateSlider.maxValue = global.speechRateMax;
			speechRateSlider.value = global.speechRate;
			ttsSetSpeechRate(global.speechRate);
			int num = 0;
			while (ttsGetInstalledVoice(voice, num++))
			{
				installedVoices.Add(voice.ToString());
			}
		}
		if (installedVoices.Count == 0)
		{
			installedVoices.Add("Not installed");
			ipaToggle.isOn = true;
			sapiErrorDlg.SetActive(value: true);
		}
		global.ttsVoice = Mathf.Max(Mathf.Min(global.ttsVoice, installedVoices.Count - 1), 0);
		voiceDropdown.AddOptions(installedVoices);
		voiceDropdown.value = global.ttsVoice;
		ttsSetVoice(voiceDropdown.options[global.ttsVoice].text);
		modeToggle.isOn = global.isMode2D;
		if (LicenseManager.IsEvaluationLicense())
		{
			evaluationWatermark.SetActive(value: true);
		}
	}

	private void OnApplicationQuit()
	{
		ttsReset();
		Debug.Log("TTS Reset");
	}

	private void Update()
	{
		if (mediaState != 0)
		{
			mediaPositionMs = ttsGetMediaPosition();
			long num = ttsGetPhonemeFromMediaPosition(mediaPositionMs);
			global.phoneme = num & 0xFFFF;
			global.duration = (num >> 16) & 0xFFFF;
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(mediaPositionMs);
			ipaText.text = ipaStrings[(int)global.phoneme];
			counter.text = $"{timeSpan.Hours:0}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
			if (EventSystem.current.currentSelectedGameObject != progressSlider.gameObject)
			{
				progressSlider.value = mediaPositionMs;
			}
			if (ttsGetMediaState() == 2)
			{
				stopButton.onClick.Invoke();
			}
		}
	}

	public void OnRecord()
	{
		if (textCtrl.text == "")
		{
			dataEntryErrorDlg.SetActive(value: true);
		}
		else if (ttsRecord(textCtrl.text))
		{
			if (ttsGetWaveformImageFileName(waveformImageFileName))
			{
				mediaDurationMs = ttsGetMediaDuration();
				progressSlider.minValue = 0f;
				progressSlider.maxValue = mediaDurationMs;
				progressSlider.value = 0f;
				progressSlider.GetComponentInChildren<Image>().enabled = true;
				waveform.texture = utils.LoadWaveformImage(waveformImageFileName.ToString());
			}
			progressSlider.gameObject.SetActive(value: true);
			prompt.gameObject.SetActive(value: false);
			playButton.interactable = true;
			mediaState = MediaState.initialised;
		}
	}

	public void OnPlay()
	{
		if (ttsPlay())
		{
			mediaState = MediaState.playing;
		}
	}

	public void OnPause()
	{
		if (ttsPause())
		{
			mediaState = MediaState.paused;
		}
	}

	public void OnStop()
	{
		if (ttsStop())
		{
			mediaState = MediaState.stopped;
		}
	}

	public void OnPrevious()
	{
		ttsPrevious();
	}

	public void OnNext()
	{
		ttsNext();
	}

	public void OnProgressSlider(float value)
	{
		if (EventSystem.current.currentSelectedGameObject == progressSlider.gameObject)
		{
			mediaPositionMs = (long)Mathf.Min(value, mediaDurationMs);
			ttsSetMediaPosition(mediaPositionMs);
		}
	}

	public void OnProgressSliderRelease()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void OnSpeechRateSlider()
	{
		global.speechRate = (long)speechRateSlider.value;
		ttsSetSpeechRate(global.speechRate);
	}

	public void OnSpeechRateSliderRelease()
	{
		if (mediaState != 0)
		{
			stopButton.onClick.Invoke();
			OnRecord();
		}
	}

	public void OnVoiceSelection(int index)
	{
		global.ttsVoice = index;
		ttsSetVoice(voiceDropdown.options[index].text);
	}

	public void OnIPA()
	{
		if (int.TryParse(Regex.Match(EventSystem.current.currentSelectedGameObject.name, "\\d+").Value, out var result))
		{
			textCtrl.text = sapiStrings[result];
			OnRecord();
			OnPlay();
		}
	}

	public void OnToggle2D(bool activate)
	{
		global.isMode2D = activate;
		model2D.SetActive(activate);
		model3D.SetActive(!activate);
	}
}
