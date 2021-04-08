using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FriendBar : MonoBehaviour
{
	public Text FriendName;
	public GameObject InteractionButtons;

	private string _name;
	private const string INTERACT_FRIEND_URL = "http://127.0.0.1:65000/users/{0}_friend";

	public delegate void FriendBarEvent(bool accepted);
	public event FriendBarEvent OnInteractionBtnClicked;

	public void Init(string name, bool isRequest)
	{
		_name = name;
		FriendName.text = name;

		InteractionButtons.SetActive(isRequest);
	}

	public void OnInteractionButtonClicked(bool accepted)
	{
		InteractionButtons.SetActive(false);
		string url = string.Format(INTERACT_FRIEND_URL, accepted ? "accept" : "refuse");

		string response = WebUtility.Instance.Get(url, ("userIdToken",MainProgram.Instance.LoginData.localId), ("friend_username", _name));

		OnInteractionBtnClicked?.Invoke(accepted);
	}
}
