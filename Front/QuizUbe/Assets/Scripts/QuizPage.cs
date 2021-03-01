using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class QuizPage : Page
{
	private const string CREATE_QUIZ_URL = @"http://127.0.0.1:5000/quiz/create_quiz?channelId=";
	private const string SAVE_QUIZ_URL = @"http://127.0.0.1:5000/quiz/save_quiz";

	private QuizResponse _quizResponse;
	private Quiz _quiz;

	public Transform QuestionsRoot;

	public QuestionObject QuestionObjectPrefab;

	private QuestionObject[] _questionObjects;

	public Button StartQuizButton;

	private const int NB_QUESTIONS = 4;

	public override void Open(string value)
	{
		gameObject.SetActive(true);

		string data = WebUtility.Instance.Get(CREATE_QUIZ_URL + value);

		StringReader reader = new StringReader(data);
		JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
		QuizResponse response = (QuizResponse)ser.Deserialize(reader, typeof(QuizResponse));

		_quizResponse = response;
		_quiz = new Quiz(response);
		//TODO Wait for quiz in async
    }

	public void OnStartQuizButtonClicked()
	{
		_questionObjects = new QuestionObject[NB_QUESTIONS];
		StartQuizButton.image.DOFade(0f, 0.5f).OnComplete(() => 
		{
			StartQuizButton.gameObject.SetActive(false);

			for (int i = 0; i < NB_QUESTIONS; i++)
			{
				_questionObjects[i] = Instantiate(QuestionObjectPrefab, QuestionsRoot);
				_questionObjects[i].Init(_quiz.Questions[i]);
				_questionObjects[i].gameObject.SetActive(i == 0);
				_questionObjects[i].OnQuestionAnswered += OnQuestionAnswered;
			}
		});
		//TODO add small move maybe?
	}

	private int _questionIdx = 0;
	private int _goodAnswerCount = 0;
	private void OnQuestionAnswered(bool correct)
	{
		_questionIdx++;
		if (correct)
			_goodAnswerCount++;

		for (int i = 0; i < NB_QUESTIONS; i++)
		{
			_questionObjects[i].gameObject.SetActive(i == _questionIdx);
		}

		if (_questionIdx == NB_QUESTIONS)
		{
			QuizCompleted();
		}
	}

	private void QuizCompleted()
	{
		QuizResponse toPost = _quizResponse;
		toPost.userScore = _goodAnswerCount.ToString();

		Debug.Log("localUserId: " + toPost.userLocalId);
		WebUtility.Instance.Post(SAVE_QUIZ_URL, toPost);
	}
}
