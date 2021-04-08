using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestionObject : MonoBehaviour
{
	public delegate void OnQuestionEvent(int answer, bool correct);
	public OnQuestionEvent OnQuestionAnswered;

	public Text Question;
	public Text IndexText;
	public Transform VideoObjectsRoot;
	public VideoObject VideoObjectPrefab;

	private VideoObject[] _videosObjects;

	private int _goodAnswer;

	private bool _clicked = false;

	public void Init(Quiz.Question question, string idxTxt)
	{
		Question.text = question.QuestionType; //TODO LOC

		_videosObjects = new VideoObject[question.Videos.Length];
		for (int i = 0; i < question.Videos.Length; i++)
		{
			_videosObjects[i] = Instantiate(VideoObjectPrefab, VideoObjectsRoot);
			_videosObjects[i].Init(question.Videos[i].Title, question.Videos[i].MiniatureURL);
			_videosObjects[i].OnVideoObjectClicked += OnVideoObjectClicked;
		}
		_goodAnswer = question.GoodAnswer;

		IndexText.text = idxTxt;
	}

	private void OnVideoObjectClicked(VideoObject sender)
	{
		if (_clicked) return;
		_clicked = true;

		int answer = Array.IndexOf(_videosObjects, sender);
		bool rightAnswer = answer == _goodAnswer;
		sender.Feedback(rightAnswer);

		if (!rightAnswer)
		{
			_videosObjects[_goodAnswer].Feedback(true);
		}

		DOVirtual.DelayedCall(0.5f, () => OnQuestionAnswered?.Invoke(answer, rightAnswer));
	}
}
