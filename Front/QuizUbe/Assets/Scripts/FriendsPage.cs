using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using Newtonsoft.Json;
using System;

public class PendingChallengesData
{
	public string channelName;
	public string channelUrl;
	public string fromId;
	public string fromName;
	public string quiz_uuid;
}

public class FriendsPage : Page
{
	public FriendBar FriendBarPrefab;
	public HistoCase HistoCasePrefab;

	public InputField AddFriendInputField;
	public Transform FriendsListRoot;
	private const int PENDING_REQUEST_SIBLING_INDEX = 0;
	public Transform FriendsIntercalaire;
	private int FriendsRequestSiblingIndex => FriendsIntercalaire.GetSiblingIndex();

	public GameObject FriendsResponseObject;
	public Text FriendsResponseText;

	private const string ADD_FRIEND_URL = @"http://127.0.0.1:65000/users/add_friend";
	private const string GET_PENDING_REQUEST_URL = @"http://127.0.0.1:65000/users/get_pending_list";
	private const string GET_FRIENDS_URL = @"http://127.0.0.1:65000/users/friend_list";
	private const string GET_CHALLENGES_URL = @"http://127.0.0.1:65000/quiz/get_defis_list";
	private const string ACCEPT_CHALLENGE_URL = @"http://127.0.0.1:65000/quiz/get_defis_quiz";

	private List<FriendBar> _pendingRequests;
	private List<FriendBar> _friends;

	private List<HistoCase> _pendingChallenges;

	public void OnAddFriendButtonClicked()
	{
		string addFriendResponse = WebUtility.Instance.Get(ADD_FRIEND_URL, ("userIdToken", Program.LoginData.localId), ("friend_username", AddFriendInputField.text.Trim(' ')));

		int response = int.Parse(addFriendResponse);

		string display = response > 0 ? "Invite sent" : "User not found";

		FriendsResponseObject.SetActive(true);
		FriendsResponseText.text = display;
		(FriendsResponseObject.transform as RectTransform).DOKill();
		(FriendsResponseObject.transform as RectTransform).DOAnchorPosY(-200f, 0f);
		(FriendsResponseObject.transform as RectTransform).DOAnchorPosY(0f, 0.2f).OnComplete(() => DOVirtual.DelayedCall(2f, () => FriendsResponseObject.SetActive(false)));
	}

	private class FriendsRequestData
	{
		public string name;
		public string userIdToken;
	}


	public override void Open(string value)
	{
		gameObject.SetActive(true);
		_onFriendsList = true;

		RefreshStuff();

		RefreshRequestsList();
		RefreshFriendList();

		RefreshChallengeList();
	}

	private void RefreshChallengeList()
	{
		if (_pendingChallenges == null)
		{
			_pendingChallenges = new List<HistoCase>();
		}

		foreach (HistoCase hc in _pendingChallenges)
		{
			Destroy(hc.gameObject);
		}
		_pendingChallenges.Clear();

		string pendingResponse = WebUtility.Instance.Get(GET_CHALLENGES_URL, ("userLocalId", Program.LoginData.localId));
		IEnumerable<PendingChallengesData> pendingRequests = new List<PendingChallengesData>();
		try
		{
			StringReader reader = new StringReader(pendingResponse);
			JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
			pendingRequests = (IEnumerable<PendingChallengesData>)ser.Deserialize(reader, typeof(IEnumerable<PendingChallengesData>));

			foreach (PendingChallengesData pr in pendingRequests)
			{
				HistoCase fb = Instantiate(HistoCasePrefab, ChallengesList.transform);
				fb.Init(pr.channelName, "", "", "", pr.fromName, true, true);
				fb.OnClicked += () => AcceptChallenge(pr.fromId, pr.quiz_uuid);
				(fb.transform as RectTransform).sizeDelta = new Vector2(848f, (fb.transform as RectTransform).sizeDelta.y);
				_pendingChallenges.Add(fb);
			}
		}
		catch
		{
		}
	}

