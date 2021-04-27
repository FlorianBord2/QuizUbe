using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizHistoResponse
{
	public HistoResponse histo;
	public QuizResponse.QuizResponseStructure[] quiz;
}

public class HistoResponse
{
	public string channelName;
	public string channelUrl;
	public string date;
	public bool defis;
	public bool done;
	public string from;
	public string fromName;
	public int nbQuestion;
	public string time;
	public string to;
	public string toName;
	public int userScore;
	public int userScore2;
	public string uuid;
}

public class QuizResponse
{
	public string userLocalId;
	public string date;
	public string time;
	public string uuid;
	public string channelName;
	public string channelUrl;
	public int userScore;
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

	public Quiz(QuizHistoResponse quizHistoResponse)
	{
		Questions = new Question[quizHistoResponse.quiz.Length];

		for (int i = 0; i < quizHistoResponse.quiz.Length; i++)
		{
			Questions[i] = new Question(quizHistoResponse.quiz[i]);
		}
	}
}
