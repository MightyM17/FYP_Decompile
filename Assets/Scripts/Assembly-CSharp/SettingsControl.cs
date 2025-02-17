using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
	public Slider speechRateSlider;

	public Slider difficultyLevelSlider;

	public Slider backgroundColorSlider;

	public Slider zoomSlider;

	public Slider upperJawTransparencySlider;

	public Slider lowerJawTransparencySlider;

	public Slider upperTeethTransparencySlider;

	public Slider lowerTeethTransparencySlider;

	public Slider lipsTransparencySlider;

	public Slider tongueTransparencySlider;

	public Dropdown qualityDropdown;

	public Dropdown voiceDropdown;

	public Dropdown languageDropdown;

	public Toggle upperJawToggle;

	public Toggle lowerJawToggle;

	public Toggle upperTeethToggle;

	public Toggle lowerTeethToggle;

	public Toggle lipsToggle;

	public Toggle tongueToggle;

	public Toggle airflowToggle;

	public Toggle appearanceToggle;

	public Toggle modeToggle;

	private void Awake()
	{
		global.speechRate = PlayerPrefs.GetInt("SPEECH_RATE", -5);
		global.difficultyLevel = PlayerPrefs.GetInt("DIFFICULTY_LEVEL", 0);
		global.qualityLevel = PlayerPrefs.GetInt("QUALITY_LEVEL", 1);
		global.ttsVoice = PlayerPrefs.GetInt("TTS_VOICE", 0);
		global.asrLanguage = PlayerPrefs.GetInt("ASR_LANGUAGE", 0);
		global.backgroundColor = (byte)PlayerPrefs.GetInt("BACKGROUND_COLOR", 255);
		global.zoom = PlayerPrefs.GetFloat("ZOOM", 8f);
		global.upperJawTransparency = PlayerPrefs.GetFloat("UPPER_JAW_TRANSPARENCY", 0.25f);
		global.lowerJawTransparency = PlayerPrefs.GetFloat("LOWER_JAW_TRANSPARENCY", 1f);
		global.upperTeethTransparency = PlayerPrefs.GetFloat("UPPER_TEETH_TRANSPARENCY", 0.25f);
		global.lowerTeethTransparency = PlayerPrefs.GetFloat("LOWER_TEETH_TRANSPARENCY", 1f);
		global.lipsTransparency = PlayerPrefs.GetFloat("LIPS_TRANSPARENCY", 0.7f);
		global.tongueTransparency = PlayerPrefs.GetFloat("TONGUE_TRANSPARENCY", 1f);
		global.showUpperJaw = PlayerPrefs.GetInt("SHOW_UPPER_JAW", 1) != 0;
		global.showLowerJaw = PlayerPrefs.GetInt("SHOW_LOWER_JAW", 1) != 0;
		global.showUpperTeeth = PlayerPrefs.GetInt("SHOW_UPPER_TEETH", 1) != 0;
		global.showLowerTeeth = PlayerPrefs.GetInt("SHOW_LOWER_TEETH", 1) != 0;
		global.showLips = PlayerPrefs.GetInt("SHOW_LIPS", 1) != 0;
		global.showTongue = PlayerPrefs.GetInt("SHOW_TONGUE", 1) != 0;
		global.showAirflow = PlayerPrefs.GetInt("SHOW_AIRFLOW", 1) != 0;
		global.isRealistic = PlayerPrefs.GetInt("REALISTIC", 1) != 0;
		global.isMode2D = PlayerPrefs.GetInt("MODE_2D", 0) != 0;
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("SPEECH_RATE", (int)global.speechRate);
		PlayerPrefs.SetInt("DIFFICULTY_LEVEL", (int)global.difficultyLevel);
		PlayerPrefs.SetInt("QUALITY_LEVEL", global.qualityLevel);
		PlayerPrefs.SetInt("TTS_VOICE", global.ttsVoice);
		PlayerPrefs.SetInt("ASR_LANGUAGE", global.asrLanguage);
		PlayerPrefs.SetInt("BACKGROUND_COLOR", global.backgroundColor);
		PlayerPrefs.SetFloat("ZOOM", global.zoom);
		PlayerPrefs.SetFloat("UPPER_JAW_TRANSPARENCY", global.upperJawTransparency);
		PlayerPrefs.SetFloat("LOWER_JAW_TRANSPARENCY", global.lowerJawTransparency);
		PlayerPrefs.SetFloat("UPPER_TEETH_TRANSPARENCY", global.upperTeethTransparency);
		PlayerPrefs.SetFloat("LOWER_TEETH_TRANSPARENCY", global.lowerTeethTransparency);
		PlayerPrefs.SetFloat("LIPS_TRANSPARENCY", global.lipsTransparency);
		PlayerPrefs.SetFloat("TONGUE_TRANSPARENCY", global.tongueTransparency);
		PlayerPrefs.SetInt("SHOW_UPPER_JAW", global.showUpperJaw ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_LOWER_JAW", global.showLowerJaw ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_UPPER_TEETH", global.showUpperTeeth ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_LOWER_TEETH", global.showLowerTeeth ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_LIPS", global.showLips ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_TONGUE", global.showTongue ? 1 : 0);
		PlayerPrefs.SetInt("SHOW_AIRFLOW", global.showAirflow ? 1 : 0);
		PlayerPrefs.SetInt("REALISTIC", global.isRealistic ? 1 : 0);
		PlayerPrefs.SetInt("MODE_2D", global.isMode2D ? 1 : 0);
	}

	public void OnRestoreDefaultSettings()
	{
		global.speechRate = -5L;
		speechRateSlider.value = global.speechRate;
		global.difficultyLevel = 0L;
		difficultyLevelSlider.value = global.difficultyLevel;
		global.qualityLevel = 1;
		qualityDropdown.value = global.qualityLevel;
		global.ttsVoice = 0;
		voiceDropdown.value = global.ttsVoice;
		global.asrLanguage = 0;
		languageDropdown.value = global.asrLanguage;
		global.backgroundColor = byte.MaxValue;
		backgroundColorSlider.value = (int)global.backgroundColor;
		global.zoom = 8f;
		zoomSlider.value = global.zoomMax - global.zoom;
		global.upperJawTransparency = 0.25f;
		upperJawTransparencySlider.value = global.upperJawTransparency;
		global.lowerJawTransparency = 1f;
		lowerJawTransparencySlider.value = global.lowerJawTransparency;
		global.upperTeethTransparency = 0.25f;
		upperTeethTransparencySlider.value = global.upperTeethTransparency;
		global.lowerTeethTransparency = 1f;
		lowerTeethTransparencySlider.value = global.lowerTeethTransparency;
		global.lipsTransparency = 0.7f;
		lipsTransparencySlider.value = global.lipsTransparency;
		global.tongueTransparency = 1f;
		tongueTransparencySlider.value = global.tongueTransparency;
		global.showUpperJaw = true;
		upperJawToggle.isOn = global.showUpperJaw;
		global.showLowerJaw = true;
		lowerJawToggle.isOn = global.showLowerJaw;
		global.showUpperTeeth = true;
		upperTeethToggle.isOn = global.showUpperTeeth;
		global.showLowerTeeth = true;
		lowerTeethToggle.isOn = global.showLowerTeeth;
		global.showLips = true;
		lipsToggle.isOn = global.showLips;
		global.showTongue = true;
		tongueToggle.isOn = global.showTongue;
		global.showAirflow = true;
		airflowToggle.isOn = global.showAirflow;
		global.isRealistic = true;
		appearanceToggle.isOn = global.isRealistic;
		global.isMode2D = false;
		modeToggle.isOn = global.isMode2D;
	}
}
