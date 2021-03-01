using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestionObject : MonoBehaviour
{
	public delegate void OnQuestionEvent(bool correct);
	public OnQuestionEvent OnQuestionAnswered;

	public Text Question;
	public Transform VideoObjectsRoot;
	public VideoObject VideoObjectPrefab;

	private VideoObject[] _videosObjects;

	private int _goodAnswer;

	public void Init(Quiz.Question question)
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
	}

	private void OnVideoObjectClicked(VideoObject sender)
	{
		bool rightAnswer = Array.IndexOf(_videosObjects, sender) == _goodAnswer;
		sender.Feedback(rightAnswer);

		DOVirtual.DelayedCall(0.5f, () => OnQuestionAnswered?.Invoke(rightAnswer));
	}
}
