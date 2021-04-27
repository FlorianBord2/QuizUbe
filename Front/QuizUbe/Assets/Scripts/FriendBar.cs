using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FriendBar : MonoBehaviour
{
	public Text FriendName;
	public GameObject PendingRoot;
	public GameObject AcceptedRoot;

	private string _name;
	private string _userIdToken;
	private const string INTERACT_FRIEND_URL = "http://127.0.0.1:65000/users/{0}_friend";

	public delegate void FriendBarEvent(bool accepted);
	public event FriendBarEvent OnInteractionBtnClicked;
	public event System.Action<string, string> OnChallengeBtnClicked;

	public void Init(string name, string userIdToken, bool isRequest)
	{
		_name = name;
		_userIdToken = userIdToken;
		FriendName.text = name;

		PendingRoot.SetActive(isRequest);
		AcceptedRoot.SetActive(!isRequest);
	}

	public void OnInteractionButtonClicked(bool accepted)
	{
		PendingRoot.SetActive(false);
		AcceptedRoot.SetActive(true);
		string url = string.Format(INTERACT_FRIEND_URL, accepted ? "accept" : "refuse");

		string response = WebUtility.Instance.Get(url, ("userIdToken",MainProgram.Instance.LoginData.localId), ("friend_username", _name));

		OnInteractionBtnClicked?.Invoke(accepted);
	}

	public void OnChallengeClicked()
	{
		OnChallengeBtnClicked?.Invoke(_name, _userIdToken);
	}
}
