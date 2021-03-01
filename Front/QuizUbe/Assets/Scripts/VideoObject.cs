using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoObject : MonoBehaviour
{
    public delegate void VideoObjectEvent(VideoObject obj);
    public VideoObjectEvent OnVideoObjectClicked;

    public Image Miniature;
    public Text Title;

    public void Init(string title, string miniatureURL)
	{
        Title.text = title;
        WebUtility.Instance.GetTextureAsync(miniatureURL, (texture) => 
        {
            Miniature.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        });
	}

    public void OnClick()
	{
        OnVideoObjectClicked?.Invoke(this);

    }

	internal void Feedback(bool good)
	{
        if (good)
		{
            GetComponent<Image>().DOColor(Color.green, 0.4f);
		}
		else
		{
            GetComponent<Image>().DOColor(Color.red, 0.4f);
        }
    }
}
