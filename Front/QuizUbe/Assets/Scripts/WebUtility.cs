﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net.Http;
using System.Text;

public class WebUtility : MonoBehaviour
{
	private static WebUtility _instance;
	public static WebUtility Instance => _instance;


	private static readonly HttpClient client = new HttpClient();

	private void Awake()
	{
		_instance = this;
	}

	public string Get(string uri)
	{
		//TODO update to http client method as http request isn't as performant like following 
		//var responseString = await client.GetStringAsync("http://www.example.com/recepticle.aspx");
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
		request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
		using (Stream stream = response.GetResponseStream())
		using (StreamReader reader = new StreamReader(stream))
		{
			return reader.ReadToEnd();
		}
	}

	public async void Post<T>(string uri, T value)
	{
		var content = new StringContent(value.ToString(), Encoding.UTF8, "application/json");
		var result = await client.PostAsync(uri, content);
	}

	public void GetTextureAsync(string url, Action<Texture> textureDownloadedCallback)
	{
		StartCoroutine(GetTexture(url, textureDownloadedCallback));
	}

	private IEnumerator GetTexture(string url, Action<Texture> textureDownloadedCallback)
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
		yield return www.SendWebRequest();

		textureDownloadedCallback?.Invoke(DownloadHandlerTexture.GetContent(www));
	}
}