	private void AcceptChallenge(string from, string uuid)
	{
		string result = WebUtility.Instance.Get(ACCEPT_CHALLENGE_URL, ("fromId", from), ("uuid", uuid));

		StringReader reader = new StringReader(result);
		JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
		QuizHistoResponse histoResponse = (QuizHistoResponse)ser.Deserialize(reader, typeof(QuizHistoResponse));

		Close();
		(Program.QuizPage as QuizPage).OpenAndInitWithQuizHisto(histoResponse);
	}

	private void RefreshRequestsList()
	{
		if (_pendingRequests == null)
		{
			_pendingRequests = new List<FriendBar>();
		}

		foreach(FriendBar fb in _pendingRequests)
		{
			Destroy(fb.gameObject);
		}
		_pendingRequests.Clear();

		string pendingResponse = WebUtility.Instance.Get(GET_PENDING_REQUEST_URL, ("userIdToken", Program.LoginData.localId));
		IEnumerable<FriendsRequestData> pendingRequests = new List<FriendsRequestData>();
		try
		{
			StringReader reader = new StringReader(pendingResponse);
			JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
			pendingRequests = (IEnumerable<FriendsRequestData>)ser.Deserialize(reader, typeof(IEnumerable<FriendsRequestData>));
		}
		catch
		{
		}

		foreach (FriendsRequestData pr in pendingRequests)
		{
			FriendBar fb = Instantiate(FriendBarPrefab, FriendsListRoot);
			fb.transform.SetSiblingIndex(PENDING_REQUEST_SIBLING_INDEX + 1);
			fb.Init(pr.name, pr.userIdToken, true);
			fb.OnInteractionBtnClicked += Fb_OnInteractionBtnClicked;
			_pendingRequests.Add(fb);
		}
	}

	private void RefreshFriendList()
	{
		if (_friends == null)
		{
			_friends = new List<FriendBar>();
		}

		foreach (FriendBar fb in _friends)
		{
			Destroy(fb.gameObject);
		}
		_friends.Clear();

		string friendsResponse = WebUtility.Instance.Get(GET_FRIENDS_URL, ("userIdToken", Program.LoginData.localId));
		IEnumerable<FriendsRequestData> friends = new List<FriendsRequestData>();
		try
		{
			StringReader reader = new StringReader(friendsResponse);
			JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
			friends = (IEnumerable<FriendsRequestData>)ser.Deserialize(reader, typeof(IEnumerable<FriendsRequestData>));
		}
		catch
		{
		}

		foreach (FriendsRequestData pr in friends)
		{
			FriendBar fb = Instantiate(FriendBarPrefab, FriendsListRoot);
			fb.transform.SetSiblingIndex(FriendsRequestSiblingIndex + 1);
			fb.Init(pr.name, pr.userIdToken, false);
			fb.OnChallengeBtnClicked += Fb_OnChallengeBtnClicked;
			_friends.Add(fb);
		}
	}

	private void Fb_OnChallengeBtnClicked(string name, string userIdToken)
	{
		Close();
		Program.SearchPage.Open($"{name},{userIdToken}");
	}

	private void Fb_OnInteractionBtnClicked(bool accepted)
	{
		RefreshRequestsList();
		RefreshFriendList();
	}

	public override void Back()
	{
		Close();
		Program.MainPage.Open(null);
	}

	private bool _onFriendsList = true;

	public void SwapTabs()
	{
		_onFriendsList = !_onFriendsList;
		RefreshStuff();
	}

	public Text TabTitle;
	public GameObject FriendsList;
	public GameObject ChallengesList;
	private void RefreshStuff()
	{
		FriendsList.SetActive(_onFriendsList);
		ChallengesList.SetActive(!_onFriendsList);
		TabTitle.text = _onFriendsList ? "Friends List" : "Challenges";
	}
}
