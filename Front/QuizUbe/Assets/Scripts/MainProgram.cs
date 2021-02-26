using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainProgram : MonoBehaviour
{
	public Page HomePage;
	public Page SearchPage;
	public Page QuizPage;
	public Page ResultPage;
	public Page LeaderboardPage;

	private Page[] _pages;

	private void Start()
	{
		//_pages = new Page[] { HomePage, SearchPage, QuizPage, ResultPage, LeaderboardPage };
		_pages = new Page[] {  SearchPage, QuizPage}; //Debug

		foreach (Page p in _pages)
		{
			p.Program = this;
			p.gameObject.SetActive(p is SearchPage);//TODO Replace this by homepage
		}
	}

	public void StartQuiz(string channelIdx)
	{
		SearchPage.Close();
		QuizPage.Open(channelIdx);
	}
}
