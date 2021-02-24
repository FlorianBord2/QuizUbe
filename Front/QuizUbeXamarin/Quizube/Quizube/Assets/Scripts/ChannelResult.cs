using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelResultData
{
	public string Title;
	public string ChannelID;
	public string URL;
}

public class ChannelResult : MonoBehaviour
{
	public ChannelResultData ChannelResultData;

	public Text ChannelNameText;
	public Text ChannelSubsText;
	public Image ChannelPic;

	public void Init(ChannelResultData data)
	{
		ChannelNameText.text = data.Title;
	}
}
