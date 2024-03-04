using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatItem : MonoBehaviour
{
	[SerializeField] private TMP_Text textStat;


	public void SetText(string text)
	{
		textStat.text = text;
	}
}
