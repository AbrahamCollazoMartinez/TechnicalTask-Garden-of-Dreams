using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LifeElement : MonoBehaviour
{

	[SerializeField] private Slider slider;
	[SerializeField] private TMP_Text percentage;

	private float lifeAmount;
	public int amountLife { get { return (int)lifeAmount; } }


	//Cache-------------
	float newValue;
	Tween animSlider;

	public void Heal(float amount)
	{
		newValue = slider.value + (amount / 100);

		if (newValue > 1)
			newValue = 1;

		if (animSlider != null)
			animSlider.Kill();

		lifeAmount = newValue * 100;

		animSlider = slider.DOValue(newValue, 1f);

	}

	public void Damage(float amount)
	{

		newValue = slider.value - (amount / 100);

		if (newValue < 0)
			newValue = 0;


		if (animSlider != null)
			animSlider.Kill();

		lifeAmount = newValue * 100f;

		animSlider = slider.DOValue(newValue, 1);

	}

	public void UpdatePercentage()
	{
		float percentageValue = slider.normalizedValue * 100f;

		percentage.text = $" % {(int)percentageValue}";
	}

	public void SetValueLife(int value)
	{
		lifeAmount = value;

		animSlider = slider.DOValue(lifeAmount / 100, 1);
	}

}
