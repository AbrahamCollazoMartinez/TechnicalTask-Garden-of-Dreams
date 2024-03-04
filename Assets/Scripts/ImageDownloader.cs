using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

public static class ImageDownloader
{
	public static async Task<(string, Sprite)[]> RequestImages(ImageToDownloadData[] dataUrls)
	{

		List<UniTask<(string, Sprite)>> getSpriteTasks = new List<UniTask<(string, Sprite)>>();

		foreach (var url in dataUrls)
		{
			getSpriteTasks.Add(GetImageFromWebRequest(url.nameID, url.url));
		}

		(string, Sprite)[] sprites = await UniTask.WhenAll(getSpriteTasks);
		LoadingScreen.showLoadingScreen?.Invoke(false);
		return sprites;
	}


	private static async UniTask<(string, Sprite)> GetImageFromWebRequest(string nameResource, string url)
	{
		var unityWebRequestTexture = await UnityWebRequestTexture.
		GetTexture(url)
		.SendWebRequest();

		Texture2D texture = ((DownloadHandlerTexture)unityWebRequestTexture.downloadHandler).texture;
		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());

		return (nameResource, sprite);

	}
}

[Serializable]
public class ImageToDownloadData
{
	public string nameID;
	public string url;
}

