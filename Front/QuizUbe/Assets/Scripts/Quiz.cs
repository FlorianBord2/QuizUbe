using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizResponse
{
	public string userLocalId;
	public string date;
	public string time;
	public string uuid;
	public string userScore;
	public string nbQuestion;

	public QuizResponseStructure[] quiz;

	public class QuizResponseStructure
	{
		public string type;
		public QuizResponseVideo[] videos;
		public string response;
		public string userResponse;
	}

	public class QuizResponseVideo
	{
		public string url;
		public string title;
		public string desc;
		public string view;
		public string like;
		public string dislike;
		public string comment;
	}
}

public class Quiz
{
	public class Question
	{
		public string QuestionType;
		public Video[] Videos;
		public int GoodAnswer;

		public Question(QuizResponse.QuizResponseStructure responseStruc)
		{
			QuestionType = responseStruc.type; //TODO localization

			Videos = new Video[responseStruc.videos.Length];
			for (int i = 0; i < responseStruc.videos.Length; i++)
			{
				Videos[i] = new Video(responseStruc.videos[i]);
			}

			GoodAnswer = int.Parse(responseStruc.response);
		}
	}

	public class Video
	{
		public string MiniatureURL;
		public string Title;
		public string Description;
		public ulong NbViews;
		public ulong NbLikes;
		public ulong NbDislikes;
		public ulong NbComments;

		public Video(QuizResponse.QuizResponseVideo responseVideo)
		{
			MiniatureURL = responseVideo.url;
			Title = responseVideo.title;
			Description = responseVideo.desc;
			NbViews = ulong.Parse(responseVideo.view);
			NbLikes = ulong.Parse(responseVideo.like);
			NbDislikes = ulong.Parse(responseVideo.dislike);
			NbComments = ulong.Parse(responseVideo.comment);
		}
	}

	public Question[] Questions;
	
	public Quiz(QuizResponse quizResponse)
	{
		Questions = new Question[quizResponse.quiz.Length];

		for (int i = 0; i < quizResponse.quiz.Length; i++)
		{
			Questions[i] = new Question(quizResponse.quiz[i]);
		}
	}
}


