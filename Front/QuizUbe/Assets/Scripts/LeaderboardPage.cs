using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPage : Page
{
	private class LeaderboardResult
	{
		public string name;
		public string score;
		public string nbQuiz;
	}

	private enum Tab
	{
		None = -1,
		Name,
		ScoreTot,
		TotQuiz
	}

	private Tab _selectedTab = Tab.None;

	public Transform SpawnRoot;

	[System.Serializable]
	public struct OrderButton
	{
		public Button Button;
		public Image Image;
	}

	public OrderButton NameButton;
	public OrderButton ScoreTotButton;
	public OrderButton TotQuizzButton;

	private List<OrderButton> _orderButtons;

	public Color SelectedButtonColor;
	public Color UnselectedButtonColor;

	public ScoreLine ScoreLinePrefab;

	private const string LEADERBOARD_REQUEST = @"";

	private List<ScoreLine> _lines = new List<ScoreLine>();

	public override void Open(string value)
	{
		gameObject.SetActive(true);

		_orderButtons = new List<OrderButton> { NameButton, ScoreTotButton, TotQuizzButton };

		foreach (ScoreLine sl in _lines)
		{
			Destroy(sl.gameObject);
		}

		_lines = new List<ScoreLine>();

		string chars = "0123456789azertyuiopqsdfghjklmwxcvbn";
		for (int i = 0; i < Random.Range(3, 10); i++)
		{
			System.Text.StringBuilder name = new System.Text.StringBuilder();
			for (int y = 0; y < Random.Range(5, 15); y++)
			{
				name.Append(chars[Random.Range(0, chars.Length)]);
			}

			LeaderboardResult result = new LeaderboardResult { name = name.ToString(), score = Random.Range(20, 65000).ToString(), nbQuiz = Random.Range(20, 65000).ToString() };
			ScoreLine line = Instantiate(ScoreLinePrefab, SpawnRoot);
			line.Init(result.name, int.Parse(result.score), int.Parse(result.nbQuiz));
			_lines.Add(line);
		}


		SelectTab(Tab.Name);
	}

	public void OnNameTab()
	{
		SelectTab(Tab.Name);
	}

	public void OnScoreQuizTab()
	{
		SelectTab(Tab.ScoreTot);
	}

	public void OnTotQuizTab()
	{
		SelectTab(Tab.TotQuiz);
	}

	private bool _flipFlop = false;
	private void SelectTab(Tab tab)
	{
		bool reverse = _selectedTab == tab;

		if (!reverse) _flipFlop = false;
		else _flipFlop = !_flipFlop;

		switch (tab)
		{
			case Tab.Name:
				_lines.Sort(delegate (ScoreLine a, ScoreLine b)
				{
					if (_flipFlop)
						return b.Username.CompareTo(a.Username);

					return a.Username.CompareTo(b.Username);
				});
				break;

			case Tab.ScoreTot:
				_lines.Sort(delegate (ScoreLine a, ScoreLine b)
				{
					if (_flipFlop)
						return b.Score.CompareTo(a.Score);
					return a.Score.CompareTo(b.Score);
				});
				break;

			case Tab.TotQuiz:
				_lines.Sort(delegate (ScoreLine a, ScoreLine b)
				{
					if (_flipFlop)
						return b.QuizCount.CompareTo(a.QuizCount);
					return a.QuizCount.CompareTo(b.QuizCount);
				});
				break;
		}

		for (int i = 0; i < _lines.Count; i++)
		{
			_lines[i].transform.SetSiblingIndex(i);
		}

		_selectedTab = tab;

		for (int i = 0; i < _orderButtons.Count; i++)
		{
			bool selected = i == (int)tab;

			_orderButtons[i].Button.image.color = selected ? SelectedButtonColor : UnselectedButtonColor;
			_orderButtons[i].Image.transform.DORotate(new Vector3(0f, 0f, selected && !_flipFlop ? 0f : 180f), 0.3f).SetEase(Ease.InOutBack);
		}
	}

	public override void Back()
	{
		Close();
		Program.MainPage.Open(null);
	}
}
