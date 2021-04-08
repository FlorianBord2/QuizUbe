using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPage : Page
{
	private enum Tab
	{
		ScoreTot,
		TotQuiz
	}

	public Transform SpawnRoot;

	public Button ScoreTotButton;
	public Button TotQuizzButton;

	public Color SelectedButtonColor;
	public Color UnselectedButtonColor;

	public ScoreLine ScoreLinePrefab;

	private List<ScoreLine> _lines = new List<ScoreLine>();

	public override void Open(string value)
	{
		gameObject.SetActive(true);

		SelectTab(Tab.ScoreTot);
	}

	public void OnScoreQuizTab()
	{
		SelectTab(Tab.ScoreTot);
	}

	public void OnTotQuizTab()
	{
		SelectTab(Tab.TotQuiz);
	}

	private class LeaderboardScore
	{
		private class ScoreData
		{
			public string name;
			public int score;
		}
	}

	private void SelectTab(Tab tab)
	{
		foreach(ScoreLine sl in _lines)
		{
			Destroy(sl.gameObject);
		}
		_lines = new List<ScoreLine>();

		switch (tab)
		{
			case Tab.ScoreTot:

				ScoreTotButton.image.color = SelectedButtonColor;
				TotQuizzButton.image.color = UnselectedButtonColor;
				break;

			case Tab.TotQuiz:

				ScoreTotButton.image.color = UnselectedButtonColor;
				TotQuizzButton.image.color = SelectedButtonColor;

				break;
		}
	}

	public override void Back()
	{
		Close();
		Program.MainPage.Open(null);
	}
}
