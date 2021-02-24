using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebUtility : MonoBehaviour
{
	private static WebUtility _instance;
	public static WebUtility Instance => _instance;

	private void Awake()
	{
		_instance = this;
	}

	public string Get(string uri)
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
		request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
		using (Stream stream = response.GetResponseStream())
		using (StreamReader reader = new StreamReader(stream))
		{
			return reader.ReadToEnd();
		}
	}

	public IEnumerator GetTexture(string url, Action<Texture> textureDownloadedCallback)
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
		yield return www.SendWebRequest();

		textureDownloadedCallback?.Invoke(DownloadHandlerTexture.GetContent(www));
	}
}
