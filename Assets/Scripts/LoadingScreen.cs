using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;


	public static Action<bool> showLoadingScreen = delegate { };


	private void ShowLoadingScreen(bool state)
	{

		canvasGroup.DOFade(AlphaAmount(state), 0.5f);

		canvasGroup.interactable = state;
		canvasGroup.blocksRaycasts = state;
	}

	private int AlphaAmount(bool state) => state ? 1 : 0;


	private void OnEnable()
	{
		showLoadingScreen += ShowLoadingScreen;
	}

	private void OnDisable()
	{
		showLoadingScreen -= ShowLoadingScreen;
	}
}
