using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPage : Page
{
	public InputField AddFriendInputField;

	public void OnAddFriendButtonClicked()
	{

	}

	public override void Open(string value)
	{
		gameObject.SetActive(true);
	}
}
