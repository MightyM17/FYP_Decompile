using UnityEngine;

public class global
{
	public const long SPEECH_RATE = -5L;

	public const long DIFFICULTY_LEVEL = 0L;

	public const int QUALITY_LEVEL = 1;

	public const int TTS_VOICE = 0;

	public const int ASR_LANGUAGE = 0;

	public const byte BACKGROUND_COLOR = byte.MaxValue;

	public const float ZOOM = 8f;

	public const float UPPER_JAW_TRANSPARENCY = 0.25f;

	public const float LOWER_JAW_TRANSPARENCY = 1f;

	public const float UPPER_TEETH_TRANSPARENCY = 0.25f;

	public const float LOWER_TEETH_TRANSPARENCY = 1f;

	public const float LIPS_TRANSPARENCY = 0.7f;

	public const float TONGUE_TRANSPARENCY = 1f;

	public const bool SHOW_UPPER_JAW = true;

	public const bool SHOW_LOWER_JAW = true;

	public const bool SHOW_UPPER_TEETH = true;

	public const bool SHOW_LOWER_TEETH = true;

	public const bool SHOW_LIPS = true;

	public const bool SHOW_TONGUE = true;

	public const bool SHOW_AIRFLOW = true;

	public const bool REALISTIC = true;

	public const bool MODE_2D = false;

	public static float xRotation = 90f;

	public static float yRotation = 0f;

	public static long phoneme = 0L;

	public static long duration = 0L;

	public static long speechRateMin = -10L;

	public static long speechRateMax = 10L;

	public static long speechRate = -5L;

	public static long difficultyLevelMin = 0L;

	public static long difficultyLevelMax = 2L;

	public static long difficultyLevel = 0L;

	public static float animationSpeed = 2f;

	public static int qualityLevel = 1;

	public static int ttsVoice = 0;

	public static int asrLanguage = 0;

	public static byte backgroundColorMin = 0;

	public static byte backgroundColorMax = byte.MaxValue;

	public static byte backgroundColor = byte.MaxValue;

	public static float zoomMin = 1f;

	public static float zoomMax = 10f;

	public static float zoom = 8f;

	public static float transparencyMin = 0f;

	public static float transparencyMax = 1f;

	public static float upperJawTransparency = 0.25f;

	public static float lowerJawTransparency = 1f;

	public static float upperTeethTransparency = 0.25f;

	public static float lowerTeethTransparency = 1f;

	public static float lipsTransparency = 0.7f;

	public static float tongueTransparency = 1f;

	public static bool showUpperJaw = true;

	public static bool showLowerJaw = true;

	public static bool showUpperTeeth = true;

	public static bool showLowerTeeth = true;

	public static bool showLips = true;

	public static bool showTongue = true;

	public static bool showAirflow = true;

	public static bool isRealistic = true;

	public static bool isMode2D = false;

	public static bool grabbed = false;

	public static string version = "Version " + Application.version;
}
