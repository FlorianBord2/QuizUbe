using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPage : Page
{
	private class LeaderboardResult
	{
		public string name;
		public string score;
		public string quiz;
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

	private const string LEADERBOARD_REQUEST = @"http://127.0.0.1:65000/users/leadersboard";

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

		string historyResponse = WebUtility.Instance.Get(LEADERBOARD_REQUEST);

		StringReader reader = new StringReader(historyResponse);
		JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
		List<LeaderboardResult> results = new List<LeaderboardResult>();
		results = (List<LeaderboardResult>)ser.Deserialize(reader, typeof(List<LeaderboardResult>));

		for (int i = 0; i < results.Count; i++)
		{
			ScoreLine line = Instantiate(ScoreLinePrefab, SpawnRoot);
			line.Init(results[i].name, int.Parse(results[i].score), int.Parse(results[i].quiz));
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
						return a.Score.CompareTo(b.Score);
					return b.Score.CompareTo(a.Score);
				});
				break;

			case Tab.TotQuiz:
				_lines.Sort(delegate (ScoreLine a, ScoreLine b)
				{
					if (_flipFlop)
						return a.QuizCount.CompareTo(b.QuizCount);
					return b.QuizCount.CompareTo(a.QuizCount);
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
