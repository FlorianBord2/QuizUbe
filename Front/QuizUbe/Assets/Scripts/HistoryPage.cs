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
		public string nbQuestion;
		public string time;
		public string userScore;
		public string uuid;
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
			hc.Init(hr.channelName, hr.userScore, hr.nbQuestion, hr.date);
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
