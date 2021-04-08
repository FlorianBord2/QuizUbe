using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using Newtonsoft.Json;

public class FriendsPage : Page
{
	public FriendBar FriendBarPrefab;

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

	private List<FriendBar> _pendingRequests;
	private List<FriendBar> _friends;


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

		RefreshRequestsList();

		RefreshFriendList();
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
			fb.Init(pr.name, true);
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
			fb.Init(pr.name, false);
			_friends.Add(fb);
		}
	}

	private void Fb_OnInteractionBtnClicked(bool accepted)
	{
		RefreshRequestsList();
		RefreshFriendList();
	}
}
