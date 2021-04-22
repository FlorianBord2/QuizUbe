using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;

public class SearchPage : Page
{
    public RectTransform ResultsParent;
    public ChannelResult ChannelResultPrefab;

    public InputField SearchField;
    public Button SearchButton;

    private const string SEARCH_CHANNEL_URL = @"http://127.0.0.1:65000/quiz/search_channel?q=";

    private List<ChannelResult> _results;

    public void OnSearchButtonClicked()
	{
        ClearResults();

        _results = new List<ChannelResult>();

        string sanitizedRequest = SearchField.text.Trim(' ');
        string data = WebUtility.Instance.Get(SEARCH_CHANNEL_URL+sanitizedRequest);

        StringReader reader = new StringReader(data);
        JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
        IEnumerable<ChannelResultData> channelInfos = (IEnumerable<ChannelResultData>)ser.Deserialize(reader, typeof(IEnumerable<ChannelResultData>));

        foreach(ChannelResultData crd in channelInfos)
		{
            ChannelResult cr = Instantiate(ChannelResultPrefab, ResultsParent);

            cr.Init(crd);

            cr.onClick += OnResultClicked;

            WebUtility.Instance.GetTextureAsync(crd.URL, (texture) =>
            {
                cr.ChannelPic.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            });

            _results.Add(cr);
        }
    }

	private void OnResultClicked(string channelId, string channelName, string channelUrl)
	{
        Program.StartQuiz(channelId, channelName, channelUrl);
	}

	private void ClearResults()
	{
        if (_results != null)
		{
            for (int i = _results.Count - 1; i >= 0; i--)
            {
                _results[i].onClick -= OnResultClicked;
                Destroy(_results[i].gameObject);
            }
		}
	}

	public override void Open(string value)
	{
        gameObject.SetActive(true);
	}

	public override void Back()
	{
        Close();
        Program.MainPage.Open(null);
	}
}
