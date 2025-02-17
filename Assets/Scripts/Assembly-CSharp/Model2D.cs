using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Model2D : MonoBehaviour
{
	public Image lipImage;

	public Image anatomyImage;

	public Image tpcImage;

	private long phoneme;

	private static readonly long spriteCount = 58L;

	private List<Sprite> anatomySprites = new List<Sprite>();

	private List<Sprite> lipSprites = new List<Sprite>();

	private List<Sprite> tpcSprites = new List<Sprite>();

	private void Start()
	{
		for (int i = 0; i < spriteCount; i++)
		{
			lipSprites.Add(Resources.Load<Sprite>("lips/" + i));
			anatomySprites.Add(Resources.Load<Sprite>("anatomy/" + i));
			tpcSprites.Add(Resources.Load<Sprite>("tongue-palate-contact/" + i));
		}
	}

	private void Update()
	{
		if (phoneme != global.phoneme)
		{
			lipImage.sprite = lipSprites[(int)global.phoneme];
			anatomyImage.sprite = anatomySprites[(int)global.phoneme];
			tpcImage.sprite = tpcSprites[(int)global.phoneme];
			phoneme = global.phoneme;
		}
	}
}
