using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuizPage : Page
{
	private const string CREATE_QUIZ_URL = @"http://127.0.0.1:5000/quiz/create_quiz?channelId=";

	public override void Open(string value)
	{
		string data = WebUtility.Instance.Get(CREATE_QUIZ_URL + value);

		JObject obj = JObject.Parse(data);
		JToken jQuiz = obj["quiz"];

		Quiz.Question[] quizQuestions = jQuiz.ToObject < Quiz.Question[]>();
		Quiz quiz = new Quiz(quizQuestions);
    }
}
