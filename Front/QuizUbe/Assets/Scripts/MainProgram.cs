using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainProgram : MonoBehaviour
{
	private static MainProgram _instance;
	public static MainProgram Instance => _instance;


	public Page LoginPage;
	public Page MainPage;
	public Page SearchPage;
	public Page QuizPage;
	public Page FriendsPage;
	public Page LeaderboardPage;

	private Page[] _pages;

	public Login.LoginData LoginData;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		//_pages = new Page[] { HomePage, SearchPage, QuizPage, ResultPage, LeaderboardPage };
		_pages = new Page[] { LoginPage, MainPage, SearchPage, QuizPage, FriendsPage, LeaderboardPage }; //Debug

		foreach (Page p in _pages)
		{
			p.Program = this;
			p.gameObject.SetActive(false);//TODO Replace this by homepage
		}
		LoginPage.Open(null);
		LoginData = new Login.LoginData();
	}

	public void StartQuiz(string channelIdx)
	{
		SearchPage.Close();
		QuizPage.Open(channelIdx);
	}
}
