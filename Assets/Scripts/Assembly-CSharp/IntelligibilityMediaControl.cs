using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntelligibilityMediaControl : MonoBehaviour
{
	public enum MediaState
	{
		uninitialised = 0,
		initialised = 1,
		stopped = 2,
		paused = 3,
		playing = 4,
		recording = 5
	}

	private MediaState mediaState;

	private float startTime;

	public Text counter;

	public Text prompt;

	public Button playButton;

	public Button stopButton;

	public Button recordButton;

	public Button microphoneButton;

	public Slider progressSlider;

	// public Slider difficultyLevelSlider;

	public InputField textCtrl;

	public InputField resultCtrl;

	public InputField scoreCtrl;

	// public Dropdown languageDropdown;

	public RawImage waveform;

	public Image recoResultImage;

	public Text scoreText;

	public Sprite[] flags;

	public Sprite[] recordButtonImage;

	public Sprite[] recoResultImages;

	public GameObject sapiErrorDlg;

	public GameObject dataEntryErrorDlg;

	private long mediaDurationMs;

	private long mediaPositionMs;

	private static readonly int maxString = 260;

	private StringBuilder waveformImageFileName = new StringBuilder(maxString);

	private StringBuilder recoResultText = new StringBuilder(maxString);

	private List<Dropdown.OptionData> installedLanguages = new List<Dropdown.OptionData>();

	private string[,] supportedLanguages = new string[2, 2]
	{
		{ "409", "US English" },
		{ "809", "UK English" }
	};

	private short score;

	private bool match;

	private bool activated;

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrInit(string applicationPath);

	[DllImport("pcasr")]
	private static extern void asrReset();

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrRecord(string text);

	[DllImport("pcasr")]
	private static extern bool asrPlay();

	[DllImport("pcasr")]
	private static extern bool asrPause();

	[DllImport("pcasr")]
	private static extern bool asrStop();

	[DllImport("pcasr")]
	private static extern bool asrNext();

	[DllImport("pcasr")]
	private static extern bool asrPrevious();

	[DllImport("pcasr")]
	private static extern void asrSetMediaPosition(long mediaPositionMs);

	[DllImport("pcasr")]
	private static extern long asrGetMediaPosition();

	[DllImport("pcasr")]
	private static extern long asrGetMediaDuration();

	[DllImport("pcasr")]
	private static extern int asrGetMediaState();

	[DllImport("pcasr")]
	private static extern void asrSetDifficulty(long difficultyLevel);

	[DllImport("pcasr")]
	private static extern long asrGetDifficulty();

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrGetLanguage(StringBuilder language);

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrSetLanguage(string language);

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrIsLanguage(string language);

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrGetResult(StringBuilder text, out short score, out bool match);

	[DllImport("pcasr", CharSet = CharSet.Unicode)]
	private static extern bool asrGetWaveformImageFileName(StringBuilder fileName);

	[DllImport("pcasr")]
	private static extern bool asrGetEndpointerState();

	private void Start()
	{
		if (asrInit(Application.dataPath + "/"))
		{
			Debug.Log("ASR Init");
			// difficultyLevelSlider.minValue = global.difficultyLevelMin;
			// difficultyLevelSlider.maxValue = global.difficultyLevelMax;
			// difficultyLevelSlider.value = global.difficultyLevel;
			asrSetDifficulty(global.difficultyLevel);
			for (int i = 0; i < supportedLanguages.Length / 2; i++)
			{
				if (asrIsLanguage(supportedLanguages[i, 0]))
				{
					Dropdown.OptionData item = new Dropdown.OptionData(supportedLanguages[i, 1], flags[i]);
					installedLanguages.Add(item);
				}
			}
		}
		if (installedLanguages.Count == 0)
		{
			Dropdown.OptionData item2 = new Dropdown.OptionData("Not installed");
			installedLanguages.Add(item2);
			sapiErrorDlg.SetActive(value: true);
		}
		global.asrLanguage = Mathf.Max(Mathf.Min(global.asrLanguage, installedLanguages.Count - 1), 0);
		// languageDropdown.AddOptions(installedLanguages);
		// languageDropdown.value = global.asrLanguage;
		asrSetLanguage(supportedLanguages[global.asrLanguage, 0]);
	}

	private void OnApplicationQuit()
	{
		asrReset();
		Debug.Log("ASR Reset");
	}

	private void Update()
	{
		if (mediaState == MediaState.recording)
		{
			if (asrGetEndpointerState())
			{
				stopButton.onClick.Invoke();
			}
		}
		else if (mediaState != 0)
		{
			mediaPositionMs = asrGetMediaPosition();
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(mediaPositionMs);
			counter.text = $"{timeSpan.Hours:0}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
			if (EventSystem.current.currentSelectedGameObject != progressSlider.gameObject)
			{
				progressSlider.value = mediaPositionMs;
			}
			if (asrGetMediaState() == 2)
			{
				stopButton.onClick.Invoke();
			}
		}
		if (asrGetResult(recoResultText, out score, out match))
		{
			Debug.Log(recoResultText.ToString() + score + match.ToString());
			resultCtrl.text = recoResultText.ToString();
			scoreCtrl.text = score + "%";
			scoreText.text = score.ToString();
			recoResultImage.enabled = true;
			recoResultImage.sprite = recoResultImages[(!match) ? 1u : 0u];
		}
	}

	public void OnRecord()
	{
		if (textCtrl.text == "")
		{
			dataEntryErrorDlg.SetActive(value: true);
		}
		else if (textCtrl.text.Length > 0 && asrRecord(textCtrl.text))
		{
			prompt.resizeTextMaxSize = 100;
			prompt.text = textCtrl.text;
			resultCtrl.text = "No result";
			scoreCtrl.text = "0%";
			scoreText.text = "0";
			recordButton.GetComponent<Image>().sprite = recordButtonImage[1];
			recordButton.interactable = false;
			recoResultImage.enabled = false;
			playButton.interactable = true;
			progressSlider.gameObject.SetActive(value: false);
			prompt.gameObject.SetActive(value: true);
			mediaState = MediaState.recording;
		}
	}

	public void OnPlay()
	{
		if (asrPlay())
		{
			mediaState = MediaState.playing;
		}
	}

	public void OnPause()
	{
		if (asrPause())
		{
			mediaState = MediaState.paused;
		}
	}

	public void OnStop()
	{
		if (asrStop())
		{
			if (asrGetWaveformImageFileName(waveformImageFileName))
			{
				mediaDurationMs = asrGetMediaDuration();
				progressSlider.minValue = 0f;
				progressSlider.maxValue = mediaDurationMs;
				progressSlider.value = 0f;
				progressSlider.GetComponentInChildren<Image>().enabled = true;
				waveform.texture = utils.LoadWaveformImage(waveformImageFileName.ToString());
			}
			recordButton.GetComponent<Image>().sprite = recordButtonImage[0];
			recordButton.interactable = true;
			progressSlider.gameObject.SetActive(value: true);
			prompt.gameObject.SetActive(value: false);
			mediaState = MediaState.stopped;
		}
	}

	public void OnPrevious()
	{
		asrPrevious();
	}

	public void OnNext()
	{
		asrNext();
	}

	public void OnProgressSlider(float value)
	{
		if (EventSystem.current.currentSelectedGameObject == progressSlider.gameObject)
		{
			mediaPositionMs = (long)Mathf.Min(value, mediaDurationMs);
			asrSetMediaPosition(mediaPositionMs);
		}
	}

	public void OnProgressSliderRelease()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}

	// public void OndifficultyLevelSlider()
	// {
	// 	global.difficultyLevel = (short)difficultyLevelSlider.value;
	// 	asrSetDifficulty(global.difficultyLevel);
	// }

	public void OnLanguageSelection(int index)
	{
		global.asrLanguage = index;
		asrSetLanguage(supportedLanguages[index, 0]);
	}

	public void OnActivate()
	{
		if (!activated)
		{
			microphoneButton.onClick.Invoke();
			activated = true;
		}
	}
}
