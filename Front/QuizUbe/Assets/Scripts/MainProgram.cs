using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainProgram : MonoBehaviour
{
	public Page LoginPage;
	public Page MainPage;
	public Page SearchPage;
	public Page QuizPage;
	public Page FriendsPage;
	public Page ResultPage;
	public Page LeaderboardPage;

	private Page[] _pages;

	public Login.LoginData LoginData;

	private void Start()
	{
		//_pages = new Page[] { HomePage, SearchPage, QuizPage, ResultPage, LeaderboardPage };
		_pages = new Page[] { LoginPage, MainPage, SearchPage, QuizPage, FriendsPage}; //Debug

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
