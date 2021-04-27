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
	private const string CREATE_QUIZ_URL = @"http://127.0.0.1:65000/quiz/create_quiz";
	private const string SAVE_QUIZ_URL = @"http://127.0.0.1:65000/quiz/save_quiz";
	private const string ADD_DEFI_URL = @"http://127.0.0.1:65000/quiz/defis";
	private const string SAVE_DEFI_URL = @"http://127.0.0.1:65000/quiz/save_defis";

	private QuizResponse _quizResponse;
	private QuizHistoResponse _histoQuizResponse;
	private Quiz _quiz;

	public Transform QuestionsRoot;
	public QuestionObject QuestionObjectPrefab;

	private QuestionObject[] _questionObjects;

	public Button StartQuizButton;

	private int _nbQuestions;

	public GameObject QuizRoot;
	public GameObject ResultRoot;

	public Text ResultText;


	private bool _isChallAndImSender = false;
	private bool _isChallAndImReceiver = false;
	public override void Open(string value)
	{

		gameObject.SetActive(true);

		string[] @params = value.Split(';');

		string[] val = @params[0].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

		_isChallAndImSender = int.Parse(@params[1]) == 1;
		_isChallAndImReceiver = false;

		string data = WebUtility.Instance.Get(CREATE_QUIZ_URL, ("channelId", val[0]), ("channelName", val[1]), ("channelUrl", val[2]));

		StringReader reader = new StringReader(data);
		JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
		QuizResponse response = (QuizResponse)ser.Deserialize(reader, typeof(QuizResponse));

		_quizResponse = response;
		_quizResponse.channelName = val[1];
		_quizResponse.channelUrl = val[2];


		_quiz = new Quiz(response);
		_nbQuestions = _quiz.Questions.Length;
		ResultRoot.SetActive(false);
		QuizRoot.SetActive(true);
		//TODO Wait for quiz in async

		StartQuizButton.image.DOFade(1f, 0f);
		StartQuizButton.gameObject.SetActive(true);

		_starting = false;

		if (_questionObjects != null)
		{
			foreach (QuestionObject qo in _questionObjects)
			{
				Destroy(qo.gameObject);
			}
		}
		_questionObjects = new QuestionObject[_nbQuestions];
	}

	public void OpenAndInitWithQuizHisto(QuizHistoResponse quizHisto)
	{
		_isChallAndImSender = false;
		_isChallAndImReceiver = true;
		gameObject.SetActive(true);

		_histoQuizResponse = quizHisto;
		_quiz = new Quiz(quizHisto);
		_nbQuestions = _quiz.Questions.Length;
		ResultRoot.SetActive(false);
		QuizRoot.SetActive(true);
		//TODO Wait for quiz in async

		StartQuizButton.image.DOFade(1f, 0f);
		StartQuizButton.gameObject.SetActive(true);

		_starting = false;

		if (_questionObjects != null)
		{
			foreach (QuestionObject qo in _questionObjects)
			{
				Destroy(qo.gameObject);
			}
		}
		_questionObjects = new QuestionObject[_nbQuestions];
	}

	private bool _starting = false;
	public void OnStartQuizButtonClicked()
	{
		if (_starting) return;

		_starting = true;

		_userResponses = new int[_nbQuestions];
		_questionIdx = 0;
		_goodAnswerCount = 0;

		StartQuizButton.image.DOFade(0f, 0.5f).OnComplete(() => 
		{
			StartQuizButton.gameObject.SetActive(false);

			for (int i = 0; i < _nbQuestions; i++)
			{
				_questionObjects[i] = Instantiate(QuestionObjectPrefab, QuestionsRoot);
				_questionObjects[i].Init(_quiz.Questions[i], $"{i + 1}/{_nbQuestions}");
				_questionObjects[i].gameObject.SetActive(i == 0);
				_questionObjects[i].OnQuestionAnswered += OnQuestionAnswered;
			}
		});
		
		//TODO add small move maybe?
	}

	private int _questionIdx = 0;
	private int _goodAnswerCount = 0;
	private int[] _userResponses;
	private void OnQuestionAnswered(int answer, bool correct)
	{
		_userResponses[_questionIdx] = answer;

		_questionIdx++;
		if (correct)
			_goodAnswerCount++;

		for (int i = 0; i < _nbQuestions; i++)
		{
			_questionObjects[i].gameObject.SetActive(i == _questionIdx);
		}

		if (_questionIdx == _nbQuestions)
		{
			QuizCompleted();
		}
	}

	private void QuizCompleted()
	{
		QuizResponse toPost = _quizResponse;
		
		if (toPost != null)
		{
			toPost.userScore = _goodAnswerCount;
			toPost.userLocalId = Program.LoginData.localId;

			for (int i = 0; i < _nbQuestions; i++)
			{
				toPost.quiz[i].userResponse = _userResponses[i].ToString();
			}
		}

		

		if (_isChallAndImSender)
		{
			WebUtility.Instance.Post(ADD_DEFI_URL, toPost, ("userLocalId", Program.LoginData.localId), ("cibleName", (Program.SearchPage as SearchPage).CredsPair.Value.challName), ("cibleId", (Program.SearchPage as SearchPage).CredsPair.Value.challUserId));
		}
		else if (_isChallAndImReceiver)
		{
			QuizHistoResponse toHistoPost = _histoQuizResponse;
			toHistoPost.histo.userScore2 = _goodAnswerCount;

			for (int i = 0; i < _nbQuestions; i++)
			{
				toHistoPost.quiz[i].userResponse = _userResponses[i].ToString();
			}

			WebUtility.Instance.Post(SAVE_DEFI_URL, toHistoPost, ("userLocalId", Program.LoginData.localId));
		}
		else
		{
			WebUtility.Instance.Post(SAVE_QUIZ_URL, toPost);
		}

		ResultRoot.SetActive(true);
		QuizRoot.SetActive(false);

		ResultText.text = $"{_goodAnswerCount}/{_nbQuestions}";
		ResultText.transform.DOScale(0f, 0f);
		ResultText.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack).SetDelay(1f);

		QuitQuizButton.transform.DOScale(0f, 0f);
		QuitQuizButton.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack).SetDelay(2f);
	}
	public Button QuitQuizButton;
	public void OnEndQuizButtonClicked()
	{
		Close();
		Program.MainPage.Open(null);
	}
}
