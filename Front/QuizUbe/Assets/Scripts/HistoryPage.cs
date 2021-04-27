using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HistoryPage : Page
{
	public class HistoResponse
	{
		public string channelName;
		public string channelUrl;
		public string date;
		public int nbQuestion;
		public string time;
		public int userScore;
		public int userScore2;
		public string uuid;
		public bool defis;
		public bool done;
		public string from;
		public string fromName;
		public string to;
		public string toName;
	}

	public HistoCase HistoCasePrefab;
	public Transform SpawnRoot;

	private const string GET_QUIZ_HISTO_URL = @"http://127.0.0.1:65000/quiz/get_quiz_histo";

	private List<HistoCase> _cases;
	public override void Open(string value)
	{
		gameObject.SetActive(true);

		string response = WebUtility.Instance.Get(GET_QUIZ_HISTO_URL, ("userLocalId", Program.LoginData.localId));

		StringReader reader = new StringReader(response);
		JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
		IEnumerable<HistoResponse> histos = (IEnumerable<HistoResponse>)ser.Deserialize(reader, typeof(IEnumerable<HistoResponse>));

		if (_cases != null)
		{
			foreach (var c in _cases)
			{
				Destroy(c.gameObject);
			}
		}
		_cases = new List<HistoCase>();

		foreach (HistoResponse hr in histos)
		{
			HistoCase hc = Instantiate(HistoCasePrefab, SpawnRoot);

			if (hr.defis)
			{
				bool receiving = hr.from != Program.LoginData.localId;
				string otherName = receiving ? hr.fromName : hr.toName;

				bool won = receiving ? hr.userScore2 >= hr.userScore : hr.userScore >= hr.userScore2;

				hc.Init(hr.channelName, hr.userScore.ToString(), hr.nbQuestion.ToString(), hr.date, otherName, !hr.done, receiving, won);
			}
			else
			{
				hc.Init(hr.channelName, hr.userScore.ToString(), hr.nbQuestion.ToString(), hr.date);
			}

			_cases.Add(hc);
		}
	}

	public override void Back()
	{
		base.Back();
		Close();
		Program.MainPage.Open(null);
	}
}
